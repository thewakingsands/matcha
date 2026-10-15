namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Cafe.Matcha.DTO;

    // Wire contract: ffxiv-cyou/ffxiv-fishing-float, 22e72eeb0c34c1025d95129c9b51c074e821fe85,
    // src/model/FishingSession.ts serialize(). Protocol encoding implemented independently.
    internal static class FishingUpload
    {
        public static object[] Create(FishingDTO cast, FishingAttributes attributes)
        {
            var c = cast.Capture;
            if (c == null) return null;
            // No tug flag plus a null result encodes a right-censored, early-ended cast.
            // Keep BiteTime absent: this is an observed end, not an inferred bite.
            var early = !cast.BiteTime.HasValue && c.EndTime.HasValue && !cast.FishId.HasValue
                && !cast.Tug.HasValue && !c.GatheringFailed;
            var end = early ? c.EndTime : cast.BiteTime;
            if (!c.Terminal || c.Unsupported || !c.ServerCastTime.HasValue || !end.HasValue
                || end <= c.ServerCastTime || (!early && end - c.ServerCastTime < 1000)
                || end - c.ServerCastTime > 600000 || !cast.PlaceId.HasValue || !cast.BaitId.HasValue
                || !cast.Chum.HasValue || !cast.Mooch.HasValue || cast.Statuses == null
                || (!early && (!cast.Tug.HasValue || cast.Tug < 1 || cast.Tug > 3 || !c.Hook.HasValue))
                || (c.Hook.HasValue && (c.Hook < 1 || c.Hook > 6))
                || attributes?.Gathering == null || attributes.Perception == null || attributes.CurrentGp == null
                || c.LureTimes.Any(t => t < c.ServerCastTime || t > end)) return null;
            var success = cast.FishId.HasValue;
            if (success && (!c.Size.HasValue || !c.Quantity.HasValue || c.Quantity <= 0 || !c.Large.HasValue)) return null;
            if (cast.Statuses.Contains(1803u) && c.Slap == 0 || cast.Statuses.Contains(1804u) && c.Identical == 0) return null;
            if (!early && cast.Statuses.Contains(764u) && c.PatienceSuccess == null) return null;
            uint flags = 0;
            var statuses = new uint[] { 763, 762, 568, 3907, 2780 };
            for (var i = 0; i < statuses.Length; i++) if (cast.Statuses.Contains(statuses[i])) flags |= 1u << i;
            if (cast.Statuses.Contains(764u)) flags |= cast.Statuses.Contains(765u) ? 1u << 6 : 1u << 5;
            if (c.Identical != 0) flags |= 1u << 7;
            if (c.Slap != 0) flags |= 1u << 8;
            flags |= (uint)(cast.Tug ?? 0) << 9 | (uint)(c.Hook ?? 0) << 11;
            if (c.GatheringFailed) flags |= 2u << 14;
            else if (success) flags |= 1u << 14;
            flags |= (uint)c.Lure << 16;
            if (c.LureTarget) flags |= 1u << 19;
            if (c.Hidden != 0) flags |= 1u << 20;
            if (c.PatienceSuccess.HasValue)
            {
                flags |= 1u << 21;
                if (c.PatiencePrecision == true) flags |= 1u << 22;
                if (c.PatienceSuccess == true) flags |= 1u << 23;
            }
            flags |= (uint)c.HiddenStack << 24;
            return new object[] { c.ServerCastTime.Value, cast.PlaceId.Value, cast.BaitId.Value,
                end.Value - c.ServerCastTime.Value, flags,
                success ? new object[] { cast.FishId.Value, c.Size.Value, c.Large.Value, c.Quantity.Value } : null,
                c.Slap, c.Identical, c.Hidden,
                c.LureTimes.Length == 0 ? 0L : c.LureTimes.Last() - c.ServerCastTime.Value,
                c.LureTimes.Select(t => (object)(t - c.ServerCastTime.Value)).ToArray(),
                new object[] { attributes.Gathering.Value, attributes.Perception.Value, attributes.CurrentGp.Value } };
        }
        // This contract only uses arrays, unsigned integers, booleans and null.
        public static byte[] Encode(IEnumerable<object[]> records)
        {
            using (var stream = new MemoryStream()) { Write(stream, records.Cast<object>().ToArray()); return stream.ToArray(); }
        }
        private static void Write(Stream stream, object value)
        {
            if (value == null) { stream.WriteByte(0xf6); return; }
            if (value is bool boolean) { stream.WriteByte(boolean ? (byte)0xf5 : (byte)0xf4); return; }
            if (value is object[] array)
            {
                Number(stream, 4, (ulong)array.Length);
                foreach (var item in array) Write(stream, item);
                return;
            }
            Number(stream, 0, Convert.ToUInt64(value));
        }
        private static void Number(Stream stream, int major, ulong value)
        {
            if (value < 24) { stream.WriteByte((byte)(major * 32 + (int)value)); return; }
            int count = value <= byte.MaxValue ? 1 : value <= ushort.MaxValue ? 2 : value <= uint.MaxValue ? 4 : 8;
            stream.WriteByte((byte)(major * 32 + (count == 1 ? 24 : count == 2 ? 25 : count == 4 ? 26 : 27)));
            for (int i = count - 1; i >= 0; i--) stream.WriteByte((byte)(value >> (8 * i)));
        }
    }
}
