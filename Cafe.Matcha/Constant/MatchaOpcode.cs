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
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x01c5, MatchaOpcode.ActorControl },
            { 0x018b, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x0097, MatchaOpcode.CompanyAirshipStatus },
            { 0x02ba, MatchaOpcode.CompanySubmersibleStatus },
            { 0x013f, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x0206, MatchaOpcode.EventPlay },
            { 0x02b5, MatchaOpcode.EventStart },
            { 0x033d, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x0247, MatchaOpcode.InitZone },
            { 0x028b, MatchaOpcode.InventoryTransaction },
            { 0x0380, MatchaOpcode.ItemInfo },
            { 0x0311, MatchaOpcode.MarketBoardItemListing },
            { 0x01ec, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01a2, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x03a3, MatchaOpcode.NpcSpawn },
            { 0x036d, MatchaOpcode.PlayerSetup },
            { 0x012a, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0084, MatchaOpcode.ActorControl },
            { 0x007b, MatchaOpcode.ActorControlSelf },
            { 0x01b5, MatchaOpcode.CEDirector },
            { 0x0069, MatchaOpcode.CompanyAirshipStatus },
            { 0x02e4, MatchaOpcode.CompanySubmersibleStatus },
            { 0x007d, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0159, MatchaOpcode.ResumeEventScene32 },
            { 0x027c, MatchaOpcode.EventPlay },
            { 0x02bb, MatchaOpcode.EventStart },
            { 0x0096, MatchaOpcode.Examine },
            { 0x0242, MatchaOpcode.FateInfo },
            { 0x0317, MatchaOpcode.InitZone },
            { 0x0073, MatchaOpcode.InventoryTransaction },
            { 0x01d6, MatchaOpcode.ItemInfo },
            { 0x031c, MatchaOpcode.MarketBoardItemListing },
            { 0x027f, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00df, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8380, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x00a4, MatchaOpcode.NpcSpawn },
            { 0x0207, MatchaOpcode.PlayerSetup },
            { 0x0125, MatchaOpcode.PlayerSpawn },
        };
    }
}
