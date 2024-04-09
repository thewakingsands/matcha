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
            { 0x0333, MatchaOpcode.ActorControl },
            { 0x0111, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x027a, MatchaOpcode.CompanyAirshipStatus },
            { 0x020c, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0225, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.DirectorStart },
            { 0x012a, MatchaOpcode.EventPlay },
            { 0x02bf, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x031b, MatchaOpcode.InitZone },
            { 0x0201, MatchaOpcode.InventoryTransaction },
            { 0x0344, MatchaOpcode.ItemInfo },
            { 0x03e5, MatchaOpcode.MarketBoardItemListing },
            { 0x02ed, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0109, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x02bd, MatchaOpcode.NpcSpawn },
            { 0x0216, MatchaOpcode.PlayerSetup },
            { 0x00f6, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x030f, MatchaOpcode.ActorControl },
            { 0x01a2, MatchaOpcode.ActorControlSelf },
            { 0x0241, MatchaOpcode.CEDirector },
            { 0x0233, MatchaOpcode.CompanyAirshipStatus },
            { 0x02b6, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0222, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0101, MatchaOpcode.DirectorStart },
            { 0x00a3, MatchaOpcode.EventPlay },
            { 0x01ef, MatchaOpcode.Examine },
            { 0x0379, MatchaOpcode.FateInfo },
            { 0x0341, MatchaOpcode.InitZone },
            { 0x02c8, MatchaOpcode.InventoryTransaction },
            { 0x036d, MatchaOpcode.ItemInfo },
            { 0x01c9, MatchaOpcode.MarketBoardItemListing },
            { 0x01bf, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0074, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01d7, MatchaOpcode.NpcSpawn },
            { 0x02c1, MatchaOpcode.PlayerSetup },
            { 0x031b, MatchaOpcode.PlayerSpawn },
        };
    }
}
