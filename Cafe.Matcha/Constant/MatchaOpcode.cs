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
            { 0x0343, MatchaOpcode.ActorControl },
            { 0x020e, MatchaOpcode.ActorControlSelf },
            { 0x016c, MatchaOpcode.CEDirector },
            { 0x021a, MatchaOpcode.CompanyAirshipStatus },
            { 0x0163, MatchaOpcode.CompanySubmersibleStatus },
            { 0x007e, MatchaOpcode.ContentFinderNotifyPop },
            { 0x00d6, MatchaOpcode.ResumeEventScene32 },
            { 0x010f, MatchaOpcode.EventPlay },
            { 0x034c, MatchaOpcode.EventStart },
            { 0x02c4, MatchaOpcode.Examine },
            { 0x00e1, MatchaOpcode.FateInfo },
            { 0x0237, MatchaOpcode.InitZone },
            { 0x0318, MatchaOpcode.InventoryTransaction },
            { 0x01a0, MatchaOpcode.ItemInfo },
            { 0x032d, MatchaOpcode.MarketBoardItemListing },
            { 0x02e8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01cc, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x81a3, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x00f6, MatchaOpcode.NpcSpawn },
            { 0x01d6, MatchaOpcode.PlayerSetup },
            { 0x02aa, MatchaOpcode.PlayerSpawn },
            { 0x035c, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0343, MatchaOpcode.ActorControl },
            { 0x020e, MatchaOpcode.ActorControlSelf },
            { 0x016c, MatchaOpcode.CEDirector },
            { 0x021a, MatchaOpcode.CompanyAirshipStatus },
            { 0x0163, MatchaOpcode.CompanySubmersibleStatus },
            { 0x007e, MatchaOpcode.ContentFinderNotifyPop },
            { 0x00d6, MatchaOpcode.ResumeEventScene32 },
            { 0x010f, MatchaOpcode.EventPlay },
            { 0x034c, MatchaOpcode.EventStart },
            { 0x02c4, MatchaOpcode.Examine },
            { 0x00e1, MatchaOpcode.FateInfo },
            { 0x0237, MatchaOpcode.InitZone },
            { 0x0318, MatchaOpcode.InventoryTransaction },
            { 0x01a0, MatchaOpcode.ItemInfo },
            { 0x032d, MatchaOpcode.MarketBoardItemListing },
            { 0x02e8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01cc, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x81a3, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x00f6, MatchaOpcode.NpcSpawn },
            { 0x01d6, MatchaOpcode.PlayerSetup },
            { 0x02aa, MatchaOpcode.PlayerSpawn },
            { 0x035c, MatchaOpcode.WorldVisitQueue },
        };
    }
}
