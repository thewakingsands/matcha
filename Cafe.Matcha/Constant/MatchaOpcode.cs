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
            { 0x0381, MatchaOpcode.ActorControl },
            { 0x014f, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x0132, MatchaOpcode.CompanyAirshipStatus },
            { 0x02b7, MatchaOpcode.CompanySubmersibleStatus },
            { 0x03e0, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.DirectorStart },
            { 0x00fb, MatchaOpcode.EventPlay },
            { 0x0364, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x0347, MatchaOpcode.InitZone },
            { 0x02e9, MatchaOpcode.InventoryTransaction },
            { 0x0128, MatchaOpcode.ItemInfo },
            { 0x00a7, MatchaOpcode.MarketBoardItemListing },
            { 0x01a4, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02b4, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01c4, MatchaOpcode.NpcSpawn },
            { 0x0218, MatchaOpcode.PlayerSetup },
            { 0x02cf, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0342, MatchaOpcode.ActorControl },
            { 0x03d5, MatchaOpcode.ActorControlSelf },
            { 0x03ce, MatchaOpcode.CEDirector },
            { 0x02a7, MatchaOpcode.CompanyAirshipStatus },
            { 0x0146, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00ef, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01e3, MatchaOpcode.DirectorStart },
            { 0x00ae, MatchaOpcode.EventPlay },
            { 0x014e, MatchaOpcode.Examine },
            { 0x01ac, MatchaOpcode.FateInfo },
            { 0x00bc, MatchaOpcode.InitZone },
            { 0x0087, MatchaOpcode.InventoryTransaction },
            { 0x0265, MatchaOpcode.ItemInfo },
            { 0x0345, MatchaOpcode.MarketBoardItemListing },
            { 0x0069, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01fd, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01ca, MatchaOpcode.NpcSpawn },
            { 0x0322, MatchaOpcode.PlayerSetup },
            { 0x0391, MatchaOpcode.PlayerSpawn },
        };
    }
}
