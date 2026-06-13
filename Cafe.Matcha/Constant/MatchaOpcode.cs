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
            { 0x027f, MatchaOpcode.ActorControl },
            { 0x0362, MatchaOpcode.ActorControlSelf },
            { 0x013b, MatchaOpcode.CEDirector },
            { 0x027e, MatchaOpcode.CompanyAirshipStatus },
            { 0x00cd, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00fd, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0065, MatchaOpcode.ResumeEventScene32 },
            { 0x029b, MatchaOpcode.EventPlay },
            { 0x0356, MatchaOpcode.EventStart },
            { 0x01bb, MatchaOpcode.Examine },
            { 0x0277, MatchaOpcode.FateInfo },
            { 0x008b, MatchaOpcode.InitZone },
            { 0x034f, MatchaOpcode.InventoryTransaction },
            { 0x0314, MatchaOpcode.ItemInfo },
            { 0x02e0, MatchaOpcode.MarketBoardItemListing },
            { 0x0184, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0230, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x820a, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0113, MatchaOpcode.NpcSpawn },
            { 0x03a8, MatchaOpcode.PlayerSetup },
            { 0x03b4, MatchaOpcode.PlayerSpawn },
            { 0x009e, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x027f, MatchaOpcode.ActorControl },
            { 0x0362, MatchaOpcode.ActorControlSelf },
            { 0x013b, MatchaOpcode.CEDirector },
            { 0x027e, MatchaOpcode.CompanyAirshipStatus },
            { 0x00cd, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00fd, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0065, MatchaOpcode.ResumeEventScene32 },
            { 0x029b, MatchaOpcode.EventPlay },
            { 0x0356, MatchaOpcode.EventStart },
            { 0x01bb, MatchaOpcode.Examine },
            { 0x0277, MatchaOpcode.FateInfo },
            { 0x008b, MatchaOpcode.InitZone },
            { 0x034f, MatchaOpcode.InventoryTransaction },
            { 0x0314, MatchaOpcode.ItemInfo },
            { 0x02e0, MatchaOpcode.MarketBoardItemListing },
            { 0x0184, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0230, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x820a, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0113, MatchaOpcode.NpcSpawn },
            { 0x03a8, MatchaOpcode.PlayerSetup },
            { 0x03b4, MatchaOpcode.PlayerSpawn },
            { 0x009e, MatchaOpcode.WorldVisitQueue },
        };
    }
}
