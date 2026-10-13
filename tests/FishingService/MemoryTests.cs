using System;
using System.IO;
using Cafe.Matcha.Network.Fishing;
internal static class MemoryTests
{
    internal static void Run()
    {
        // Entirely artificial AMD64 PE image with executable and writable data sections.
        var image = new byte[1536];
        void I32(int offset, int value) => BitConverter.GetBytes(value).CopyTo(image, offset);
        void I16(int offset, ushort value) => BitConverter.GetBytes(value).CopyTo(image, offset);
        I16(0, 0x5a4d); I32(0x3c, 64); I32(64, 0x4550); I16(68, 0x8664); I16(70, 2); I16(84, 240);
        I16(88, 0x20b); I32(144, 0x4000);
        const int text = 328, data = 368;
        I32(text + 8, 512); I32(text + 12, 0x1000); I32(text + 16, 512); I32(text + 20, 512); I32(text + 36, 0x60000000);
        I32(data + 8, 0x1000); I32(data + 12, 0x2000); I32(data + 36, 0x40000000);
        var signature = new byte[] { 0x48, 0x8d, 0x0d, 0, 0, 0, 0, 0xe8, 0, 0, 0, 0, 0x84, 0xc0, 0x75, 0x06, 0xf6, 0x43, 0x18, 0x02 };
        signature.CopyTo(image, 512); I32(515, 0x2000 - 0x1007);
        Check(FishingMemoryReader.Locate(image) == 0x2000, "RIP address resolution");
        image[1400] = 1;
        Check(FishingMemoryReader.Locate(image) == 0x2000, "no executable hash gate");
        I32(515, 0x5000 - 0x1007);
        Check(FishingMemoryReader.Locate(image) == null, "out-of-image target rejected");
        I32(515, 0x1000 - 0x1007);
        Check(FishingMemoryReader.Locate(image) == null, "executable target rejected");
        I32(515, 0x2f00 - 0x1007);
        Check(FishingMemoryReader.Locate(image) == null, "state must wholly fit section");
        I32(515, 0x2000 - 0x1007);
        signature.CopyTo(image, 600);
        Check(FishingMemoryReader.Locate(image) == null, "ambiguous signature rejected");
        Array.Clear(image, 600, signature.Length);
        I16(68, 0x14c); Check(FishingMemoryReader.Locate(image) == null, "32-bit images rejected"); I16(68, 0x8664);
        I32(text + 16, int.MaxValue); Check(FishingMemoryReader.Locate(image) == null, "truncated section rejected"); I32(text + 16, 512);
        Array.Clear(image, 512, signature.Length);
        Check(FishingMemoryReader.Locate(image) == null, "missing signature rejected");
        SampleChecks();
    }
    static void Check(bool result, string message) { if (!result) throw new Exception(message); }
    static void SampleChecks()
    {
        var state = new byte[0x4e8]; state[0] = 1; state[0x64] = 1; state[0x7e] = 18;
        BitConverter.GetBytes(200).CopyTo(state, 0x2c8); BitConverter.GetBytes(300).CopyTo(state, 0x2cc);
        BitConverter.GetBytes(10).CopyTo(state, 0x4e4);
        byte[] Read(Tuple<int, int> range) { var b = new byte[range.Item2]; Array.Copy(state, range.Item1, b, 0, b.Length); return b; }
        var result = FishingMemoryReader.Sample(1000, Read, new int?[] { 0, 600, 1 });
        Check(result.Gathering == 200 && result.Perception == 300 && result.CurrentGp == 0 && result.BaitId == 10, "valid zero GP snapshot");
        state[0] = 0; Check(FishingMemoryReader.Sample(1000, Read, null).Gathering == null, "unloaded character"); state[0] = 1;
        state[0x7e] = 1; Check(FishingMemoryReader.Sample(1000, Read, null).Gathering == null, "wrong job"); state[0x7e] = 18;
        Check(FishingMemoryReader.Sample(1000, Read, new int?[] { 0, 600, 2 }).Gathering == null, "ACT character mismatch");
        int reads = 0;
        byte[] Race(Tuple<int, int> r) { if (r.Item1 == 0x64 && ++reads == 2) state[0x64] = 2; return Read(r); }
        Check(FishingMemoryReader.Sample(1000, Race, null).Gathering == null, "character changes during read"); state[0x64] = 1;
        bool failed = false;
        try { FishingMemoryReader.Sample(1000, _ => Array.Empty<byte>(), null); } catch (IOException) { failed = true; }
        Check(failed, "partial reads rejected");
        BitConverter.GetBytes(-1).CopyTo(state, 0x2c8);
        Check(FishingMemoryReader.Sample(1000, Read, null).Gathering == null, "invalid stat unknown");
        Check(result.Gathering == 200, "previous snapshot immutable");
    }
}
