// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    using System.Collections.Generic;

    internal enum MatchaOpcode
    {
        ActorControlSelf,
        CEDirector,
        CompanyAirshipStatus,
        CompanySubmersibleStatus,
        ContentFinderNotifyPop,
        DirectorStart,
        EventPlay,
        Examine,
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
            { 0x0301, MatchaOpcode.ActorControlSelf },
            { 0x010a, MatchaOpcode.CEDirector },
            { 0x0142, MatchaOpcode.CompanyAirshipStatus },
            { 0x02b1, MatchaOpcode.CompanySubmersibleStatus },
            { 0x016e, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x027a, MatchaOpcode.EventPlay },
            { 0x03ae, MatchaOpcode.Examine },
            { 0x017f, MatchaOpcode.InitZone },
            { 0x0134, MatchaOpcode.InventoryTransaction },
            { 0x00ec, MatchaOpcode.ItemInfo },
            { 0x014b, MatchaOpcode.MarketBoardItemListing },
            { 0x0370, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0103, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x032f, MatchaOpcode.NpcSpawn },
            { 0x01db, MatchaOpcode.PlayerSetup },
            { 0x03dc, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0205, MatchaOpcode.ActorControlSelf },
            { 0x00ea, MatchaOpcode.CEDirector },
            { 0x02c9, MatchaOpcode.CompanyAirshipStatus },
            { 0x01cd, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02a0, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01c7, MatchaOpcode.DirectorStart },
            { 0x0310, MatchaOpcode.EventPlay },
            { 0x0299, MatchaOpcode.Examine },
            { 0x0286, MatchaOpcode.InitZone },
            { 0x02fe, MatchaOpcode.InventoryTransaction },
            { 0x0115, MatchaOpcode.ItemInfo },
            { 0x0255, MatchaOpcode.MarketBoardItemListing },
            { 0x030f, MatchaOpcode.MarketBoardItemListingCount },
            { 0x006b, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0318, MatchaOpcode.NpcSpawn },
            { 0x03b2, MatchaOpcode.PlayerSetup },
            { 0x03b3, MatchaOpcode.PlayerSpawn },
        };
    }
}
