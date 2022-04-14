// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    using System.Collections.Generic;

    internal enum MatchaOpcode
    {
        ActorControl,
        ActorControlSelf,
        CEDirector,
        CompanyAirshipStatus,
        CompanySubmersibleStatus,
        ContentFinderNotifyPop,
        DirectorStart,
        EventPlay,
        Examine,
        FateInfo,
        InitZone,
        InventoryTransaction,
        ItemInfo,
        MarketBoardItemListing,
        MarketBoardItemListingCount,
        MarketBoardItemListingHistory,
        NpcSpawn,
        PlayerSetup,
        PlayerSpawn,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x02e7, MatchaOpcode.ActorControl },
            { 0x028f, MatchaOpcode.ActorControlSelf },
            { 0x0336, MatchaOpcode.CEDirector },
            { 0x03b4, MatchaOpcode.CompanyAirshipStatus },
            { 0x0172, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01a3, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x00f3, MatchaOpcode.EventPlay },
            { 0xf008, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x01e7, MatchaOpcode.InitZone },
            { 0x00d7, MatchaOpcode.InventoryTransaction },
            { 0x0309, MatchaOpcode.ItemInfo },
            { 0x0069, MatchaOpcode.MarketBoardItemListing },
            { 0x0167, MatchaOpcode.MarketBoardItemListingCount },
            { 0x019e, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x012f, MatchaOpcode.NpcSpawn },
            { 0x01d2, MatchaOpcode.PlayerSetup },
            { 0x02bc, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0282, MatchaOpcode.ActorControl },
            { 0x0205, MatchaOpcode.ActorControlSelf },
            { 0x00ea, MatchaOpcode.CEDirector },
            { 0x02c9, MatchaOpcode.CompanyAirshipStatus },
            { 0x01cd, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02a0, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01c7, MatchaOpcode.DirectorStart },
            { 0x0310, MatchaOpcode.EventPlay },
            { 0x0299, MatchaOpcode.Examine },
            { 0x03ab, MatchaOpcode.FateInfo },
            { 0x0286, MatchaOpcode.InitZone },
            { 0x02fe, MatchaOpcode.InventoryTransaction },
            { 0x0115, MatchaOpcode.ItemInfo },
            { 0x0255, MatchaOpcode.MarketBoardItemListing },
            { 0x030f, MatchaOpcode.MarketBoardItemListingCount },
            { 0x006b, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0318, MatchaOpcode.NpcSpawn },
            { 0x03b2, MatchaOpcode.PlayerSetup },
            { 0x03b3, MatchaOpcode.PlayerSpawn },
        };
    }
}
