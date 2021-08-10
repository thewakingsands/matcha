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
            { 0x0314, MatchaOpcode.ActorControlSelf },
            { 0x010d, MatchaOpcode.CEDirector },
            { 0x0250, MatchaOpcode.CompanyAirshipStatus },
            { 0x0292, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01ac, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf005, MatchaOpcode.DirectorStart },
            { 0x01ef, MatchaOpcode.EventPlay },
            { 0x00d3, MatchaOpcode.Examine },
            { 0x0100, MatchaOpcode.InitZone },
            { 0x03ac, MatchaOpcode.InventoryTransaction },
            { 0x00a7, MatchaOpcode.ItemInfo },
            { 0x00f5, MatchaOpcode.MarketBoardItemListing },
            { 0x0275, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0112, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0296, MatchaOpcode.PlayerSetup },
            { 0x0249, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0190, MatchaOpcode.ActorControlSelf },
            { 0x0308, MatchaOpcode.CEDirector },
            { 0x0171, MatchaOpcode.CompanyAirshipStatus },
            { 0x02ba, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00ee, MatchaOpcode.ContentFinderNotifyPop },
            { 0x02da, MatchaOpcode.DirectorStart },
            { 0x014d, MatchaOpcode.EventPlay },
            { 0x0334, MatchaOpcode.Examine },
            { 0x036e, MatchaOpcode.InitZone },
            { 0x0376, MatchaOpcode.InventoryTransaction },
            { 0x0322, MatchaOpcode.ItemInfo },
            { 0x0315, MatchaOpcode.MarketBoardItemListing },
            { 0x00d8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03e0, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01bf, MatchaOpcode.PlayerSetup },
            { 0x0323, MatchaOpcode.PlayerSpawn },
        };
    }
}
