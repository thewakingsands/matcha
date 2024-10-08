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
        MarketBoardRequestItemListingInfo,
        NpcSpawn,
        PlayerSetup,
        PlayerSpawn,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0178, MatchaOpcode.ActorControl },
            { 0x02a7, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x0371, MatchaOpcode.CompanyAirshipStatus },
            { 0x00f8, MatchaOpcode.CompanySubmersibleStatus },
            { 0x014f, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.DirectorStart },
            { 0x026d, MatchaOpcode.EventPlay },
            { 0x0326, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x01f1, MatchaOpcode.InitZone },
            { 0x00e8, MatchaOpcode.InventoryTransaction },
            { 0x0236, MatchaOpcode.ItemInfo },
            { 0x01c3, MatchaOpcode.MarketBoardItemListing },
            { 0x00a0, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0102, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0186, MatchaOpcode.NpcSpawn },
            { 0x00c6, MatchaOpcode.PlayerSetup },
            { 0x024f, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0065, MatchaOpcode.ActorControl },
            { 0x0127, MatchaOpcode.ActorControlSelf },
            { 0x007d, MatchaOpcode.CEDirector },
            { 0x0200, MatchaOpcode.CompanyAirshipStatus },
            { 0x00e0, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00e4, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0339, MatchaOpcode.DirectorStart },
            { 0x018d, MatchaOpcode.EventPlay },
            { 0x007e, MatchaOpcode.Examine },
            { 0x03b6, MatchaOpcode.FateInfo },
            { 0x0093, MatchaOpcode.InitZone },
            { 0x0156, MatchaOpcode.InventoryTransaction },
            { 0x01ec, MatchaOpcode.ItemInfo },
            { 0x0333, MatchaOpcode.MarketBoardItemListing },
            { 0x0397, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02f6, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x81c8, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x032f, MatchaOpcode.NpcSpawn },
            { 0x01b1, MatchaOpcode.PlayerSetup },
            { 0x00a1, MatchaOpcode.PlayerSpawn },
        };
    }
}
