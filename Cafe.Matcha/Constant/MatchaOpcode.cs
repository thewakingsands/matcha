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
            { 0x00cb, MatchaOpcode.ActorControl },
            { 0x03cd, MatchaOpcode.ActorControlSelf },
            { 0x0172, MatchaOpcode.CEDirector },
            { 0x02d6, MatchaOpcode.CompanyAirshipStatus },
            { 0x01f0, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0147, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x00b6, MatchaOpcode.EventPlay },
            { 0xf008, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x010f, MatchaOpcode.InitZone },
            { 0x03c3, MatchaOpcode.InventoryTransaction },
            { 0x01a0, MatchaOpcode.ItemInfo },
            { 0x0371, MatchaOpcode.MarketBoardItemListing },
            { 0x033c, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02ed, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01a4, MatchaOpcode.NpcSpawn },
            { 0x0304, MatchaOpcode.PlayerSetup },
            { 0x020c, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x022c, MatchaOpcode.ActorControl },
            { 0x010c, MatchaOpcode.ActorControlSelf },
            { 0x0285, MatchaOpcode.CEDirector },
            { 0x00a4, MatchaOpcode.CompanyAirshipStatus },
            { 0x03d6, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02a6, MatchaOpcode.ContentFinderNotifyPop },
            { 0x00e5, MatchaOpcode.DirectorStart },
            { 0x0306, MatchaOpcode.EventPlay },
            { 0x0259, MatchaOpcode.Examine },
            { 0x027a, MatchaOpcode.FateInfo },
            { 0x03cc, MatchaOpcode.InitZone },
            { 0x013f, MatchaOpcode.InventoryTransaction },
            { 0x0387, MatchaOpcode.ItemInfo },
            { 0x03d3, MatchaOpcode.MarketBoardItemListing },
            { 0x011c, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01c4, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x03a7, MatchaOpcode.NpcSpawn },
            { 0x0142, MatchaOpcode.PlayerSetup },
            { 0x0106, MatchaOpcode.PlayerSpawn },
        };
    }
}
