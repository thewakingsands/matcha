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
            { 0x00d8, MatchaOpcode.ActorControl },
            { 0x038a, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x008f, MatchaOpcode.CompanyAirshipStatus },
            { 0x0228, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0214, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0250, MatchaOpcode.EventPlay },
            { 0x0076, MatchaOpcode.EventStart },
            { 0x03c9, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x012e, MatchaOpcode.InitZone },
            { 0x035f, MatchaOpcode.InventoryTransaction },
            { 0x022c, MatchaOpcode.ItemInfo },
            { 0x0147, MatchaOpcode.MarketBoardItemListing },
            { 0x0368, MatchaOpcode.MarketBoardItemListingCount },
            { 0x026b, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x011e, MatchaOpcode.NpcSpawn },
            { 0x036f, MatchaOpcode.PlayerSetup },
            { 0x018b, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x00d8, MatchaOpcode.ActorControl },
            { 0x038a, MatchaOpcode.ActorControlSelf },
            { 0x0229, MatchaOpcode.CEDirector },
            { 0x008f, MatchaOpcode.CompanyAirshipStatus },
            { 0x0228, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0214, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0189, MatchaOpcode.ResumeEventScene32 },
            { 0x0250, MatchaOpcode.EventPlay },
            { 0x0076, MatchaOpcode.EventStart },
            { 0x00ce, MatchaOpcode.Examine },
            { 0x00eb, MatchaOpcode.FateInfo },
            { 0x012e, MatchaOpcode.InitZone },
            { 0x035f, MatchaOpcode.InventoryTransaction },
            { 0x022c, MatchaOpcode.ItemInfo },
            { 0x0368, MatchaOpcode.MarketBoardItemListing },
            { 0x03bc, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0142, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x81ee, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x011e, MatchaOpcode.NpcSpawn },
            { 0x036f, MatchaOpcode.PlayerSetup },
            { 0x018b, MatchaOpcode.PlayerSpawn },
            { 0x01ee, MatchaOpcode.WorldVisitQueue },
        };
    }
}
