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
            { 0x00a1, MatchaOpcode.ActorControl },
            { 0x03bd, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x02ee, MatchaOpcode.CompanyAirshipStatus },
            { 0x01ad, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0387, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0083, MatchaOpcode.EventPlay },
            { 0x0107, MatchaOpcode.EventStart },
            { 0x0349, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x03a3, MatchaOpcode.InitZone },
            { 0x00eb, MatchaOpcode.InventoryTransaction },
            { 0x00f6, MatchaOpcode.ItemInfo },
            { 0x027d, MatchaOpcode.MarketBoardItemListing },
            { 0x011e, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03c1, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x02ea, MatchaOpcode.NpcSpawn },
            { 0x026f, MatchaOpcode.PlayerSetup },
            { 0x00e3, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0167, MatchaOpcode.ActorControl },
            { 0x0110, MatchaOpcode.ActorControlSelf },
            { 0x0364, MatchaOpcode.CEDirector },
            { 0x02f4, MatchaOpcode.CompanyAirshipStatus },
            { 0x00e5, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0080, MatchaOpcode.ContentFinderNotifyPop },
            { 0x024e, MatchaOpcode.ResumeEventScene32 },
            { 0x03b5, MatchaOpcode.EventPlay },
            { 0x0235, MatchaOpcode.EventStart },
            { 0x0340, MatchaOpcode.Examine },
            { 0x02e5, MatchaOpcode.FateInfo },
            { 0x0362, MatchaOpcode.InitZone },
            { 0x01a0, MatchaOpcode.InventoryTransaction },
            { 0x00b7, MatchaOpcode.ItemInfo },
            { 0x02c1, MatchaOpcode.MarketBoardItemListing },
            { 0x013f, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02ed, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8308, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x00fc, MatchaOpcode.NpcSpawn },
            { 0x0300, MatchaOpcode.PlayerSetup },
            { 0x03e7, MatchaOpcode.PlayerSpawn },
            { 0x03af, MatchaOpcode.WorldVisitQueue },
        };
    }
}
