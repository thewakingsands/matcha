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
            { 0x0335, MatchaOpcode.ActorControl },
            { 0x02ed, MatchaOpcode.ActorControlSelf },
            { 0x02ee, MatchaOpcode.CEDirector },
            { 0x0195, MatchaOpcode.CompanyAirshipStatus },
            { 0x00ed, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00c9, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x02db, MatchaOpcode.EventPlay },
            { 0x03b6, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x036a, MatchaOpcode.InitZone },
            { 0x03d4, MatchaOpcode.InventoryTransaction },
            { 0x02ce, MatchaOpcode.ItemInfo },
            { 0x0159, MatchaOpcode.MarketBoardItemListing },
            { 0x037b, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0376, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x02e4, MatchaOpcode.NpcSpawn },
            { 0x0294, MatchaOpcode.PlayerSetup },
            { 0x029d, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x01d4, MatchaOpcode.ActorControl },
            { 0x012c, MatchaOpcode.ActorControlSelf },
            { 0x00d1, MatchaOpcode.CEDirector },
            { 0x0288, MatchaOpcode.CompanyAirshipStatus },
            { 0x032d, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0317, MatchaOpcode.ContentFinderNotifyPop },
            { 0x013b, MatchaOpcode.DirectorStart },
            { 0x0282, MatchaOpcode.EventPlay },
            { 0x03e0, MatchaOpcode.Examine },
            { 0x0237, MatchaOpcode.FateInfo },
            { 0x012e, MatchaOpcode.InitZone },
            { 0x0244, MatchaOpcode.InventoryTransaction },
            { 0x02d3, MatchaOpcode.ItemInfo },
            { 0x00fd, MatchaOpcode.MarketBoardItemListing },
            { 0x0349, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01d7, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x015d, MatchaOpcode.NpcSpawn },
            { 0x0205, MatchaOpcode.PlayerSetup },
            { 0x0068, MatchaOpcode.PlayerSpawn },
        };
    }
}
