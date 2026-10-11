using System;
using Cafe.Matcha.Network.Fishing;
internal static class MemoryTests
{
    internal static void Run()
    {
        // Artificial PE image: one executable section and RIP-relative static address signature.
        var image = new byte[1024];
        void I32(int offset, int value) => BitConverter.GetBytes(value).CopyTo(image, offset);
        void I16(int offset, short value) => BitConverter.GetBytes(value).CopyTo(image, offset);
        I32(0x3c, 64); I32(64, 0x4550); I16(70, 1); I16(84, 0);
        I32(100, 0x1000); I32(104, 512); I32(108, 256); I32(124, 0x20000000);
        var signature = new byte[] { 0x48, 0x8d, 0x0d, 0, 0, 0, 0, 0xe8, 0, 0, 0, 0, 0x84, 0xc0, 0x75, 0x06, 0xf6, 0x43, 0x18, 0x02 };
        signature.CopyTo(image, 256); I32(259, 0x2000 - 0x1007);
        if (FishingMemoryReader.Scan(image) != 0x2000) throw new Exception("RIP address resolution");
        signature.CopyTo(image, 300);
        if (FishingMemoryReader.Scan(image) != null) throw new Exception("ambiguous signature must fail closed");
        if (FishingMemoryReader.Locate(image) != null) throw new Exception("unknown build is unsupported");
    }
}
