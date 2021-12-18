// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    using System.Collections.Generic;

    internal enum MatchaOpcode
    {
        ActorControlSelf,
        CEDirector,
        CompanyAirshipStatus,
        CompanySubmersibleStatus,
        ContentFinderNotifyPop,
        DirectorStart,
        EventPlay,
        Examine,
        InitZone,
        InventoryTransaction,
        ItemInfo,
        MarketBoardItemListing,
        MarketBoardItemListingCount,
        MarketBoardItemListingHistory,
        PlayerSetup,
        PlayerSpawn,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x02e6, MatchaOpcode.ActorControlSelf },
            { 0x00bb, MatchaOpcode.CEDirector },
            { 0x00ed, MatchaOpcode.CompanyAirshipStatus },
            { 0x00f5, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0317, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x00a5, MatchaOpcode.EventPlay },
            { 0x011b, MatchaOpcode.Examine },
            { 0x02c4, MatchaOpcode.InitZone },
            { 0x008f, MatchaOpcode.InventoryTransaction },
            { 0x0173, MatchaOpcode.ItemInfo },
            { 0x0323, MatchaOpcode.MarketBoardItemListing },
            { 0x023c, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0192, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x008b, MatchaOpcode.PlayerSetup },
            { 0x0133, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0245, MatchaOpcode.ActorControlSelf },
            { 0x016f, MatchaOpcode.CEDirector },
            { 0x039e, MatchaOpcode.CompanyAirshipStatus },
            { 0x014e, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0225, MatchaOpcode.ContentFinderNotifyPop },
            { 0x03b0, MatchaOpcode.DirectorStart },
            { 0x0276, MatchaOpcode.EventPlay },
            { 0x03cc, MatchaOpcode.Examine },
            { 0x01d6, MatchaOpcode.InitZone },
            { 0x02e4, MatchaOpcode.InventoryTransaction },
            { 0x036a, MatchaOpcode.ItemInfo },
            { 0x0325, MatchaOpcode.MarketBoardItemListing },
            { 0x022d, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02c8, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0194, MatchaOpcode.PlayerSetup },
            { 0x00f9, MatchaOpcode.PlayerSpawn },
        };
    }
}
