namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Utils;

    internal sealed class FishingRuntime : IDisposable
    {
        private readonly FishingDiagnostics diagnostics = new FishingDiagnostics();
        private readonly FishingService service;
        private readonly FishingCredentials credentials;
        private readonly IFishingMemoryReader memory;
        private readonly Timer sampleTimer, serviceTimer;
        private readonly Models.ConfigOverlay config;
        private readonly Action reset;
        private readonly object sampleLock = new object();
        private FishingAttributes snapshot;
        private string processKey;
        private volatile bool disposed;
        public FishingAttributes Snapshot => Volatile.Read(ref snapshot);
        public FishingRuntime(Action reset, Action<FishingStatisticsDTO> publish)
        {
            this.reset = reset;
            memory = new FishingMemoryReader(() => ParsePlugin.Instance?.GetProcess(),
                () => ParsePlugin.Instance?.GetGatheringPoints(), () => Helper.Now, diagnostics);
            config = Config.Instance.Overlay;
            credentials = new FishingCredentials(config.FishingFloatAuth, value => config.FishingFloatAuth = value);
            credentials.SetEnabled(config.FishingFloat);
            service = new FishingService(new FishingApi(credentials: credentials), () => Helper.Now,
                place => Data.Instance.FishingPlaces?.Contains(place) == true, () => Snapshot, publish, diagnostics);
            config.PropertyChanged += SettingsChanged;
            service.SetEnabled(config.FishingFloat);
            sampleTimer = new Timer(_ => Sample(), null, 0, 500);
            serviceTimer = new Timer(_ => Tick(), null, 0, 5000);
        }
        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(config.FishingFloat))
            {
                credentials.SetEnabled(config.FishingFloat);
                service.SetEnabled(config.FishingFloat);
            }
        }
        private void Tick()
        {
            try { if (!disposed) { service.Tick(); diagnostics.State("service-timer", "ready"); } }
            catch (Exception error) { diagnostics.State("service-timer", FishingDiagnostics.Error(error), 'W'); }
        }
        private void ResetSession()
        {
            try { reset(); }
            catch (Exception error) { diagnostics.State("session-reset", FishingDiagnostics.Error(error), 'W'); }
            try { service.Reset(); }
            catch (Exception error) { diagnostics.State("service-reset", FishingDiagnostics.Error(error), 'W'); }
        }
        private void Sample()
        {
            if (!Monitor.TryEnter(sampleLock)) return;
            try
            {
                if (disposed) return;
                var process = ParsePlugin.Instance?.GetProcess();
                var key = process == null || process.HasExited ? null : process.Id + ":" + process.StartTime.Ticks;
                if (key != processKey)
                {
                    processKey = key; Volatile.Write(ref snapshot, null);
                    diagnostics.Event("memory: process-session-changed");
                    ResetSession();
                }
                var value = memory.Read();
                if (!disposed)
                {
                    Volatile.Write(ref snapshot, value);
                    diagnostics.State("attributes", $"gathering={value?.Gathering.HasValue == true}; perception={value?.Perception.HasValue == true}; gp={value?.CurrentGp.HasValue == true}; max-gp={value?.MaxGp.HasValue == true}; bait={value?.BaitId.HasValue == true}");
                }
            }
            catch (Exception error) { Volatile.Write(ref snapshot, null); diagnostics.State("sampling", FishingDiagnostics.Error(error), 'W'); if (!disposed && processKey != null) { processKey = null; ResetSession(); } }
            finally { Monitor.Exit(sampleLock); }
        }
        public void Observe(FishingDTO value)
        {
            if (value.Action == "reset") Volatile.Write(ref snapshot, null);
            service.Observe(value);
        }
        public void Dispose()
        {
            disposed = true; config.PropertyChanged -= SettingsChanged;
            sampleTimer.Dispose(); serviceTimer.Dispose(); service.Dispose();
            lock (sampleLock) { memory.Dispose(); Volatile.Write(ref snapshot, null); }
        }
    }
}
