namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    internal sealed class FishingMemoryReader : IFishingMemoryReader
    {
        private readonly Func<Process> process;
        private readonly Func<int?[]> gp;
        private readonly Func<long> now;
        private string identity;
        private long playerState;
        private bool disposed;
        private const int StateReadExtent = 0x4e8;
        internal const uint ReadAccess = 0x1010; // VM_READ | QUERY_LIMITED_INFORMATION; never write or VM_OPERATION.
        public FishingMemoryReader(Func<Process> process, Func<int?[]> gp, Func<long> now)
        { this.process = process; this.gp = gp; this.now = now; }

        public FishingAttributes Read()
        {
            var time = now();
            if (disposed) return new FishingAttributes(time);
            try
            {
                var target = process();
                if (target == null || target.HasExited) { identity = null; playerState = 0; return new FishingAttributes(time); }
                var key = target.Id + ":" + target.StartTime.ToUniversalTime().Ticks + ":" + target.MainModule.FileName;
                if (identity != key)
                {
                    identity = key; playerState = 0;
                    // Bound allocation even if the executable is replaced or malformed.
                    byte[] image;
                    using (var file = File.OpenRead(target.MainModule.FileName))
                    {
                        if (file.Length < 64 || file.Length > 512L * 1024 * 1024) throw new InvalidDataException();
                        image = new byte[(int)file.Length];
                        int offset = 0;
                        while (offset < image.Length) { var count = file.Read(image, offset, image.Length - offset); if (count == 0) throw new IOException(); offset += count; }
                    }
                    var rva = Locate(image);
                    if (rva.HasValue && (long)rva.Value + StateReadExtent <= target.MainModule.ModuleMemorySize)
                        playerState = checked(target.MainModule.BaseAddress.ToInt64() + rva.Value);
                }
                var points = gp();
                if (playerState == 0) return new FishingAttributes(time, gp: points?.Length >= 2 ? points[0] : null, maxGp: points?.Length >= 2 ? points[1] : null);
                using (var handle = OpenProcess(ReadAccess, false, target.Id))
                {
                    if (handle.IsInvalid) { return new FishingAttributes(time); }
                    // Bind this handle to the selected process instance, including PID reuse races.
                    if (!GetProcessTimes(handle, out var created, out var exited, out var kernel, out var user)
                        || created != target.StartTime.ToUniversalTime().ToFileTimeUtc())
                    { return new FishingAttributes(time); }
                    var result = Sample(time, offset => Read(handle, checked(playerState + offset.Item1), offset.Item2), points);
                    if (target.HasExited) { return new FishingAttributes(time); }
                    return result;
                }
            }
            // A transient read failure must not rescan the executable on the next sample.
            catch { return new FishingAttributes(time); }
        }
        internal static FishingAttributes Sample(long time, Func<Tuple<int, int>, byte[]> read, int?[] points)
        {
            byte[] Bytes(int offset, int size)
            {
                var data = read(Tuple.Create(offset, size));
                if (data == null || data.Length != size) throw new IOException();
                return data;
            }
            var loaded = Bytes(0, 1)[0];
            var actor = BitConverter.ToUInt32(Bytes(0x64, 4), 0);
            if (loaded != 1 || actor == 0 || Bytes(0x7e, 1)[0] != 18) return new FishingAttributes(time);
            if (points?.Length >= 3 && points[2].HasValue && unchecked((uint)points[2].Value) != actor) return new FishingAttributes(time);
            var stats = Bytes(0x1a8 + 72 * 4, 8);
            var bait = BitConverter.ToUInt32(Bytes(0x4e4, 4), 0);
            if (Bytes(0, 1)[0] != loaded || Bytes(0x7e, 1)[0] != 18 || BitConverter.ToUInt32(Bytes(0x64, 4), 0) != actor)
                return new FishingAttributes(time);
            var currentGp = points?.Length >= 2 ? points[0] : null;
            var maxGp = points?.Length >= 2 ? points[1] : null;
            if (currentGp < 0 || maxGp < 0 || currentGp > maxGp) currentGp = maxGp = null;
            return new FishingAttributes(time, Positive(BitConverter.ToInt32(stats, 0)), Positive(BitConverter.ToInt32(stats, 4)),
                currentGp, maxGp, bait == 0 ? (uint?)null : bait);
        }
        private static int? Positive(int value) => value > 0 && value < 100000 ? (int?)value : null;

        // FFXIVClientStructs 25762ad04: PlayerState.Instance/Attributes/FishingBait.
        // Resolve a unique static-address signature without restricting the executable hash.
        internal static uint? Locate(byte[] image)
        {
            int?[] signature = { 0x48, 0x8d, 0x0d, null, null, null, null, 0xe8, null, null, null, null,
                0x84, 0xc0, 0x75, 0x06, 0xf6, 0x43, 0x18, 0x02 };
            if (image == null || image.Length < 64 || BitConverter.ToUInt16(image, 0) != 0x5a4d) return null;
            var pe = BitConverter.ToInt32(image, 0x3c);
            if (pe < 0 || pe > image.Length - 24 || BitConverter.ToUInt32(image, pe) != 0x4550) return null;
            if (BitConverter.ToUInt16(image, pe + 4) != 0x8664) return null;
            int optionalSize = BitConverter.ToUInt16(image, pe + 20);
            if (optionalSize < 64 || (long)pe + 24 + optionalSize > image.Length || BitConverter.ToUInt16(image, pe + 24) != 0x20b) return null;
            uint imageSize = BitConverter.ToUInt32(image, pe + 24 + 56);
            int sections = BitConverter.ToUInt16(image, pe + 6);
            int table = pe + 24 + BitConverter.ToUInt16(image, pe + 20);
            if (table < 0 || (long)table + sections * 40 > image.Length) return null;
            uint? found = null;
            for (int section = 0; section < sections; section++)
            {
                int entry = table + section * 40;
                if ((BitConverter.ToUInt32(image, entry + 36) & 0x20000000) == 0) continue;
                uint va = BitConverter.ToUInt32(image, entry + 12), size = BitConverter.ToUInt32(image, entry + 16), raw = BitConverter.ToUInt32(image, entry + 20);
                if ((long)raw + size > image.Length || (long)va + size > imageSize) return null;
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
            if (!found.HasValue || (long)found.Value + StateReadExtent > imageSize) return null;
            for (int section = 0; section < sections; section++)
            {
                int entry = table + section * 40;
                uint va = BitConverter.ToUInt32(image, entry + 12), size = BitConverter.ToUInt32(image, entry + 8);
                uint flags = BitConverter.ToUInt32(image, entry + 36);
                // PlayerState must fit wholly in a readable, non-executable data section.
                if ((flags & 0x40000000) != 0 && (flags & 0x20000000) == 0
                    && found.Value >= va && (long)found.Value + StateReadExtent <= (long)va + size) return found;
            }
            return null;
        }
        private static byte[] Read(SafeProcessHandle handle, long address, int size)
        {
            var buffer = new byte[size];
            if (!ReadProcessMemory(handle, new IntPtr(address), buffer, size, out var read) || read.ToInt64() != size) throw new IOException("Memory unavailable");
            return buffer;
        }
        public void Dispose() { disposed = true; identity = null; playerState = 0; }
        [DllImport("kernel32.dll", SetLastError = true)] private static extern SafeProcessHandle OpenProcess(uint access, bool inherit, int id);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern bool ReadProcessMemory(SafeProcessHandle process, IntPtr address, byte[] data, int size, out IntPtr read);
        [DllImport("kernel32.dll", SetLastError = true)] private static extern bool GetProcessTimes(SafeProcessHandle process, out long created, out long exited, out long kernel, out long user);
    }
}
