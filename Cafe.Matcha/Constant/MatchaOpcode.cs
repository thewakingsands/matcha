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
            { 0x02a7, MatchaOpcode.ActorControl },
            { 0x023c, MatchaOpcode.ActorControlSelf },
            { 0x0108, MatchaOpcode.CEDirector },
            { 0x00ef, MatchaOpcode.CompanyAirshipStatus },
            { 0x031b, MatchaOpcode.CompanySubmersibleStatus },
            { 0x018c, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x02fd, MatchaOpcode.EventPlay },
            { 0x03e0, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x00e1, MatchaOpcode.InitZone },
            { 0x017d, MatchaOpcode.InventoryTransaction },
            { 0x02ed, MatchaOpcode.ItemInfo },
            { 0x0201, MatchaOpcode.MarketBoardItemListing },
            { 0x02a1, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0194, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x019b, MatchaOpcode.NpcSpawn },
            { 0x0342, MatchaOpcode.PlayerSetup },
            { 0x0334, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x012d, MatchaOpcode.ActorControl },
            { 0x02b4, MatchaOpcode.ActorControlSelf },
            { 0x0356, MatchaOpcode.CEDirector },
            { 0x01e7, MatchaOpcode.CompanyAirshipStatus },
            { 0x0101, MatchaOpcode.CompanySubmersibleStatus },
            { 0x03d1, MatchaOpcode.ContentFinderNotifyPop },
            { 0x011a, MatchaOpcode.DirectorStart },
            { 0x0321, MatchaOpcode.EventPlay },
            { 0x01c7, MatchaOpcode.Examine },
            { 0x01b5, MatchaOpcode.FateInfo },
            { 0x019b, MatchaOpcode.InitZone },
            { 0x0281, MatchaOpcode.InventoryTransaction },
            { 0x03bc, MatchaOpcode.ItemInfo },
            { 0x0190, MatchaOpcode.MarketBoardItemListing },
            { 0x00ef, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0351, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x03cb, MatchaOpcode.NpcSpawn },
            { 0x00a7, MatchaOpcode.PlayerSetup },
            { 0x02c6, MatchaOpcode.PlayerSpawn },
        };
    }
}
