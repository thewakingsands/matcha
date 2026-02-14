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
            { 0x01ec, MatchaOpcode.ActorControl },
            { 0x0162, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x0345, MatchaOpcode.CompanyAirshipStatus },
            { 0x02e4, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0215, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0226, MatchaOpcode.EventPlay },
            { 0x0391, MatchaOpcode.EventStart },
            { 0x018d, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x02c0, MatchaOpcode.InitZone },
            { 0x0240, MatchaOpcode.InventoryTransaction },
            { 0x0211, MatchaOpcode.ItemInfo },
            { 0x016e, MatchaOpcode.MarketBoardItemListing },
            { 0x0390, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0073, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0197, MatchaOpcode.NpcSpawn },
            { 0x0141, MatchaOpcode.PlayerSetup },
            { 0x0221, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x01ec, MatchaOpcode.ActorControl },
            { 0x0162, MatchaOpcode.ActorControlSelf },
            { 0x0256, MatchaOpcode.CEDirector },
            { 0x0345, MatchaOpcode.CompanyAirshipStatus },
            { 0x02e4, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0215, MatchaOpcode.ContentFinderNotifyPop },
            { 0x021a, MatchaOpcode.ResumeEventScene32 },
            { 0x0226, MatchaOpcode.EventPlay },
            { 0x0391, MatchaOpcode.EventStart },
            { 0x0134, MatchaOpcode.Examine },
            { 0x0176, MatchaOpcode.FateInfo },
            { 0x02c0, MatchaOpcode.InitZone },
            { 0x0240, MatchaOpcode.InventoryTransaction },
            { 0x0211, MatchaOpcode.ItemInfo },
            { 0x016e, MatchaOpcode.MarketBoardItemListing },
            { 0x0390, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0073, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8341, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0197, MatchaOpcode.NpcSpawn },
            { 0x0141, MatchaOpcode.PlayerSetup },
            { 0x0221, MatchaOpcode.PlayerSpawn },
            { 0x0341, MatchaOpcode.WorldVisitQueue },
        };
    }
}
