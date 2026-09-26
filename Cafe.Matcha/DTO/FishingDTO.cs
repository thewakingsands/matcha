// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    // Every transition carries a complete cast snapshot. Null means not observed.
    internal class FishingDTO : BaseDTO
    {
        public override EventType EventType => EventType.Fishing;
        [JsonProperty("action")]
        public string Action;
        [JsonProperty("time")]
        public long Time;
        [JsonProperty("castId")]
        public string CastId;
        [JsonProperty("actorId")]
        public uint ActorId;
        [JsonProperty("castTime")]
        public long? CastTime;
        [JsonProperty("biteTime")]
        public long? BiteTime;
        [JsonProperty("hookTime")]
        public long? HookTime;
        [JsonProperty("baitId")]
        public uint? BaitId;
        [JsonProperty("baseBaitId")]
        public uint? BaseBaitId;
        [JsonProperty("placeId")]
        public uint? PlaceId;
        [JsonProperty("tug")]
        public int? Tug;
        [JsonProperty("fishId")]
        public uint? FishId;
        [JsonProperty("mooch")]
        public bool? Mooch;
        [JsonProperty("chum")]
        public bool? Chum;
        [JsonProperty("snagging")]
        public bool? Snagging;
        [JsonProperty("statuses")]
        public uint[] Statuses;
    }
}
