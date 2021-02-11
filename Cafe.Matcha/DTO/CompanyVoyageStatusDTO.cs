// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using System.Collections.Generic;
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class CompanyVoyageStatusItem
    {
        [JsonProperty("returnTime")]
        public uint ReturnTime;

        [JsonProperty("maxDistance")]
        public int MaxDistance;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("destination")]
        public int[] Destination;
    }

    internal class CompanyVoyageStatusDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.CompanyVoyageStatus;
            }
        }

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("list")]
        public IEnumerable<CompanyVoyageStatusItem> List;
    }
}
