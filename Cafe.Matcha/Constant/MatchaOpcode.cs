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
            { 0x0212, MatchaOpcode.ActorControl },
            { 0x00af, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x01e5, MatchaOpcode.CompanyAirshipStatus },
            { 0x00f3, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01c4, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x03ca, MatchaOpcode.EventPlay },
            { 0x03ae, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x016c, MatchaOpcode.InitZone },
            { 0x02fc, MatchaOpcode.InventoryTransaction },
            { 0x0310, MatchaOpcode.ItemInfo },
            { 0x00f2, MatchaOpcode.MarketBoardItemListing },
            { 0x02d2, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03d7, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf010, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x039a, MatchaOpcode.NpcSpawn },
            { 0x0226, MatchaOpcode.PlayerSetup },
            { 0x0171, MatchaOpcode.PlayerSpawn },
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
