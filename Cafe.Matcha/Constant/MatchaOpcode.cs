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
            { 0x009c, MatchaOpcode.ActorControl },
            { 0x02e2, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x0353, MatchaOpcode.CompanyAirshipStatus },
            { 0x02ea, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0159, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x010d, MatchaOpcode.EventPlay },
            { 0x0127, MatchaOpcode.EventStart },
            { 0x01f4, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x0316, MatchaOpcode.InitZone },
            { 0x0104, MatchaOpcode.InventoryTransaction },
            { 0x0194, MatchaOpcode.ItemInfo },
            { 0x018f, MatchaOpcode.MarketBoardItemListing },
            { 0x0136, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01e2, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x025e, MatchaOpcode.NpcSpawn },
            { 0x0137, MatchaOpcode.PlayerSetup },
            { 0x0374, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x018e, MatchaOpcode.ActorControl },
            { 0x02df, MatchaOpcode.ActorControlSelf },
            { 0x0176, MatchaOpcode.CEDirector },
            { 0x0240, MatchaOpcode.CompanyAirshipStatus },
            { 0x01c5, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0114, MatchaOpcode.ContentFinderNotifyPop },
            { 0x03e1, MatchaOpcode.ResumeEventScene32 },
            { 0x013e, MatchaOpcode.EventPlay },
            { 0x0268, MatchaOpcode.EventStart },
            { 0x0259, MatchaOpcode.Examine },
            { 0x02c1, MatchaOpcode.FateInfo },
            { 0x010f, MatchaOpcode.InitZone },
            { 0x01c3, MatchaOpcode.InventoryTransaction },
            { 0x00c4, MatchaOpcode.ItemInfo },
            { 0x0313, MatchaOpcode.MarketBoardItemListing },
            { 0x011b, MatchaOpcode.MarketBoardItemListingCount },
            { 0x016a, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x81ff, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x01aa, MatchaOpcode.NpcSpawn },
            { 0x033b, MatchaOpcode.PlayerSetup },
            { 0x02c6, MatchaOpcode.PlayerSpawn },
            { 0x0391, MatchaOpcode.WorldVisitQueue },
        };
    }
}
