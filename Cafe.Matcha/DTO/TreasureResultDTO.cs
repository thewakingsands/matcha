// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class TreasureResultDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.TreasureResult;
            }
        }

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("round")]
        public int Round;
    }
}
