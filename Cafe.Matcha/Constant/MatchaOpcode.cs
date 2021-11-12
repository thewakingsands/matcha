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
            { 0x02b6, MatchaOpcode.ActorControlSelf },
            { 0x0104, MatchaOpcode.CEDirector },
            { 0x0166, MatchaOpcode.CompanyAirshipStatus },
            { 0x0247, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0327, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01dd, MatchaOpcode.DirectorStart },
            { 0x016b, MatchaOpcode.EventPlay },
            { 0x0365, MatchaOpcode.Examine },
            { 0x0320, MatchaOpcode.InitZone },
            { 0x027f, MatchaOpcode.InventoryTransaction },
            { 0x01cc, MatchaOpcode.ItemInfo },
            { 0x0076, MatchaOpcode.MarketBoardItemListing },
            { 0x0068, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01ba, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01d5, MatchaOpcode.PlayerSetup },
            { 0x01d8, MatchaOpcode.PlayerSpawn },
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
