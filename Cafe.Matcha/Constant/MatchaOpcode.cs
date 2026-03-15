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
            { 0x00e7, MatchaOpcode.ActorControl },
            { 0x0217, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x00cc, MatchaOpcode.CompanyAirshipStatus },
            { 0x0074, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0285, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x02b4, MatchaOpcode.EventPlay },
            { 0x00bc, MatchaOpcode.EventStart },
            { 0x036c, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x0246, MatchaOpcode.InitZone },
            { 0x011a, MatchaOpcode.InventoryTransaction },
            { 0x01a8, MatchaOpcode.ItemInfo },
            { 0x01c2, MatchaOpcode.MarketBoardItemListing },
            { 0x00a5, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02ef, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x03c3, MatchaOpcode.NpcSpawn },
            { 0x016d, MatchaOpcode.PlayerSetup },
            { 0x03ca, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x00e7, MatchaOpcode.ActorControl },
            { 0x0217, MatchaOpcode.ActorControlSelf },
            { 0x02f5, MatchaOpcode.CEDirector },
            { 0x00cc, MatchaOpcode.CompanyAirshipStatus },
            { 0x0074, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0285, MatchaOpcode.ContentFinderNotifyPop },
            { 0x015e, MatchaOpcode.ResumeEventScene32 },
            { 0x02b4, MatchaOpcode.EventPlay },
            { 0x00bc, MatchaOpcode.EventStart },
            { 0x03c4, MatchaOpcode.Examine },
            { 0x02fa, MatchaOpcode.FateInfo },
            { 0x0246, MatchaOpcode.InitZone },
            { 0x011a, MatchaOpcode.InventoryTransaction },
            { 0x01a8, MatchaOpcode.ItemInfo },
            { 0x01c2, MatchaOpcode.MarketBoardItemListing },
            { 0x00a5, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02ef, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8350, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x03c3, MatchaOpcode.NpcSpawn },
            { 0x016d, MatchaOpcode.PlayerSetup },
            { 0x03ca, MatchaOpcode.PlayerSpawn },
            { 0x0350, MatchaOpcode.WorldVisitQueue },
        };
    }
}
