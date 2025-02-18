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
            { 0x01e2, MatchaOpcode.ActorControl },
            { 0x0393, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x01ec, MatchaOpcode.CompanyAirshipStatus },
            { 0x026c, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02fd, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0159, MatchaOpcode.EventPlay },
            { 0x00c1, MatchaOpcode.EventStart },
            { 0x03e1, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x02f0, MatchaOpcode.InitZone },
            { 0x018a, MatchaOpcode.InventoryTransaction },
            { 0x0119, MatchaOpcode.ItemInfo },
            { 0x01de, MatchaOpcode.MarketBoardItemListing },
            { 0x027b, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00bf, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x039a, MatchaOpcode.NpcSpawn },
            { 0x036a, MatchaOpcode.PlayerSetup },
            { 0x0325, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x02bf, MatchaOpcode.ActorControl },
            { 0x0119, MatchaOpcode.ActorControlSelf },
            { 0x036a, MatchaOpcode.CEDirector },
            { 0x0291, MatchaOpcode.CompanyAirshipStatus },
            { 0x01b4, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01ad, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01fe, MatchaOpcode.ResumeEventScene32 },
            { 0x008a, MatchaOpcode.EventPlay },
            { 0x01a7, MatchaOpcode.EventStart },
            { 0x02bd, MatchaOpcode.Examine },
            { 0x0242, MatchaOpcode.FateInfo },
            { 0x0108, MatchaOpcode.InitZone },
            { 0x0162, MatchaOpcode.InventoryTransaction },
            { 0x03a9, MatchaOpcode.ItemInfo },
            { 0x0207, MatchaOpcode.MarketBoardItemListing },
            { 0x026e, MatchaOpcode.MarketBoardItemListingCount },
            { 0x010e, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x81bf, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x03bb, MatchaOpcode.NpcSpawn },
            { 0x0215, MatchaOpcode.PlayerSetup },
            { 0x01dd, MatchaOpcode.PlayerSpawn },
            { 0x0124, MatchaOpcode.WorldVisitQueue },
        };
    }
}
