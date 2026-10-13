namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Cafe.Matcha.DTO;

    internal sealed class FishingService : IDisposable
    {
        private sealed class Cache { public long Time; public FishingDistribution[] Rows; }
        private sealed class Pending { public long Time; public object[] Row; }
        private readonly object sync = new object();
        private readonly IFishingApi api;
        private readonly FishingDiagnostics diagnostics;
        private readonly Func<long> now;
        private readonly Func<uint, bool> knownPlace;
        private readonly Func<FishingAttributes> attributes;
        private readonly Action<FishingStatisticsDTO> publish;
        private readonly Dictionary<uint, Cache> cache = new Dictionary<uint, Cache>();
        private readonly Dictionary<uint, long> retry = new Dictionary<uint, long>();
        private readonly HashSet<uint> loading = new HashSet<uint>();
        private readonly Queue<Pending> pending = new Queue<Pending>();
        private CancellationTokenSource cancellation = new CancellationTokenSource();
        private bool enabled, disposed, uploading;
        private long revision, generation, nextUpload, uploadRetry;
        private readonly string session = Guid.NewGuid().ToString("N");
        private FishingDTO current;
        private FishingAttributes castAttributes;
        private bool eligible;
        private string uploadState = "idle";

        public FishingService(IFishingApi api, Func<long> now, Func<uint, bool> knownPlace,
            Func<FishingAttributes> attributes, Action<FishingStatisticsDTO> publish, FishingDiagnostics diagnostics = null)
        { this.diagnostics = diagnostics ?? new FishingDiagnostics(); this.api = api; this.now = now; this.knownPlace = knownPlace; this.attributes = attributes; this.publish = publish; }

        public void SetEnabled(bool value)
        {
            lock (sync)
            {
                if (disposed || enabled == value) return;
                enabled = value;
                diagnostics.Event(value ? "service: enabled" : "service: disabled");
                ResetRequests();
                if (value) { nextUpload = now() + 60000; QueryIfNeeded(); }
                Publish();
            }
        }
        private void ResetRequests()
        {
            diagnostics.Event($"service: cancel-and-clear; queued={pending.Count}; queries={loading.Count}; uploading={uploading}");
            generation++;
            cancellation.Cancel(); cancellation.Dispose(); cancellation = new CancellationTokenSource();
            loading.Clear(); pending.Clear(); retry.Clear(); cache.Clear();
            uploading = false; eligible = false; castAttributes = null; uploadRetry = 0; uploadState = "idle";
        }
        public void Reset()
        {
            lock (sync) { if (disposed) return; ResetRequests(); current = null; nextUpload = now() + 60000; Publish(); }
        }
        public void Observe(FishingDTO value)
        {
            lock (sync)
            {
                if (disposed) return;
                if (value.Action == "reset") { Reset(); return; }
                if (value.Action == "quit") { current = null; eligible = false; Publish(); return; }
                if (value.Action == "bait") return; // Never change the bait snapshot of an already active cast.
                if (value.Action == "cast")
                {
                    if (current?.CastId == value.CastId) return;
                    castAttributes = attributes();
                    if (castAttributes != null && (now() - castAttributes.SampledAt > 2000 || now() < castAttributes.SampledAt)) castAttributes = null;
                    diagnostics.State("cast-attributes", castAttributes == null ? "missing-or-stale" :
                        $"gathering={castAttributes.Gathering.HasValue}; perception={castAttributes.Perception.HasValue}; gp={castAttributes.CurrentGp.HasValue}; max-gp={castAttributes.MaxGp.HasValue}; bait={castAttributes.BaitId.HasValue}");
                    eligible = enabled;
                }
                else if (current?.CastId != value.CastId) return;
                current = value;
                if (eligible && value.Capture?.Terminal == true)
                {
                    eligible = false;
                    if (Known(value.PlaceId))
                    {
                        var row = FishingUpload.Create(value, castAttributes);
                        if (row != null)
                        {
                            while (pending.Count >= 120) { pending.Dequeue(); diagnostics.Event("upload: queue-limit-drop", 'W'); }
                            pending.Enqueue(new Pending { Time = now(), Row = row });
                            diagnostics.Event($"upload: queued; count={pending.Count}");
                        }
                        else { uploadState = "incomplete"; diagnostics.State("upload-record", "incomplete-skip"); }
                    }
                    else diagnostics.State("upload-record", "unknown-place-skip");
                }
                QueryIfNeeded(); Publish();
            }
        }
        private bool Known(uint? place) => place.HasValue && place != 0 && place != 950 && knownPlace(place.Value);
        private bool Active => current != null && current.Action != "catch" && current.Action != "end";
        private void QueryIfNeeded()
        {
            if (!enabled || !Active) return;
            if (!Known(current.PlaceId) || !current.BaitId.HasValue || current.BaitId == 0)
            { diagnostics.State("query", "waiting-for-place-or-bait"); return; }
            uint place = current.PlaceId.Value;
            var time = now();
            if (cache.TryGetValue(place, out var cached) && time - cached.Time < (cached.Rows.Length == 0 ? 300000 : 600000)) { diagnostics.State("query", "cache-hit"); return; }
            if (loading.Contains(place) || retry.TryGetValue(place, out var next) && next > time) return;
            diagnostics.State("query", "request-start");
            loading.Add(place);
            _ = Query(place, generation, cancellation.Token);
        }
        private async Task Query(uint place, long capturedGeneration, CancellationToken token)
        {
            FishingDistribution[] rows = null;
            long delay = 60000;
            try { rows = await api.Query(place, token).ConfigureAwait(false); }
            catch (FishingApiException e) { delay = (long)Math.Min(86400000, e.RetryAfter.TotalMilliseconds); diagnostics.Event($"query: {FishingDiagnostics.Error(e)}; retry-ms={delay}", 'W'); }
            catch (Exception error) { diagnostics.Event("query: " + (token.IsCancellationRequested ? "cancelled" : FishingDiagnostics.Error(error)), token.IsCancellationRequested ? 'I' : 'W'); }
            lock (sync)
            {
                if (disposed || !enabled || capturedGeneration != generation) { diagnostics.Event("query: obsolete-result-ignored"); return; }
                loading.Remove(place);
                if (rows != null)
                {
                    if (cache.Count >= 32 && !cache.ContainsKey(place)) cache.Remove(cache.OrderBy(p => p.Value.Time).First().Key);
                    cache[place] = new Cache { Time = now(), Rows = rows }; retry.Remove(place);
                    diagnostics.Event($"query: completed; distributions={rows.Length}");
                }
                else { retry[place] = now() + delay; diagnostics.State("query", "failed-cooldown", 'W'); }
                Publish();
            }
        }
        public void Tick()
        {
            lock (sync)
            {
                if (disposed) return;
                var time = now();
                int expired = 0;
                while (pending.Count > 0 && time - pending.Peek().Time >= 600000) { pending.Dequeue(); expired++; }
                if (expired > 0) diagnostics.Event($"upload: expired; count={expired}");
                QueryIfNeeded();
                if (enabled && !uploading && pending.Count > 0 && time >= nextUpload && time >= uploadRetry)
                {
                    // Remove only this immutable batch before sending; never resend ambiguous outcomes.
                    var batch = pending.Select(p => p.Row).ToArray(); pending.Clear();
                    uploading = true; uploadState = "sending"; nextUpload = time + 60000;
                    diagnostics.Event($"upload: request-start; count={batch.Length}");
                    _ = Upload(batch, generation, cancellation.Token);
                }
                Publish();
            }
        }
        private async Task Upload(object[][] batch, long capturedGeneration, CancellationToken token)
        {
            bool success = false;
            long delay = 60000;
            try { await api.Upload(FishingUpload.Encode(batch), now(), token).ConfigureAwait(false); success = true; }
            catch (FishingApiException e) { delay = (long)Math.Min(86400000, e.RetryAfter.TotalMilliseconds); diagnostics.Event($"upload: {FishingDiagnostics.Error(e)}; retry-ms={delay}; batch-not-retried", 'W'); }
            catch (Exception error) { diagnostics.Event("upload: " + (token.IsCancellationRequested ? "cancelled" : FishingDiagnostics.Error(error)) + "; batch-not-retried", token.IsCancellationRequested ? 'I' : 'W'); }
            lock (sync)
            {
                if (disposed || !enabled || capturedGeneration != generation) { diagnostics.Event("upload: obsolete-result-ignored"); return; }
                uploading = false; uploadState = success ? "ok" : "error";
                if (success) diagnostics.Event($"upload: completed; count={batch.Length}");
                if (!success) uploadRetry = now() + delay;
                Publish();
            }
        }
        private void Publish()
        {
            var snapshot = new FishingStatisticsDTO { Enabled = enabled, Session = session, Revision = ++revision,
                Cast = current, UploadState = uploadState };
            if (enabled)
            {
                snapshot.State = "waiting";
                if (Known(current?.PlaceId) && current?.BaitId > 0)
                {
                    var place = current.PlaceId.Value;
                    bool available = cache.TryGetValue(place, out var cached) && now() - cached.Time < 86400000;
                    if (available)
                    {
                        snapshot.CachedAt = cached.Time;
                        snapshot.Distributions = cached.Rows.Where(r => r.BaitId == current.BaitId).ToArray();
                    }
                    snapshot.State = loading.Contains(place) ? (available ? "refreshing" : "loading")
                        : retry.ContainsKey(place) ? (available ? "stale" : "error")
                        : available ? (snapshot.Distributions.Length == 0 ? "empty" : "ready") : "waiting";
                }
            }
            try { publish(snapshot); }
            catch (Exception error) { diagnostics.State("statistics-event", FishingDiagnostics.Error(error), 'W'); }
        }
        public void Dispose()
        {
            lock (sync)
            {
                if (disposed) return;
                enabled = false; ResetRequests(); current = null; Publish();
                disposed = true; cancellation.Dispose(); api.Dispose();
            }
        }
    }
}
