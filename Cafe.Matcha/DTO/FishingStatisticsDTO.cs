namespace Cafe.Matcha.DTO
{
    using System;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.Network.Fishing;
    using Newtonsoft.Json;

    internal sealed class FishingStatisticsDTO : BaseDTO
    {
        public override EventType EventType => EventType.FishingStatistics;
        [JsonProperty("version")] public int Version = 1;
        [JsonProperty("enabled")] public bool Enabled;
        [JsonProperty("session")] public string Session;
        [JsonProperty("revision")] public long Revision;
        [JsonProperty("cast")] public FishingDTO Cast;
        [JsonProperty("state")] public string State = "disabled";
        [JsonProperty("cachedAt")] public long? CachedAt;
        [JsonProperty("uploadState")] public string UploadState = "idle";
        [JsonProperty("distributions")] public FishingDistribution[] Distributions = Array.Empty<FishingDistribution>();
    }
}
