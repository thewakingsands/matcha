// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    internal enum MatchaOpcode : ushort
    {
#if GLOBAL
        ActorControlSelf = 0x03d5,
        CEDirector = 0x01f5,
        CompanyAirshipStatus = 0x0206,
        CompanySubmersibleStatus = 0x013b,
        ContentFinderNotifyPop = 0x026e,
        DirectorStart = 0xf005,
        EventPlay = 0x0276,
        Examine = 0x0261,
        InitZone = 0x0233,
        InventoryTransaction = 0x02ee,
        ItemInfo = 0x0175,
        MarketBoardItemListing = 0x016b,
        MarketBoardItemListingCount = 0x00c0,
        MarketBoardItemListingHistory = 0x01c3,
        PlayerSetup = 0x01e9,
        PlayerSpawn = 0x01ab,
#else
        ActorControlSelf = 0x007c,
        CEDirector = 0x0144,
        CompanyAirshipStatus = 0x0271,
        CompanySubmersibleStatus = 0x0345,
        ContentFinderNotifyPop = 0x02d0,
        DirectorStart = 0x02f2,
        EventPlay = 0x01b9,
        Examine = 0x0316,
        InitZone = 0x02d2,
        InventoryTransaction = 0x00a8,
        ItemInfo = 0x031a,
        MarketBoardItemListing = 0x0158,
        MarketBoardItemListingCount = 0x0280,
        MarketBoardItemListingHistory = 0x01f3,
        PlayerSetup = 0x01f9,
        PlayerSpawn = 0x00d1,
#endif
    }
}
