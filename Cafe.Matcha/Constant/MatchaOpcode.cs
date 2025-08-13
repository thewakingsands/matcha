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
            { 0x02e0, MatchaOpcode.ActorControl },
            { 0x01f4, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x037f, MatchaOpcode.CompanyAirshipStatus },
            { 0x01b0, MatchaOpcode.CompanySubmersibleStatus },
            { 0x019f, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x00a3, MatchaOpcode.EventPlay },
            { 0x0253, MatchaOpcode.EventStart },
            { 0x031b, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x036c, MatchaOpcode.InitZone },
            { 0x0192, MatchaOpcode.InventoryTransaction },
            { 0x02a4, MatchaOpcode.ItemInfo },
            { 0x0103, MatchaOpcode.MarketBoardItemListing },
            { 0x03cd, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0077, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x00c4, MatchaOpcode.NpcSpawn },
            { 0x0359, MatchaOpcode.PlayerSetup },
            { 0x037a, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0099, MatchaOpcode.ActorControl },
            { 0x025e, MatchaOpcode.ActorControlSelf },
            { 0x014e, MatchaOpcode.CEDirector },
            { 0x019a, MatchaOpcode.CompanyAirshipStatus },
            { 0x02dd, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0246, MatchaOpcode.ContentFinderNotifyPop },
            { 0x020c, MatchaOpcode.ResumeEventScene32 },
            { 0x0380, MatchaOpcode.EventPlay },
            { 0x02b9, MatchaOpcode.EventStart },
            { 0x0097, MatchaOpcode.Examine },
            { 0x02a3, MatchaOpcode.FateInfo },
            { 0x00ce, MatchaOpcode.InitZone },
            { 0x01d6, MatchaOpcode.InventoryTransaction },
            { 0x032d, MatchaOpcode.ItemInfo },
            { 0x020e, MatchaOpcode.MarketBoardItemListing },
            { 0x01be, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03ad, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x80ef, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x01f6, MatchaOpcode.NpcSpawn },
            { 0x00d8, MatchaOpcode.PlayerSetup },
            { 0x0222, MatchaOpcode.PlayerSpawn },
            { 0x0334, MatchaOpcode.WorldVisitQueue },
        };
    }
}
