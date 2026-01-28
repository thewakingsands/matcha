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
        ResumeEventScene32,
        EventPlay,
        EventStart,
        Examine,
        FateInfo,
        InitZone,
        InventoryTransaction,
        ItemInfo,
        MarketBoardItemListing,
        MarketBoardItemListingCount,
        MarketBoardItemListingHistory,
        MarketBoardRequestItemListingInfo,
        NpcSpawn,
        PlayerSetup,
        PlayerSpawn,
        WorldVisitQueue,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x02c8, MatchaOpcode.ActorControl },
            { 0x02b3, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x03c6, MatchaOpcode.CompanyAirshipStatus },
            { 0x0267, MatchaOpcode.CompanySubmersibleStatus },
            { 0x024f, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0352, MatchaOpcode.EventPlay },
            { 0x02f8, MatchaOpcode.EventStart },
            { 0x00a6, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x01a4, MatchaOpcode.InitZone },
            { 0x01a9, MatchaOpcode.InventoryTransaction },
            { 0x0094, MatchaOpcode.ItemInfo },
            { 0x0069, MatchaOpcode.MarketBoardItemListing },
            { 0x0210, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0377, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x02e8, MatchaOpcode.NpcSpawn },
            { 0x00d7, MatchaOpcode.PlayerSetup },
            { 0x013e, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x02c8, MatchaOpcode.ActorControl },
            { 0x02b3, MatchaOpcode.ActorControlSelf },
            { 0x00f5, MatchaOpcode.CEDirector },
            { 0x03c6, MatchaOpcode.CompanyAirshipStatus },
            { 0x0267, MatchaOpcode.CompanySubmersibleStatus },
            { 0x024f, MatchaOpcode.ContentFinderNotifyPop },
            { 0x03ce, MatchaOpcode.ResumeEventScene32 },
            { 0x0352, MatchaOpcode.EventPlay },
            { 0x02f8, MatchaOpcode.EventStart },
            { 0x0235, MatchaOpcode.Examine },
            { 0x020e, MatchaOpcode.FateInfo },
            { 0x01a4, MatchaOpcode.InitZone },
            { 0x01a9, MatchaOpcode.InventoryTransaction },
            { 0x0094, MatchaOpcode.ItemInfo },
            { 0x0069, MatchaOpcode.MarketBoardItemListing },
            { 0x0210, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0377, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8389, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x02e8, MatchaOpcode.NpcSpawn },
            { 0x00d7, MatchaOpcode.PlayerSetup },
            { 0x013e, MatchaOpcode.PlayerSpawn },
            { 0x0389, MatchaOpcode.WorldVisitQueue },
        };
    }
}
