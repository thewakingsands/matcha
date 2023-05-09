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
            { 0x02c2, MatchaOpcode.ActorControl },
            { 0x0256, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x03d0, MatchaOpcode.CompanyAirshipStatus },
            { 0x016d, MatchaOpcode.CompanySubmersibleStatus },
            { 0x038a, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x0284, MatchaOpcode.EventPlay },
            { 0x022f, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x01fe, MatchaOpcode.InitZone },
            { 0x031b, MatchaOpcode.InventoryTransaction },
            { 0x034d, MatchaOpcode.ItemInfo },
            { 0x0379, MatchaOpcode.MarketBoardItemListing },
            { 0x0174, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03cd, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0269, MatchaOpcode.NpcSpawn },
            { 0x02bf, MatchaOpcode.PlayerSetup },
            { 0x0094, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x020b, MatchaOpcode.ActorControl },
            { 0x0139, MatchaOpcode.ActorControlSelf },
            { 0x0239, MatchaOpcode.CEDirector },
            { 0x00b1, MatchaOpcode.CompanyAirshipStatus },
            { 0x0177, MatchaOpcode.CompanySubmersibleStatus },
            { 0x019d, MatchaOpcode.ContentFinderNotifyPop },
            { 0x035c, MatchaOpcode.DirectorStart },
            { 0x0082, MatchaOpcode.EventPlay },
            { 0x0123, MatchaOpcode.Examine },
            { 0x017c, MatchaOpcode.FateInfo },
            { 0x01c5, MatchaOpcode.InitZone },
            { 0x0278, MatchaOpcode.InventoryTransaction },
            { 0x0335, MatchaOpcode.ItemInfo },
            { 0x01b0, MatchaOpcode.MarketBoardItemListing },
            { 0x034c, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0323, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0267, MatchaOpcode.NpcSpawn },
            { 0x0085, MatchaOpcode.PlayerSetup },
            { 0x03ad, MatchaOpcode.PlayerSpawn },
        };
    }
}
