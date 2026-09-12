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
            { 0x038c, MatchaOpcode.ActorControl },
            { 0x0258, MatchaOpcode.ActorControlSelf },
            { 0x0393, MatchaOpcode.CEDirector },
            { 0x02f8, MatchaOpcode.CompanyAirshipStatus },
            { 0x0222, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0080, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0335, MatchaOpcode.ResumeEventScene32 },
            { 0x01f1, MatchaOpcode.EventPlay },
            { 0x00f2, MatchaOpcode.EventStart },
            { 0x0069, MatchaOpcode.Examine },
            { 0x0106, MatchaOpcode.FateInfo },
            { 0x03a1, MatchaOpcode.InitZone },
            { 0x024e, MatchaOpcode.InventoryTransaction },
            { 0x0073, MatchaOpcode.ItemInfo },
            { 0x027b, MatchaOpcode.MarketBoardItemListing },
            { 0x0324, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02fe, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x825d, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x01c4, MatchaOpcode.NpcSpawn },
            { 0x01dd, MatchaOpcode.PlayerSetup },
            { 0x03b2, MatchaOpcode.PlayerSpawn },
            { 0x01a9, MatchaOpcode.SubmarineStatusList },
            { 0x0110, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x038c, MatchaOpcode.ActorControl },
            { 0x0258, MatchaOpcode.ActorControlSelf },
            { 0x0393, MatchaOpcode.CEDirector },
            { 0x02f8, MatchaOpcode.CompanyAirshipStatus },
            { 0x0222, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0080, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0335, MatchaOpcode.ResumeEventScene32 },
            { 0x01f1, MatchaOpcode.EventPlay },
            { 0x00f2, MatchaOpcode.EventStart },
            { 0x0069, MatchaOpcode.Examine },
            { 0x0106, MatchaOpcode.FateInfo },
            { 0x03a1, MatchaOpcode.InitZone },
            { 0x024e, MatchaOpcode.InventoryTransaction },
            { 0x0073, MatchaOpcode.ItemInfo },
            { 0x027b, MatchaOpcode.MarketBoardItemListing },
            { 0x0324, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02fe, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x825d, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x01c4, MatchaOpcode.NpcSpawn },
            { 0x01dd, MatchaOpcode.PlayerSetup },
            { 0x03b2, MatchaOpcode.PlayerSpawn },
            { 0x01a9, MatchaOpcode.SubmarineStatusList },
            { 0x0110, MatchaOpcode.WorldVisitQueue },
        };
    }
}
