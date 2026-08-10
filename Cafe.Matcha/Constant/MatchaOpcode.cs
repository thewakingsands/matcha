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
            { 0x01da, MatchaOpcode.ActorControl },
            { 0x035d, MatchaOpcode.ActorControlSelf },
            { 0x0092, MatchaOpcode.CEDirector },
            { 0x022f, MatchaOpcode.CompanyAirshipStatus },
            { 0x02b1, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00b8, MatchaOpcode.ContentFinderNotifyPop },
            { 0x012d, MatchaOpcode.ResumeEventScene32 },
            { 0x02dd, MatchaOpcode.EventPlay },
            { 0x016d, MatchaOpcode.EventStart },
            { 0x02bb, MatchaOpcode.Examine },
            { 0x00e9, MatchaOpcode.FateInfo },
            { 0x028d, MatchaOpcode.InitZone },
            { 0x01c8, MatchaOpcode.InventoryTransaction },
            { 0x013a, MatchaOpcode.ItemInfo },
            { 0x0356, MatchaOpcode.MarketBoardItemListing },
            { 0x0256, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0127, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8070, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x006f, MatchaOpcode.NpcSpawn },
            { 0x03dd, MatchaOpcode.PlayerSetup },
            { 0x0398, MatchaOpcode.PlayerSpawn },
            { 0x01ef, MatchaOpcode.SubmarineStatusList },
            { 0x02e6, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x01da, MatchaOpcode.ActorControl },
            { 0x035d, MatchaOpcode.ActorControlSelf },
            { 0x0092, MatchaOpcode.CEDirector },
            { 0x022f, MatchaOpcode.CompanyAirshipStatus },
            { 0x02b1, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00b8, MatchaOpcode.ContentFinderNotifyPop },
            { 0x012d, MatchaOpcode.ResumeEventScene32 },
            { 0x02dd, MatchaOpcode.EventPlay },
            { 0x016d, MatchaOpcode.EventStart },
            { 0x02bb, MatchaOpcode.Examine },
            { 0x00e9, MatchaOpcode.FateInfo },
            { 0x028d, MatchaOpcode.InitZone },
            { 0x01c8, MatchaOpcode.InventoryTransaction },
            { 0x013a, MatchaOpcode.ItemInfo },
            { 0x0356, MatchaOpcode.MarketBoardItemListing },
            { 0x0256, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0127, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8070, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x006f, MatchaOpcode.NpcSpawn },
            { 0x03dd, MatchaOpcode.PlayerSetup },
            { 0x0398, MatchaOpcode.PlayerSpawn },
            { 0x01ef, MatchaOpcode.SubmarineStatusList },
            { 0x02e6, MatchaOpcode.WorldVisitQueue },
        };
    }
}
