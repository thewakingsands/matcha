// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    // WARNING: The order CANNOT be changed. ONLY append to this list.
    public enum EventType
    {
        None = 0,
        Fate,

        MatchAlert,
        InitZone,

        FishBite,
        MarketBoardItemListing,
        MarketBoardItemListingCount,

        MiniCactpot,
        Gearset,
        TreasureSpot,
        TreasureResult,
        CompanyVoyageStatus,

        DynamicEvent,
        FateWatchListChanged,
        Queue,
        FishCast
    }
}
