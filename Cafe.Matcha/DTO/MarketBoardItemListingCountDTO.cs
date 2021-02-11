// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class MarketBoardItemListingCountDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.MarketBoardItemListingCount;
            }
        }

        [JsonProperty("item")]
        public int Item;

        [JsonProperty("world")]
        public int World = 0;

        [JsonProperty("count")]
        public int Count;
    }
}
