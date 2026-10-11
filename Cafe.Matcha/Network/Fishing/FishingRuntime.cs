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
        private readonly FishingService service;
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
                () => ParsePlugin.Instance?.GetGatheringPoints(), () => Helper.Now);
            service = new FishingService(new FishingApi(), () => Helper.Now,
                place => Data.Instance.FishingPlaces?.Contains(place) == true, () => Snapshot, publish);
            config = Config.Instance.Overlay;
            config.PropertyChanged += SettingsChanged;
            service.SetEnabled(config.FishingFloat);
            sampleTimer = new Timer(_ => Sample(), null, 0, 500);
            serviceTimer = new Timer(_ => Tick(), null, 0, 5000);
        }
        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        { if (e.PropertyName == nameof(config.FishingFloat)) service.SetEnabled(config.FishingFloat); }
        private void Tick() { try { if (!disposed) service.Tick(); } catch { } }
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
                    reset(); service.Reset();
                }
                var value = memory.Read();
                if (!disposed) Volatile.Write(ref snapshot, value);
            }
            catch { Volatile.Write(ref snapshot, null); if (!disposed) { reset(); service.Reset(); } }
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
