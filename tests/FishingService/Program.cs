using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cafe.Matcha.DTO;
using Cafe.Matcha.Network.Fishing;

internal static class Program
{
    static void Check(bool condition, string label) { if (!condition) throw new Exception(label); }
    internal sealed class Api : IFishingApi
    {
        public int Queries, Uploads;
        public TaskCompletionSource<FishingDistribution[]> Next = new TaskCompletionSource<FishingDistribution[]>();
        public TaskCompletionSource<bool> Sent = new TaskCompletionSource<bool>();
        public Task<FishingDistribution[]> Query(uint p, CancellationToken ct) { Queries++; return Next.Task; }
        public Task Upload(byte[] b, long t, CancellationToken ct) { Uploads++; return Sent.Task; }
        public void Dispose() { }
    }
    static FishingDTO Cast(string id, long t, string action = "cast", uint? place = null) => new FishingDTO {
        CastId = id, Time = t, Action = action, CastTime = t, PlaceId = place, BaitId = 10,
        Chum = false, Mooch = false, Statuses = Array.Empty<uint>(),
        Capture = new FishingCapture { ServerCastTime = t, Hook = 1 },
    };
    static FishingDTO Catch(string id, long t)
    {
        var c = Cast(id, t, "catch", 20); c.FishId = 30; c.Tug = 1; c.BiteTime = t + 5000;
        c.Capture.Terminal = true; c.Capture.Size = 42; c.Capture.Quantity = 1; c.Capture.Large = false; return c;
    }
    static async Task Main()
    {
        long time = 1000000;
        var api = new Api(); FishingStatisticsDTO last = null;
        var logs = new List<string>();
        var diagnostics = new FishingDiagnostics((_, text) => logs.Add(text));
        var stats = new FishingAttributes(time, 200, 300, 0, 600, 10);
        using (var service = new FishingService(api, () => time, p => p == 20, () => stats, s => last = s, diagnostics))
        {
            service.Observe(Cast("off", time, place: 20)); service.Tick(); Check(api.Queries == 0, "opt-in required");
            service.SetEnabled(true); Check(api.Queries == 1, "enable queries current spot");
            service.SetEnabled(false);
            api.Next.SetResult(Array.Empty<FishingDistribution>()); await Task.Delay(20);
            Check(!last.Enabled && last.Distributions.Length == 0, "late response cannot restore disabled data");
            api.Next = new TaskCompletionSource<FishingDistribution[]>();
            service.SetEnabled(true); service.Observe(Catch("off", time));
            time += 61000; service.Tick(); Check(api.Uploads == 0, "never backfill pre-consent cast");
            service.Reset(); service.Observe(Cast("unknown", time, place: 950)); Check(last.State == "waiting", "unknown spot blocked");
            var queries = api.Queries; service.Tick(); Check(queries == api.Queries, "unknown spot has no query");
            stats = new FishingAttributes(time, 200, 300, 0, 600, 10);
            service.Observe(Cast("new", time)); service.Observe(Cast("new", time, "place", 20));
            Check(last.State == "loading", "pending lookup");
            api.Next.SetResult(Array.Empty<FishingDistribution>()); await Task.Delay(20);
            Check(last.State == "empty", "empty response distinct from error");
            service.Observe(Catch("new", time)); service.Observe(Catch("new", time));
            time += 60000; service.Tick(); Check(api.Uploads == 1, "single immutable upload batch");
            stats = new FishingAttributes(time, 200, 300, 0, 600, 10);
            service.Observe(Cast("next", time, place: 20)); service.Observe(Catch("next", time));
            api.Sent.SetResult(true); await Task.Delay(20);
            api.Sent = new TaskCompletionSource<bool>(); time += 60000; service.Tick();
            Check(api.Uploads == 2, "new records survive previous batch completion");
            service.SetEnabled(false); api.Sent.SetException(new Exception()); await Task.Delay(20);
            Check(last.State == "disabled", "upload failure cannot re-enable state");
        }
        Check(logs.Any(s => s.StartsWith("query: completed")), "query success diagnostics");
        Check(logs.Any(s => s.StartsWith("upload: completed")), "upload success diagnostics");
        Check(logs.Any(s => s.Contains("obsolete-result-ignored")), "late result diagnostics");
        Check(!logs.Any(s => s.Contains("actorId") || s.Contains("castId")), "diagnostic field boundary");
        var record = Catch("artificial", time);
        Check(!record.ToJSON().Contains("actorId") && !record.ToJSON().Contains("Capture"), "public field boundary");
        var row = FishingUpload.Create(record, new FishingAttributes(time, 200, 300, 0, 600));
        Check(row != null && (int)((object[])row[11])[2] == 0, "zero GP is valid");
        Check(FishingUpload.Create(record, new FishingAttributes(time)) == null, "unknown stats are not zero");
        Check(FishingUpload.Encode(new[] { row })[0] == 0x81, "CBOR batch array");
        // Independent, entirely artificial CBOR contract vector exercises all wire scalar widths.
        var encoded = FishingUpload.Encode(new[] { new object[] {
            0, 23, 24, 256, 65536, 4294967296L, true, false, null, new object[] { 1, 2 } } });
        Check(BitConverter.ToString(encoded) ==
            "81-8A-00-17-18-18-19-01-00-1A-00-01-00-00-1B-00-00-00-01-00-00-00-00-F5-F4-F6-82-01-02",
            "CBOR canonical batch contract");
        Check(FishingMemoryReader.Locate(new byte[64]) == null, "invalid PE rejected");
        await HttpTests.Run();
        await CacheTests.Run();
        HandlerTests.Run();
        MemoryTests.Run();
        DiagnosticsTests.Run();
        Console.WriteLine("Fishing service artificial-data tests passed");
    }
}
