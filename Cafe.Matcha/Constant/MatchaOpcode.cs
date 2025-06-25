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
            { 0x03e7, MatchaOpcode.ActorControl },
            { 0x0117, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x00c7, MatchaOpcode.CompanyAirshipStatus },
            { 0x02f9, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00a5, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x01e0, MatchaOpcode.EventPlay },
            { 0x0290, MatchaOpcode.EventStart },
            { 0x03ab, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x037e, MatchaOpcode.InitZone },
            { 0x0115, MatchaOpcode.InventoryTransaction },
            { 0x038e, MatchaOpcode.ItemInfo },
            { 0x0167, MatchaOpcode.MarketBoardItemListing },
            { 0x0223, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00a3, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x038c, MatchaOpcode.NpcSpawn },
            { 0x0309, MatchaOpcode.PlayerSetup },
            { 0x032a, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0344, MatchaOpcode.ActorControl },
            { 0x0393, MatchaOpcode.ActorControlSelf },
            { 0x02d4, MatchaOpcode.CEDirector },
            { 0x01d7, MatchaOpcode.CompanyAirshipStatus },
            { 0x03d7, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0181, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0133, MatchaOpcode.ResumeEventScene32 },
            { 0x031f, MatchaOpcode.EventPlay },
            { 0x01f9, MatchaOpcode.EventStart },
            { 0x02f9, MatchaOpcode.Examine },
            { 0x032e, MatchaOpcode.FateInfo },
            { 0x012a, MatchaOpcode.InitZone },
            { 0x009a, MatchaOpcode.InventoryTransaction },
            { 0x030f, MatchaOpcode.ItemInfo },
            { 0x0085, MatchaOpcode.MarketBoardItemListing },
            { 0x01c8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00e7, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8069, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x014f, MatchaOpcode.NpcSpawn },
            { 0x00cd, MatchaOpcode.PlayerSetup },
            { 0x00dc, MatchaOpcode.PlayerSpawn },
            { 0x025d, MatchaOpcode.WorldVisitQueue },
        };
    }
}
