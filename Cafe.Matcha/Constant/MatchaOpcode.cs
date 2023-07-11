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
            { 0x019d, MatchaOpcode.ActorControl },
            { 0x00a6, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x03c5, MatchaOpcode.CompanyAirshipStatus },
            { 0x0372, MatchaOpcode.CompanySubmersibleStatus },
            { 0x021a, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.DirectorStart },
            { 0x036f, MatchaOpcode.EventPlay },
            { 0x0228, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x01d5, MatchaOpcode.InitZone },
            { 0x0252, MatchaOpcode.InventoryTransaction },
            { 0x0388, MatchaOpcode.ItemInfo },
            { 0x0221, MatchaOpcode.MarketBoardItemListing },
            { 0x0067, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0069, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x00ac, MatchaOpcode.NpcSpawn },
            { 0x03e6, MatchaOpcode.PlayerSetup },
            { 0x01a7, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x02a5, MatchaOpcode.ActorControl },
            { 0x01a3, MatchaOpcode.ActorControlSelf },
            { 0x03e6, MatchaOpcode.CEDirector },
            { 0x00e1, MatchaOpcode.CompanyAirshipStatus },
            { 0x01e6, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00ca, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0251, MatchaOpcode.DirectorStart },
            { 0x03ce, MatchaOpcode.EventPlay },
            { 0x0255, MatchaOpcode.Examine },
            { 0x02cf, MatchaOpcode.FateInfo },
            { 0x01e3, MatchaOpcode.InitZone },
            { 0x01fc, MatchaOpcode.InventoryTransaction },
            { 0x036b, MatchaOpcode.ItemInfo },
            { 0x03b0, MatchaOpcode.MarketBoardItemListing },
            { 0x00ef, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03ac, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0366, MatchaOpcode.NpcSpawn },
            { 0x03a8, MatchaOpcode.PlayerSetup },
            { 0x00c8, MatchaOpcode.PlayerSpawn },
        };
    }
}
