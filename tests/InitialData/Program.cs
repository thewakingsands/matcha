using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.Matcha.Constant;
using Cafe.Matcha.Models;
using Cafe.Matcha.Network;
using Cafe.Matcha.Network.Handler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

internal static class Program
{
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static Packet PacketWith(int[] fish, int[] spearfish, long time = 1000,
        int length = 2951, MatchaOpcode opcode = MatchaOpcode.PlayerSetup,
        Packet.PacketSender sender = Packet.PacketSender.Server, string name = "Synthetic")
    {
        var bytes = new byte[32 + length];
        bytes[12] = 3;
        var wireOpcode = OpcodeStorage.China.First(p => p.Value == opcode).Key;
        Array.Copy(BitConverter.GetBytes(wireOpcode), 0, bytes, 18, 2);
        if (length >= 2951)
        {
            foreach (var index in fish) bytes[32 + 0x771 + index / 8] |= (byte)(1 << (index % 8));
            foreach (var index in spearfish) bytes[32 + 0x85b + index / 8] |= (byte)(1 << (index % 8));
            var nameBytes = Encoding.UTF8.GetBytes(name);
            Array.Copy(nameBytes, 0, bytes, 32 + 0x34c, Math.Min(32, nameBytes.Length));
        }
        return new Packet(sender, bytes, time);
    }

    private static JArray Completed(InitialDataSnapshot snapshot) =>
        (JArray)JObject.Parse(snapshot.ToFishcakeJson())["completed"];

