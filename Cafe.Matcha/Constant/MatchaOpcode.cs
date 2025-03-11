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
            { 0x038e, MatchaOpcode.ActorControl },
            { 0x018c, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x030e, MatchaOpcode.CompanyAirshipStatus },
            { 0x0114, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0312, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0085, MatchaOpcode.EventPlay },
            { 0x01b4, MatchaOpcode.EventStart },
            { 0x02f4, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x0311, MatchaOpcode.InitZone },
            { 0x0327, MatchaOpcode.InventoryTransaction },
            { 0x03aa, MatchaOpcode.ItemInfo },
            { 0x00b6, MatchaOpcode.MarketBoardItemListing },
            { 0x03cb, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0302, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0100, MatchaOpcode.NpcSpawn },
            { 0x006b, MatchaOpcode.PlayerSetup },
            { 0x01ab, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x023b, MatchaOpcode.ActorControl },
            { 0x00f3, MatchaOpcode.ActorControlSelf },
            { 0x00c1, MatchaOpcode.CEDirector },
            { 0x0390, MatchaOpcode.CompanyAirshipStatus },
            { 0x0107, MatchaOpcode.CompanySubmersibleStatus },
            { 0x011f, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0353, MatchaOpcode.ResumeEventScene32 },
            { 0x0146, MatchaOpcode.EventPlay },
            { 0x036f, MatchaOpcode.EventStart },
            { 0x0187, MatchaOpcode.Examine },
            { 0x0360, MatchaOpcode.FateInfo },
            { 0x0332, MatchaOpcode.InitZone },
            { 0x0080, MatchaOpcode.InventoryTransaction },
            { 0x03ce, MatchaOpcode.ItemInfo },
            { 0x02f5, MatchaOpcode.MarketBoardItemListing },
            { 0x020e, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0143, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x80ed, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x017e, MatchaOpcode.NpcSpawn },
            { 0x008c, MatchaOpcode.PlayerSetup },
            { 0x02a9, MatchaOpcode.PlayerSpawn },
            { 0x03ad, MatchaOpcode.WorldVisitQueue },
        };
    }
}
