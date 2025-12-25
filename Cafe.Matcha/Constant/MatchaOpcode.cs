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
            { 0x019b, MatchaOpcode.ActorControl },
            { 0x0347, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x03d5, MatchaOpcode.CompanyAirshipStatus },
            { 0x0139, MatchaOpcode.CompanySubmersibleStatus },
            { 0x03a7, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0332, MatchaOpcode.EventPlay },
            { 0x03da, MatchaOpcode.EventStart },
            { 0x0177, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x0242, MatchaOpcode.InitZone },
            { 0x02b7, MatchaOpcode.InventoryTransaction },
            { 0x00eb, MatchaOpcode.ItemInfo },
            { 0x0163, MatchaOpcode.MarketBoardItemListing },
            { 0x01ad, MatchaOpcode.MarketBoardItemListingCount },
            { 0x007e, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x039b, MatchaOpcode.NpcSpawn },
            { 0x0256, MatchaOpcode.PlayerSetup },
            { 0x00ca, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x019b, MatchaOpcode.ActorControl },
            { 0x0347, MatchaOpcode.ActorControlSelf },
            { 0x0305, MatchaOpcode.CEDirector },
            { 0x03d5, MatchaOpcode.CompanyAirshipStatus },
            { 0x0139, MatchaOpcode.CompanySubmersibleStatus },
            { 0x03a7, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0199, MatchaOpcode.ResumeEventScene32 },
            { 0x0332, MatchaOpcode.EventPlay },
            { 0x03da, MatchaOpcode.EventStart },
            { 0x0070, MatchaOpcode.Examine },
            { 0x038a, MatchaOpcode.FateInfo },
            { 0x0242, MatchaOpcode.InitZone },
            { 0x02b7, MatchaOpcode.InventoryTransaction },
            { 0x00eb, MatchaOpcode.ItemInfo },
            { 0x01ad, MatchaOpcode.MarketBoardItemListing },
            { 0x01d8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x025c, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x836f, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x039b, MatchaOpcode.NpcSpawn },
            { 0x0256, MatchaOpcode.PlayerSetup },
            { 0x00ca, MatchaOpcode.PlayerSpawn },
            { 0x036f, MatchaOpcode.WorldVisitQueue },
        };
    }
}
