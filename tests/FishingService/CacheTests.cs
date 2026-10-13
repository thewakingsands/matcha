using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cafe.Matcha.DTO;
using Cafe.Matcha.Network.Fishing;
internal static class CacheTests
{
    private sealed class Api : IFishingApi
    {
        public int Count;
        public TaskCompletionSource<FishingDistribution[]> Pending;
        public Task<FishingDistribution[]> Query(uint place, CancellationToken ct)
        { Count++; Pending = new TaskCompletionSource<FishingDistribution[]>(); return Pending.Task; }
        public Task Upload(byte[] b, long t, CancellationToken c) => Task.CompletedTask;
        public void Dispose() { }
    }
    internal static async Task Run()
    {
        long time = 10000; var api = new Api(); FishingStatisticsDTO snapshot = null;
        var rows = new[] { new FishingDistribution { BaitId = 10, FishId = 30, Count = 50, Tug = 1,
            Range = new DurationRange { Min = 1000, EffectiveMin = 1200, EffectiveMax = 3000, Max = 3500 },
            Percentiles = new DurationPercentiles { Median = 2000 } } };
        void Check(bool value, string label) { if (!value) throw new Exception(label); }
        FishingDTO Cast(string id, uint place, uint bait) => new FishingDTO { CastId = id, Time = time, CastTime = time, Action = "cast", PlaceId = place, BaitId = bait };
        using (var service = new FishingService(api, () => time, p => p == 20 || p == 21, () => null, s => snapshot = s))
        {
            service.SetEnabled(true); service.Observe(Cast("a", 20, 10)); service.Tick();
            Check(api.Count == 1, "coalesce in-flight queries");
            api.Pending.SetResult(rows); await Task.Delay(20);
            Check(snapshot.State == "ready" && snapshot.Distributions.Length == 1, "successful cache");
            service.Observe(Cast("b", 20, 11)); Check(api.Count == 1 && snapshot.State == "empty", "bait switch filters cache only");
            service.Observe(Cast("c", 20, 10)); time += 600001; service.Tick();
            Check(snapshot.State == "refreshing" && snapshot.Distributions.Length == 1, "keep cache during refresh");
            api.Pending.SetException(new FishingApiException(TimeSpan.FromMinutes(3))); await Task.Delay(20);
            Check(snapshot.State == "stale" && snapshot.Distributions.Length == 1, "keep cache on error");
            int before = api.Count; time += 60001; service.Tick(); Check(api.Count == before, "honor retry-after");
            time += 120000; service.Tick(); Check(api.Count == before + 1, "retry only after cooldown");
            var old = api.Pending; service.Observe(Cast("d", 21, 10));
            old.SetResult(rows); await Task.Delay(20);
            Check(snapshot.Cast.PlaceId == 21 && snapshot.Distributions.Length == 0, "old place response cannot replace current");
            api.Pending.SetResult(Array.Empty<FishingDistribution>()); await Task.Delay(20);
            before = api.Count; service.Tick(); Check(api.Count == before && snapshot.State == "empty", "negative cache");
            service.Observe(new FishingDTO { Action = "quit" }); time += 86400001; service.Tick(); Check(api.Count == before, "no idle retries");
            service.Observe(Cast("e", 20, 10)); Check(snapshot.Distributions.Length == 0, "expired cache cannot predict");
            service.Reset(); api.Pending.SetResult(rows); await Task.Delay(20);
            Check(snapshot.Cast == null && snapshot.Distributions.Length == 0, "reset invalidates pending lookup");
        }
    }
}
