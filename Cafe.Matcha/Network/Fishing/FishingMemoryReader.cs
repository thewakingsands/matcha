namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;

    internal sealed class FishingMemoryReader : IFishingMemoryReader
    {
        private readonly Func<Process> process;
        private readonly Func<int?[]> gp;
        private readonly Func<long> now;
        private string identity;
        private long playerState;
        private uint previousActor;
        public FishingMemoryReader(Func<Process> process, Func<int?[]> gp, Func<long> now)
        { this.process = process; this.gp = gp; this.now = now; }

        public FishingAttributes Read()
        {
            var time = now();
            try
            {
                var target = process();
                if (target == null || target.HasExited) { identity = null; playerState = 0; return new FishingAttributes(time); }
                var key = target.Id + ":" + target.StartTime.ToUniversalTime().Ticks + ":" + target.MainModule.FileName;
                if (identity != key)
                {
                    identity = key; playerState = 0; previousActor = 0;
                    var image = File.ReadAllBytes(target.MainModule.FileName);
                    var rva = Locate(image);
                    if (rva.HasValue) playerState = target.MainModule.BaseAddress.ToInt64() + rva.Value;
                }
                var points = gp();
                if (playerState == 0) return new FishingAttributes(time, gp: points?[0], maxGp: points?[1]);
                var handle = OpenProcess(0x10, false, target.Id); // PROCESS_VM_READ only.
                if (handle == IntPtr.Zero) return new FishingAttributes(time);
                try
                {
                    var loaded = Read(handle, playerState, 1)[0];
                    var actor = BitConverter.ToUInt32(Read(handle, playerState + 0x64, 4), 0);
                    if (loaded == 0 || actor == 0 || Read(handle, playerState + 0x7e, 1)[0] != 18) { previousActor = 0; return new FishingAttributes(time); }
                    if (previousActor != 0 && previousActor != actor) { previousActor = actor; return new FishingAttributes(time); }
                    previousActor = actor;
                    if (points?.Length >= 3 && points[2].HasValue && unchecked((uint)points[2].Value) != actor) return new FishingAttributes(time);
                    // PlayerAttribute.Gathering=72, Perception=73; adjacent signed 32-bit attributes.
                    var stats = Read(handle, playerState + 0x1a8 + 72 * 4, 8);
                    var bait = BitConverter.ToUInt32(Read(handle, playerState + 0x4e4, 4), 0);
                    var gathering = BitConverter.ToInt32(stats, 0); var perception = BitConverter.ToInt32(stats, 4);
                    if (target.HasExited || Read(handle, playerState, 1)[0] == 0 || BitConverter.ToUInt32(Read(handle, playerState + 0x64, 4), 0) != actor)
                        return new FishingAttributes(time);
                    return new FishingAttributes(time, Positive(gathering), Positive(perception), points?[0], points?[1], bait == 0 ? (uint?)null : bait);
                }
                finally { CloseHandle(handle); }
            }
            // A transient read failure must not rescan the executable on the next sample.
            catch { return new FishingAttributes(time); }
        }
        private static int? Positive(int value) => value > 0 && value < 100000 ? (int?)value : null;

        // Fixed build allowlist, not a promise that an upstream layout applies to every region.
        // FFXIVClientStructs 25762ad04: PlayerState.Instance/Attributes/FishingBait.
        // Both images: unique static signature and GetAttributeByIndex indexed load at +0x1A8.
        // CN HandlePlayerSetup stores payload+0x2C at PlayerState+0x4E4.
        internal static uint? Locate(byte[] image)
        {
            string hash;
            using (var sha = SHA256.Create()) hash = BitConverter.ToString(sha.ComputeHash(image)).Replace("-", "").ToLowerInvariant();
            if (hash != "7ba28760bc53cbbbf66b63105fea6c1bfe09cb6b1c6353fc8fb0ff03cc1a30f4"
                && hash != "2d5457305db876f4bf68080f7703dd187375cd7e2fd607fa211b3b3c204e4b25") return null;
            return Scan(image);
        }
        internal static uint? Scan(byte[] image)
        {
            int?[] signature = { 0x48, 0x8d, 0x0d, null, null, null, null, 0xe8, null, null, null, null,
                0x84, 0xc0, 0x75, 0x06, 0xf6, 0x43, 0x18, 0x02 };
            if (image.Length < 64) return null;
            var pe = BitConverter.ToInt32(image, 0x3c);
            if (pe < 0 || pe > image.Length - 24 || BitConverter.ToUInt32(image, pe) != 0x4550) return null;
            int sections = BitConverter.ToUInt16(image, pe + 6);
            int table = pe + 24 + BitConverter.ToUInt16(image, pe + 20);
            if (table < 0 || (long)table + sections * 40 > image.Length) return null;
            uint? found = null;
            for (int section = 0; section < sections; section++)
            {
                int entry = table + section * 40;
                if ((BitConverter.ToUInt32(image, entry + 36) & 0x20000000) == 0) continue;
                uint va = BitConverter.ToUInt32(image, entry + 12), size = BitConverter.ToUInt32(image, entry + 16), raw = BitConverter.ToUInt32(image, entry + 20);
                if ((long)raw + size > image.Length) return null;
                for (long offset = raw; offset + signature.Length <= (long)raw + size; offset++)
                {
                    int i = 0;
                    while (i < signature.Length && (!signature[i].HasValue || image[offset + i] == signature[i])) i++;
                    if (i != signature.Length) continue;
                    if (found.HasValue) return null;
                    var address = (long)va + offset - raw + 7 + BitConverter.ToInt32(image, (int)offset + 3);
                    if (address < 0 || address > uint.MaxValue) return null;
                    found = (uint)address;
                }
            }
            return found;
        }
        private static byte[] Read(IntPtr handle, long address, int size)
        {
            var buffer = new byte[size];
            if (!ReadProcessMemory(handle, new IntPtr(address), buffer, size, out var read) || read.ToInt64() != size) throw new IOException("Memory unavailable");
            return buffer;
        }
        public void Dispose() { identity = null; playerState = 0; }
        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr OpenProcess(uint access, bool inherit, int id);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern bool ReadProcessMemory(IntPtr process, IntPtr address, byte[] data, int size, out IntPtr read);
        [DllImport("kernel32.dll")] private static extern bool CloseHandle(IntPtr handle);
    }
}
