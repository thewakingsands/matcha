// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using System.Collections.Generic;
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class MarketBoardItemListingItem
    {
        [JsonProperty("price")]
        public int Price;

        [JsonProperty("quantity")]
        public int Quantity;

        [JsonProperty("hq")]
        public bool HQ;
    }

    internal class MarketBoardItemListingDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.MarketBoardItemListing;
            }
        }

        [JsonProperty("item")]
        public int Item;

        [JsonProperty("world")]
        public int World = 0;

        [JsonProperty("data")]
        public IEnumerable<MarketBoardItemListingItem> Data;
    }
}
