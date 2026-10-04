using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Matcha.Constant;
using Cafe.Matcha.DTO;
using Cafe.Matcha.Network;
using Cafe.Matcha.Network.Handler;

internal static class Program
{
    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
    }
    private static Packet Packet(MatchaOpcode type, byte[] body, Packet.PacketSender sender = Cafe.Matcha.Network.Packet.PacketSender.Server)
    {
        var bytes = new byte[32 + body.Length];
        bytes[12] = 3;
        var opcode = OpcodeStorage.China.Single(pair => pair.Value == type).Key;
        BitConverter.GetBytes(opcode).CopyTo(bytes, 18);
        body.CopyTo(bytes, 32);
        return new Packet(sender, bytes) { Known = true, MatchaOpcode = type };
    }
    private static byte[] Zone(ushort id)
    {
        var body = new byte[136];
        BitConverter.GetBytes(id).CopyTo(body, 2);
        return body;
    }
    private static byte[] Item(ushort slot, ushort condition, ushort container = 25004, uint itemId = 700001)
    {
        var body = new byte[64];
        BitConverter.GetBytes(container).CopyTo(body, 8);
        BitConverter.GetBytes(slot).CopyTo(body, 10);
        BitConverter.GetBytes(1u).CopyTo(body, 12);
        BitConverter.GetBytes(itemId).CopyTo(body, 16);
        BitConverter.GetBytes(condition).CopyTo(body, 34);
        BitConverter.GetBytes((ushort)777).CopyTo(body, 36);
        return body;
    }
    public static void Main()
    {
        var events = new List<BaseDTO>();
        var handler = new SubmarineHandler(events.Add);
        handler.Handle(Packet(MatchaOpcode.ItemInfo, Item(0, 12300)));
        Check(events.Count == 0, "Inventory outside workshop must be ignored");
        Check(!handler.Handle(Packet(MatchaOpcode.InitZone, Zone(423))), "Normal zone handling must continue");
        Check(((SubmarinePartsDTO)events.Last()).Reset, "Zone reset event");
        events.Clear();
        for (var index = 0; index < 4; ++index)
        {
            for (var part = 0; part < 4; ++part)
            {
                handler.Handle(Packet(MatchaOpcode.ItemInfo, Item((ushort)(index * 5 + part), (ushort)(12000 + part), itemId: (uint)(700001 + part))));
                var e = (SubmarinePartsDTO)events.Last();
                Check(e.Index == index && e.Parts[part].Condition == 12000 + part && e.Parts[part].ItemId == 700001 + part, "Five-slot stride and part order");
            }
        }
        var old = (SubmarinePartsDTO)events.Last();
        handler.Handle(Packet(MatchaOpcode.UpdateInventorySlot, Item(18, 30000)));
        var repaired = (SubmarinePartsDTO)events.Last();
        Check(repaired.Index == 3 && repaired.Parts[3].Condition == 30000, "Repair update");
        Check(old.Parts[3].Condition == 12003, "Published snapshots cannot change");
        handler.Handle(Packet(MatchaOpcode.UpdateInventorySlot, Item(18, 0)));
        Check(((SubmarinePartsDTO)events.Last()).Parts[3].Condition == 0, "Zero is known broken, not missing");
        var count = events.Count;
        handler.Handle(Packet(MatchaOpcode.ItemInfo, Item(4, 12300)));
        handler.Handle(Packet(MatchaOpcode.ItemInfo, Item(20, 12300)));
        handler.Handle(Packet(MatchaOpcode.ItemInfo, Item(0, 12300, 25003)));
        handler.Handle(Packet(MatchaOpcode.ItemInfo, Item(0, 12300).Take(63).ToArray()));
        handler.Handle(Packet(MatchaOpcode.ItemInfo, Item(0, 12300), Cafe.Matcha.Network.Packet.PacketSender.Client));
        Check(events.Count == count, "Reserved slot, invalid index, other container, truncation and direction");
        handler.Handle(Packet(MatchaOpcode.UpdateInventorySlot, Item(18, 0, itemId: 0)));
        Check(((SubmarinePartsDTO)events.Last()).Parts[3] == null, "Removed item clears condition");
        handler.Handle(Packet(MatchaOpcode.UpdateInventorySlot, Item(18, ushort.MaxValue)));
        Check(((SubmarinePartsDTO)events.Last()).Parts[3] == null, "Invalid condition clears stale value");
        handler.Handle(Packet(MatchaOpcode.InitZone, Zone(128)));
        count = events.Count;
        handler.Handle(Packet(MatchaOpcode.ItemInfo, Item(0, 12300)));
        Check(events.Count == count, "Shared furniture container must not be treated as workshop parts");
        handler.Handle(Packet(MatchaOpcode.SubmarineStatusList, new byte[240]));
        handler.Handle(Packet(MatchaOpcode.UpdateInventorySlot, Item(0, 15000)));
        Check(((SubmarinePartsDTO)events.Last()).Parts[1] == null, "Re-entry cannot reuse old conditions");
        handler.Handle(Packet(MatchaOpcode.PlayerSetup, new byte[0]));
        Check(((SubmarinePartsDTO)events.Last()).Reset, "Login clears conditions");
        handler.Handle(Packet(MatchaOpcode.Logout, new byte[8]));
        Check(((SubmarinePartsDTO)events.Last()).Reset, "Logout clears conditions");
        var json = repaired.ToJSON();
        Check(json.Contains("\"parts\"") && json.Contains("\"condition\":30000"), "Overlay JSON contract");
        Console.WriteLine("Synthetic submarine condition checks passed.");
    }
}