    public static void Main()
    {
        var mapping = new FishingNotebookData
        {
            Fish = new Dictionary<int, uint> { [0] = 0, [1] = 10002, [7] = 10001, [8] = 10002, [1527] = 10004 },
            Spearfish = new Dictionary<int, uint> { [20000] = 10003, [20007] = 10002, [20008] = 10005, [20303] = 10006 }
        };
        var store = new InitialDataStore();
        var session = store.BeginSession();
        var events = 0;
        var changes = 0;
        store.Changed += (_, __) => changes++;
        var handler = new PlayerSetupHandler(_ => events++, store, () => mapping, session);
        Check(store.Current == null, "No snapshot before PlayerSetup");
        var packet = PacketWith(new[] { 0, 1, 7, 8, 1527 }, new[] { 0, 7, 8, 303 });
        Check(!handler.Handle(packet), "Valid PlayerSetup continues to other observers");
        Check(changes == 1 && events == 0, "Store notification only; no overlay event");
        var captured = store.Current;
        Check(captured.CanExport && captured.CharacterName == "Synthetic" && captured.ReceivedAt == 1000, "Snapshot metadata");
        Check(captured.Fish.SequenceEqual(new uint[] { 10001, 10002, 10004 }), "FishParameter LSB-first bitmap mapping");
        Check(captured.Spearfish.SequenceEqual(new uint[] { 10002, 10003, 10005, 10006 }), "Spearfishing offset and final bit");
        Check(Completed(captured).Values<uint>().SequenceEqual(new uint[] { 10001, 10002, 10003, 10004, 10005, 10006 }), "Unique sorted Item IDs");
        var json = JObject.Parse(captured.ToFishcakeJson());
        Check(json.Properties().Select(p => p.Name).SequenceEqual(new[] { "completed", "pinned", "alarmFish" }), "Only fishcake fields");
        Check(!json["pinned"].Any() && !json["alarmFish"].Any(), "Empty pinned and alarmFish");
        Check(captured.ToFishcakeJson().Contains("\n") && !captured.ToFishcakeJson().Contains("Synthetic"), "Indented export without character metadata");

        var unrelated = PacketWith(Array.Empty<int>(), Array.Empty<int>(), opcode: MatchaOpcode.InitZone);
        handler.Handle(unrelated);
        handler.Handle(PacketWith(Array.Empty<int>(), Array.Empty<int>(), sender: Packet.PacketSender.Client));
        var unknown = PacketWith(Array.Empty<int>(), Array.Empty<int>());
        unknown.Known = false;
        handler.Handle(unknown);
        Check(ReferenceEquals(captured, store.Current), "Zone changes and unrelated packets preserve snapshot");

        handler.Handle(PacketWith(new[] { 7 }, Array.Empty<int>(), time: 2000, length: 2958, name: new string('X', 40)));
        Check(store.Current.CharacterName.Length == 32 && Completed(store.Current).Count == 1, "Bounded name; extended payload replaces snapshot");
        Check(Completed(captured).Count == 6, "Captured export remains immutable after replacement");
        handler.Handle(packet);
        Check(store.Current.ReceivedAt == 2000, "Older packets cannot overwrite a newer snapshot");

        handler.Handle(PacketWith(Array.Empty<int>(), Array.Empty<int>(), time: 3000));
        Check(store.Current.CanExport && Completed(store.Current).Count == 0, "Empty notebook is exportable");
        for (var size = 0; size < 2951; size++)
        {
            Check(handler.Handle(PacketWith(Array.Empty<int>(), Array.Empty<int>(), time: 4000 + size, length: size)), "Truncated setup must not reach other observers");
            Check(!store.Current.CanExport, "Truncated setup invalidates previous snapshot");
        }
        handler.Handle(new Packet(Packet.PacketSender.Server, new byte[12], 8000));
        Check(!store.Current.CanExport, "Invalid headers do not create a snapshot");
        handler.Handle(PacketWith(new[] { 2 }, new[] { 0 }, time: 9000));
        Check(!store.Current.CanExport && store.Current.Error.Contains("映射"), "Missing fish mapping disables export");
        Check(store.Current.Spearfish.Count == 1, "Both bitmaps are inspected");
        handler.Handle(PacketWith(new[] { 1 }, new[] { 1 }, time: 10000));
        Check(!store.Current.CanExport, "Missing spear mapping disables export");
        try { store.Current.ToFishcakeJson(); throw new Exception("Invalid snapshot exported"); }
        catch (InvalidOperationException) { }
        mapping = null;
        handler.Handle(PacketWith(Array.Empty<int>(), Array.Empty<int>(), time: 11000));
        Check(!store.Current.CanExport, "Missing map disables even empty exports");
        mapping = new FishingNotebookData();
        handler.Handle(PacketWith(Array.Empty<int>(), Array.Empty<int>(), time: 12000));
        Check(!store.Current.CanExport, "Invalid map disables export");

        store.EndSession();
        Check(store.Current == null, "Unload clears snapshot");
        store.Replace(session, captured);
        Check(store.Current == null, "Late callbacks cannot restore an unloaded snapshot");
        var nextSession = store.BeginSession();
        store.Replace(session, captured);
        Check(store.Current == null, "Prior-session callbacks cannot populate a reloaded plugin");
        Parallel.For(1, 100, i => store.Replace(nextSession,
            new InitialDataSnapshot("Synthetic", i, new uint[] { (uint)i }, Array.Empty<uint>())));
        Check(store.Current.ReceivedAt == 99 && store.Current.Fish.Single() == 99, "Concurrent snapshots remain consistent and newest wins");

        var dir = Path.Combine(Path.GetTempPath(), "matcha-initial-data-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var path = Path.Combine(dir, "export.json");
            captured.SaveFishcake(path);
            var bytes = File.ReadAllBytes(path);
            Check(bytes[0] == (byte)'{' && Encoding.UTF8.GetString(bytes) == captured.ToFishcakeJson(), "UTF-8 file without BOM");
            var current = store.Current;
            try { captured.SaveFishcake(dir); throw new Exception("Writing to a directory succeeded"); }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
            Check(ReferenceEquals(current, store.Current), "Write failure preserves current snapshot");
        }
        finally { Directory.Delete(dir, true); }

        var shipped = JsonConvert.DeserializeObject<FishingNotebookData>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "fishing-notebook.json")));
        Check(shipped.IsValid && shipped.Fish.Values.Any(id => id > 0) && shipped.Spearfish.Values.Any(id => id > 0), "Bundled offline mapping");
        Console.WriteLine("PASS: synthetic PlayerSetup parsing, mappings, session lifecycle, concurrency and fishcake export");
    }
}
