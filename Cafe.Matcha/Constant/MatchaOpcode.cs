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
            { 0x0114, MatchaOpcode.ActorControl },
            { 0x014e, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x0388, MatchaOpcode.CompanyAirshipStatus },
            { 0x01ad, MatchaOpcode.CompanySubmersibleStatus },
            { 0x037e, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0091, MatchaOpcode.EventPlay },
            { 0x0085, MatchaOpcode.EventStart },
            { 0x010a, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x009d, MatchaOpcode.InitZone },
            { 0x01c7, MatchaOpcode.InventoryTransaction },
            { 0x0328, MatchaOpcode.ItemInfo },
            { 0x0333, MatchaOpcode.MarketBoardItemListing },
            { 0x00f8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00a5, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0304, MatchaOpcode.NpcSpawn },
            { 0x00e5, MatchaOpcode.PlayerSetup },
            { 0x01a4, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0114, MatchaOpcode.ActorControl },
            { 0x014e, MatchaOpcode.ActorControlSelf },
            { 0x0277, MatchaOpcode.CEDirector },
            { 0x0388, MatchaOpcode.CompanyAirshipStatus },
            { 0x01ad, MatchaOpcode.CompanySubmersibleStatus },
            { 0x037e, MatchaOpcode.ContentFinderNotifyPop },
            { 0x034d, MatchaOpcode.ResumeEventScene32 },
            { 0x0091, MatchaOpcode.EventPlay },
            { 0x0085, MatchaOpcode.EventStart },
            { 0x008b, MatchaOpcode.Examine },
            { 0x010f, MatchaOpcode.FateInfo },
            { 0x009d, MatchaOpcode.InitZone },
            { 0x01c7, MatchaOpcode.InventoryTransaction },
            { 0x0328, MatchaOpcode.ItemInfo },
            { 0x0333, MatchaOpcode.MarketBoardItemListing },
            { 0x00f8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00a5, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x83dc, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0304, MatchaOpcode.NpcSpawn },
            { 0x00e5, MatchaOpcode.PlayerSetup },
            { 0x01a4, MatchaOpcode.PlayerSpawn },
            { 0x03dc, MatchaOpcode.WorldVisitQueue },
        };
    }
}
