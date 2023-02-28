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
        DirectorStart,
        EventPlay,
        Examine,
        FateInfo,
        InitZone,
        InventoryTransaction,
        ItemInfo,
        MarketBoardItemListing,
        MarketBoardItemListingCount,
        MarketBoardItemListingHistory,
        NpcSpawn,
        PlayerSetup,
        PlayerSpawn,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x01a4, MatchaOpcode.ActorControl },
            { 0x0203, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x00da, MatchaOpcode.CompanyAirshipStatus },
            { 0x0263, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02a1, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x03b8, MatchaOpcode.EventPlay },
            { 0x0246, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x0118, MatchaOpcode.InitZone },
            { 0x006e, MatchaOpcode.InventoryTransaction },
            { 0x01c2, MatchaOpcode.ItemInfo },
            { 0x01ed, MatchaOpcode.MarketBoardItemListing },
            { 0x031a, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0176, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x03d5, MatchaOpcode.NpcSpawn },
            { 0x0287, MatchaOpcode.PlayerSetup },
            { 0x00f9, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x03d0, MatchaOpcode.ActorControl },
            { 0x00f1, MatchaOpcode.ActorControlSelf },
            { 0x039d, MatchaOpcode.CEDirector },
            { 0x013b, MatchaOpcode.CompanyAirshipStatus },
            { 0x016e, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02dd, MatchaOpcode.ContentFinderNotifyPop },
            { 0x00f2, MatchaOpcode.DirectorStart },
            { 0x022f, MatchaOpcode.EventPlay },
            { 0x00ec, MatchaOpcode.Examine },
            { 0x032a, MatchaOpcode.FateInfo },
            { 0x00ce, MatchaOpcode.InitZone },
            { 0x00a7, MatchaOpcode.InventoryTransaction },
            { 0x0399, MatchaOpcode.ItemInfo },
            { 0x0102, MatchaOpcode.MarketBoardItemListing },
            { 0x02e6, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0231, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01c3, MatchaOpcode.NpcSpawn },
            { 0x01af, MatchaOpcode.PlayerSetup },
            { 0x008f, MatchaOpcode.PlayerSpawn },
        };
    }
}
