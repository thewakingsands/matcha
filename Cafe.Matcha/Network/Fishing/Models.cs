namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.Linq;
    using Cafe.Matcha.DTO;
    using Newtonsoft.Json;

    internal sealed class FishingAttributes
    {
        public long SampledAt { get; }
        public int? Gathering { get; }
        public int? Perception { get; }
        public int? CurrentGp { get; }
        public int? MaxGp { get; }
        public uint? BaitId { get; }
        public FishingAttributes(long time, int? gathering = null, int? perception = null,
            int? gp = null, int? maxGp = null, uint? bait = null)
        { SampledAt = time; Gathering = gathering; Perception = perception; CurrentGp = gp; MaxGp = maxGp; BaitId = bait; }
    }

    internal interface IFishingMemoryReader : IDisposable
    {
        FishingAttributes Read();
    }

    // Local-only protocol state. Never serialize this object through the overlay event channel.
    internal sealed class FishingCapture
    {
        public long? ServerCastTime;
        public long? EndTime;
        public int? Hook;
        public int? Size;
        public int? Quantity;
        public bool? Large;
        public bool GatheringFailed;
        public bool Unsupported;
        public bool Terminal;
        public bool? PatiencePrecision;
        public bool? PatienceSuccess;
        public uint Slap;
        public uint Identical;
        public uint Hidden;
        public int HiddenStack;
        public int Lure;
        public bool LureTarget;
        public long[] LureTimes = Array.Empty<long>();
        public FishingCapture Copy() => new FishingCapture {
            ServerCastTime = ServerCastTime, EndTime = EndTime, Hook = Hook, Size = Size, Quantity = Quantity,
            Large = Large, GatheringFailed = GatheringFailed, Unsupported = Unsupported,
            Terminal = Terminal, PatiencePrecision = PatiencePrecision, PatienceSuccess = PatienceSuccess,
            Slap = Slap, Identical = Identical, Hidden = Hidden, HiddenStack = HiddenStack,
            Lure = Lure, LureTarget = LureTarget, LureTimes = (long[])LureTimes.Clone(),
        };
    }

    internal sealed class DurationRange
    {
        [JsonProperty("min", Required = Required.Always)] public double Min { get; set; }
        [JsonProperty("max", Required = Required.Always)] public double Max { get; set; }
        [JsonProperty("effective_min", Required = Required.Always)] public double EffectiveMin { get; set; }
        [JsonProperty("effective_max", Required = Required.Always)] public double EffectiveMax { get; set; }
    }
    internal sealed class DurationPercentiles
    {
        [JsonProperty("p50", Required = Required.Always)] public double Median { get; set; }
    }
    internal sealed class FishingDistribution
    {
        [JsonProperty("bait_id", Required = Required.Always)] public uint BaitId { get; set; }
        [JsonProperty("fish_id", Required = Required.Always)] public uint FishId { get; set; }
        [JsonProperty("is_chum", Required = Required.Always)] public bool Chum { get; set; }
        [JsonProperty("tug_type", Required = Required.Always)] public int Tug { get; set; }
        [JsonProperty("count", Required = Required.Always)] public int Count { get; set; }
        [JsonProperty("range", Required = Required.Always)] public DurationRange Range { get; set; }
        [JsonProperty("percentiles", Required = Required.Always)] public DurationPercentiles Percentiles { get; set; }
        [JsonIgnore] public bool Valid => BaitId > 0 && FishId > 0 && Count >= 10 && Tug >= 1 && Tug <= 3
            && Range != null && Percentiles != null && Range.Min > 0 && Range.Max <= 600000
            && Range.Min <= Range.EffectiveMin && Range.EffectiveMin <= Range.EffectiveMax
            && Range.EffectiveMax <= Range.Max && Percentiles.Median >= Range.Min && Percentiles.Median <= Range.Max;
    }
    internal sealed class DurationResponse
    {
        [JsonProperty("spot_id", Required = Required.Always)] public uint SpotId { get; set; }
        [JsonProperty("distributions", Required = Required.Always)] public FishingDistribution[] Distributions { get; set; }
    }
}
