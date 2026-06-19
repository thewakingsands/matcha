// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class SubmarineStatusDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.SubmarineStatus;
            }
        }

        [JsonProperty("index")]
        public int Index;

        [JsonProperty("status")]
        public ushort Status;

        [JsonProperty("rank")]
        public ushort Rank;

        [JsonProperty("birthdate")]
        public uint Birthdate;

        [JsonProperty("returnTime")]
        public uint ReturnTime;

        [JsonProperty("currentExp")]
        public uint CurrentExp;

        [JsonProperty("totalExpForNextRank")]
        public uint TotalExpForNextRank;

        [JsonProperty("capacity")]
        public ushort Capacity;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("hull")]
        public ushort Hull;

        [JsonProperty("stern")]
        public ushort Stern;

        [JsonProperty("bow")]
        public ushort Bow;

        [JsonProperty("bridge")]
        public ushort Bridge;

        [JsonProperty("destination")]
        public uint[] Destination;
    }
}
