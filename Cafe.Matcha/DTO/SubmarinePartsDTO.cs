// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal sealed class SubmarinePartConditionDTO
    {
        [JsonProperty("itemId")]
        public uint ItemId;

        // Inventory condition units: 30000 is 100%; zero means broken.
        [JsonProperty("condition")]
        public ushort Condition;
    }

    internal sealed class SubmarinePartsDTO : BaseDTO
    {
        public override EventType EventType => EventType.SubmarineParts;

        [JsonProperty("reset")]
        public bool Reset;

        [JsonProperty("index")]
        public int Index;

        // Hull, stern, bow, bridge. Null entries have not been observed.
        [JsonProperty("parts")]
        public SubmarinePartConditionDTO[] Parts;
    }
}
