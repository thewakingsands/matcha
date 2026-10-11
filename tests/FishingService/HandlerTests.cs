using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Matcha.DTO;
using Cafe.Matcha.Network;
using Cafe.Matcha.Network.Fishing;
using Cafe.Matcha.Network.Handler;
internal static class HandlerTests
{
    internal static void Run()
    {
        // All bytes are constructed from protocol layout, not captured traffic.
        var events = new List<BaseDTO>(); var handler = new FishingHandler(events.Add); long time = 100000;
        void Check(bool value, string label) { if (!value) throw new Exception(label); }
        Packet Make(ushort opcode, int size, Action<byte[]> fill, bool client = false)
        {
            var bytes = new byte[size + 32]; BitConverter.GetBytes((uint)bytes.Length).CopyTo(bytes, 0);
            BitConverter.GetBytes(1u).CopyTo(bytes, 4); BitConverter.GetBytes(1u).CopyTo(bytes, 8);
            BitConverter.GetBytes((ushort)3).CopyTo(bytes, 12); BitConverter.GetBytes(opcode).CopyTo(bytes, 18);
            fill(bytes); return new Packet(client ? Packet.PacketSender.Client : Packet.PacketSender.Server, bytes, time);
        }
        void U32(byte[] b, int offset, uint v) => BitConverter.GetBytes(v).CopyTo(b, offset + 32);
        void U16(byte[] b, int offset, ushort v) => BitConverter.GetBytes(v).CopyTo(b, offset + 32);
        void Action(uint command) => handler.Handle(Make(0x187, 12, b => { U32(b, 0, 701); U32(b, 4, command); }, true));
        void Play(ushort scene, ushort animation = 0) => handler.Handle(Make(0x1fd, 40, b => { U32(b, 0, 1); U32(b, 8, 0x150001); U16(b, 12, scene); U16(b, 28, animation); }));
        void Log(uint message, uint parameter) => handler.Handle(Make(0xa8, 24, b => { U32(b, 0, 0x150001); U32(b, 4, message); U32(b, 12, parameter); }));
        FishingDTO Last() => events.OfType<FishingDTO>().Last();
        handler.Handle(Make(0x93, 0x30, b => U32(b, 0x2c, 10)));
        Check(Last().BaseBaitId == 10, "login bait");
        handler.Handle(Make(0x83, 384, b => U16(b, 20, 763)));
        Action(0); Play(1); var first = Last();
        Check(first.PlaceId == null && first.Chum == true && first.BaitId == 10, "fresh cast waits for place");
        time += 100; Log(1110, 20);
        Check(Last().Capture.ServerCastTime == time && Last().PlaceId == 20, "server timing and place confirmation");
        time += 1500; Action(22);
        handler.Handle(Make(0x25f, 24, b => { U16(b, 0, 20); U32(b, 4, 3972); U32(b, 8, 1); }));
        Log(5564, 2); Log(5565, 0);
        time += 3000; Play(5, 292); Action(2); Play(6);
        handler.Handle(Make(0x110, 16, b => { U32(b, 0, 30); U16(b, 4, 42); U16(b, 6, 1); U32(b, 12, 16); }));
        var caught = Last();
        Check(caught.Capture.Terminal && caught.Capture.Large == true && caught.Capture.Quantity == 1 && caught.Capture.Size == 42, "catch fields");
        Check(caught.Capture.Lure == 1 && caught.Capture.HiddenStack == 1 && caught.Capture.LureTimes.Length == 1, "confirmed lure metadata");
        Check(first.Capture.LureTimes.Length == 0, "earlier emitted snapshots are immutable");
        Check(FishingUpload.Create(caught, new FishingAttributes(time, 200, 300, 0, 600)) != null, "fully observed cast uploadable");
        time += 1000; Action(3); Play(1); Check(Last().BaitId == null && Last().PlaceId == null, "mooch waits for server");
        Log(1121, 30); Check(Last().BaitId == 30 && Last().PlaceId == 20 && Last().Mooch == true, "server-confirmed mooch");
        time += 3000; Play(5, 293); Action(2); Play(6); Log(3527, 0);
        Check(Last().Capture.GatheringFailed && Last().Capture.Terminal, "failure cause");
        handler.ResetSession(); Check(Last().BaseBaitId == null && Last().PlaceId == null, "reset clears context");
        time += 1000; Action(0); Play(1); Log(1110, 20);
        Check(Last().Chum == null && Last().Capture.Unsupported, "missing status baseline not inferred false");
    }
}
