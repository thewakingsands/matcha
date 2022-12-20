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
            { 0x0197, MatchaOpcode.ActorControl },
            { 0x0238, MatchaOpcode.ActorControlSelf },
            { 0x018c, MatchaOpcode.CEDirector },
            { 0x02de, MatchaOpcode.CompanyAirshipStatus },
            { 0x02a3, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0173, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x038c, MatchaOpcode.EventPlay },
            { 0x0141, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x00f0, MatchaOpcode.InitZone },
            { 0x02e2, MatchaOpcode.InventoryTransaction },
            { 0x03e3, MatchaOpcode.ItemInfo },
            { 0x0270, MatchaOpcode.MarketBoardItemListing },
            { 0x03c3, MatchaOpcode.MarketBoardItemListingCount },
            { 0x036c, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0359, MatchaOpcode.NpcSpawn },
            { 0x0091, MatchaOpcode.PlayerSetup },
            { 0x00dd, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0365, MatchaOpcode.ActorControl },
            { 0x0245, MatchaOpcode.ActorControlSelf },
            { 0x008c, MatchaOpcode.CEDirector },
            { 0x0272, MatchaOpcode.CompanyAirshipStatus },
            { 0x026e, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0171, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0369, MatchaOpcode.DirectorStart },
            { 0x0067, MatchaOpcode.EventPlay },
            { 0x00ed, MatchaOpcode.Examine },
            { 0x03d2, MatchaOpcode.FateInfo },
            { 0x0356, MatchaOpcode.InitZone },
            { 0x0373, MatchaOpcode.InventoryTransaction },
            { 0x0353, MatchaOpcode.ItemInfo },
            { 0x0069, MatchaOpcode.MarketBoardItemListing },
            { 0x007e, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0141, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x011d, MatchaOpcode.NpcSpawn },
            { 0x0217, MatchaOpcode.PlayerSetup },
            { 0x02b6, MatchaOpcode.PlayerSpawn },
        };
    }
}
