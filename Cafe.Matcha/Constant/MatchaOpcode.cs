// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    internal enum MatchaOpcode : ushort
    {
#if GLOBAL
        ActorControlSelf = 0x0165,
        DirectorStart = 0x0399, // 
        ContentFinderNotifyPop = 0x145,
        EventPlay = 0x02C3,
        Examine = 0x038D, // 
        InitZone = 0x01CD,
        ItemInfo = 0x02AA,
        MarketBoardItemListingHistory = 0x0240,
        MarketBoardItemListing = 0x00D7,
        MarketBoardItemListingCount = 0x0244,
        PlayerSpawn = 0x00B9,
        PlayerSetup = 0x0071,
#else
        ActorControlSelf = 0x007C,
        CEDirector = 0x0144,
        CompanyAirshipStatus = 0x0271,
        CompanySubmersibleStatus = 0x0345,
        ContentFinderNotifyPop = 0x02D0,
        DirectorStart = 0x02f2,
        EventPlay = 0x01B9,
        Examine = 0x0316,
        InitZone = 0x02D2,
        InventoryTransaction = 0x00A8,
        ItemInfo = 0x031A,
        MarketBoardItemListing = 0x0158,
        MarketBoardItemListingCount = 0x0280,
        MarketBoardItemListingHistory = 0x01F3,
        PlayerSetup = 0x01f9,
        PlayerSpawn = 0x00D1,
#endif
    }
}
