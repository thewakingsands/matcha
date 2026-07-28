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
            { 0x0112, MatchaOpcode.ActorControl },
            { 0x020e, MatchaOpcode.ActorControlSelf },
            { 0x0097, MatchaOpcode.CEDirector },
            { 0x0145, MatchaOpcode.CompanyAirshipStatus },
            { 0x007a, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0148, MatchaOpcode.ContentFinderNotifyPop },
            { 0x02de, MatchaOpcode.ResumeEventScene32 },
            { 0x015a, MatchaOpcode.EventPlay },
            { 0x00b4, MatchaOpcode.EventStart },
            { 0x0288, MatchaOpcode.Examine },
            { 0x00a6, MatchaOpcode.FateInfo },
            { 0x02d9, MatchaOpcode.InitZone },
            { 0x01df, MatchaOpcode.InventoryTransaction },
            { 0x01ea, MatchaOpcode.ItemInfo },
            { 0x015c, MatchaOpcode.MarketBoardItemListing },
            { 0x00e6, MatchaOpcode.MarketBoardItemListingCount },
            { 0x013f, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8172, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0080, MatchaOpcode.NpcSpawn },
            { 0x03a7, MatchaOpcode.PlayerSetup },
            { 0x0071, MatchaOpcode.PlayerSpawn },
            { 0x037f, MatchaOpcode.SubmarineStatusList },
            { 0x0269, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0112, MatchaOpcode.ActorControl },
            { 0x020e, MatchaOpcode.ActorControlSelf },
            { 0x0097, MatchaOpcode.CEDirector },
            { 0x0145, MatchaOpcode.CompanyAirshipStatus },
            { 0x007a, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0148, MatchaOpcode.ContentFinderNotifyPop },
            { 0x02de, MatchaOpcode.ResumeEventScene32 },
            { 0x015a, MatchaOpcode.EventPlay },
            { 0x00b4, MatchaOpcode.EventStart },
            { 0x0288, MatchaOpcode.Examine },
            { 0x00a6, MatchaOpcode.FateInfo },
            { 0x02d9, MatchaOpcode.InitZone },
            { 0x01df, MatchaOpcode.InventoryTransaction },
            { 0x01ea, MatchaOpcode.ItemInfo },
            { 0x015c, MatchaOpcode.MarketBoardItemListing },
            { 0x00e6, MatchaOpcode.MarketBoardItemListingCount },
            { 0x013f, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8172, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0080, MatchaOpcode.NpcSpawn },
            { 0x03a7, MatchaOpcode.PlayerSetup },
            { 0x0071, MatchaOpcode.PlayerSpawn },
            { 0x037f, MatchaOpcode.SubmarineStatusList },
            { 0x0269, MatchaOpcode.WorldVisitQueue },
        };
    }
}
