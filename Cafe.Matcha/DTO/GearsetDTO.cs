// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using System.Collections.Generic;
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class Materia
    {
        [JsonProperty("type")]
        public int Type;

        [JsonProperty("tier")]
        public int Tier;
    }

    internal class GearsetDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.Gearset;
            }
        }

        [JsonProperty("self")]
        public bool IsSelf;

        [JsonProperty("slot")]
        public int Slot;

        [JsonProperty("item")]
        public int Item;

        [JsonProperty("hq")]
        public bool HQ;

        [JsonProperty("glamour")]
        public int Glamour;

        [JsonProperty("materias")]
        public IEnumerable<Materia> Materias;
    }
}
