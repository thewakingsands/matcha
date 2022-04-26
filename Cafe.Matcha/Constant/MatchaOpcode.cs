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
            { 0x016f, MatchaOpcode.ActorControl },
            { 0x03ae, MatchaOpcode.ActorControlSelf },
            { 0x0336, MatchaOpcode.CEDirector },
            { 0x024b, MatchaOpcode.CompanyAirshipStatus },
            { 0x0254, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0188, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x0313, MatchaOpcode.EventPlay },
            { 0xf008, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x01f5, MatchaOpcode.InitZone },
            { 0x009b, MatchaOpcode.InventoryTransaction },
            { 0x00e1, MatchaOpcode.ItemInfo },
            { 0x03ac, MatchaOpcode.MarketBoardItemListing },
            { 0x01ac, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02e5, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x02b1, MatchaOpcode.NpcSpawn },
            { 0x0312, MatchaOpcode.PlayerSetup },
            { 0x018f, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x00ff, MatchaOpcode.ActorControl },
            { 0x03b7, MatchaOpcode.ActorControlSelf },
            { 0x01ad, MatchaOpcode.CEDirector },
            { 0x0296, MatchaOpcode.CompanyAirshipStatus },
            { 0x028e, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0230, MatchaOpcode.ContentFinderNotifyPop },
            { 0x027c, MatchaOpcode.DirectorStart },
            { 0x02e5, MatchaOpcode.EventPlay },
            { 0x009c, MatchaOpcode.Examine },
            { 0x035b, MatchaOpcode.FateInfo },
            { 0x0163, MatchaOpcode.InitZone },
            { 0x02ef, MatchaOpcode.InventoryTransaction },
            { 0x02dd, MatchaOpcode.ItemInfo },
            { 0x024d, MatchaOpcode.MarketBoardItemListing },
            { 0x00bd, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00b2, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x007b, MatchaOpcode.NpcSpawn },
            { 0x01bb, MatchaOpcode.PlayerSetup },
            { 0x0208, MatchaOpcode.PlayerSpawn },
        };
    }
}
