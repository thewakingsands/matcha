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
        SubmarineStatusList,
        WorldVisitQueue,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x025f, MatchaOpcode.ActorControl },
            { 0x0204, MatchaOpcode.ActorControlSelf },
            { 0x031e, MatchaOpcode.CEDirector },
            { 0x006c, MatchaOpcode.CompanyAirshipStatus },
            { 0x01ec, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0333, MatchaOpcode.ContentFinderNotifyPop },
            { 0x024d, MatchaOpcode.ResumeEventScene32 },
            { 0x01fd, MatchaOpcode.EventPlay },
            { 0x02e1, MatchaOpcode.EventStart },
            { 0x01f2, MatchaOpcode.Examine },
            { 0x0154, MatchaOpcode.FateInfo },
            { 0x032b, MatchaOpcode.InitZone },
            { 0x023a, MatchaOpcode.InventoryTransaction },
            { 0x0084, MatchaOpcode.ItemInfo },
            { 0x034d, MatchaOpcode.MarketBoardItemListing },
            { 0x00c0, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0241, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8320, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x020c, MatchaOpcode.NpcSpawn },
            { 0x0093, MatchaOpcode.PlayerSetup },
            { 0x01c4, MatchaOpcode.PlayerSpawn },
            { 0x038a, MatchaOpcode.SubmarineStatusList },
            { 0x01e8, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x025f, MatchaOpcode.ActorControl },
            { 0x0204, MatchaOpcode.ActorControlSelf },
            { 0x031e, MatchaOpcode.CEDirector },
            { 0x006c, MatchaOpcode.CompanyAirshipStatus },
            { 0x01ec, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0333, MatchaOpcode.ContentFinderNotifyPop },
            { 0x024d, MatchaOpcode.ResumeEventScene32 },
            { 0x01fd, MatchaOpcode.EventPlay },
            { 0x02e1, MatchaOpcode.EventStart },
            { 0x01f2, MatchaOpcode.Examine },
            { 0x0154, MatchaOpcode.FateInfo },
            { 0x032b, MatchaOpcode.InitZone },
            { 0x023a, MatchaOpcode.InventoryTransaction },
            { 0x0084, MatchaOpcode.ItemInfo },
            { 0x034d, MatchaOpcode.MarketBoardItemListing },
            { 0x00c0, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0241, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8320, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x020c, MatchaOpcode.NpcSpawn },
            { 0x0093, MatchaOpcode.PlayerSetup },
            { 0x01c4, MatchaOpcode.PlayerSpawn },
            { 0x038a, MatchaOpcode.SubmarineStatusList },
            { 0x01e8, MatchaOpcode.WorldVisitQueue },
        };
    }
}
