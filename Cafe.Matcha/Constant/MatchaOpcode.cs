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
        PlayerSetup,
        PlayerSpawn,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x023e, MatchaOpcode.ActorControlSelf },
            { 0x0160, MatchaOpcode.CEDirector },
            { 0x03d2, MatchaOpcode.CompanyAirshipStatus },
            { 0x0288, MatchaOpcode.CompanySubmersibleStatus },
            { 0x018f, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01d8, MatchaOpcode.DirectorStart },
            { 0x032b, MatchaOpcode.EventPlay },
            { 0x0252, MatchaOpcode.Examine },
            { 0x0085, MatchaOpcode.InitZone },
            { 0x02c5, MatchaOpcode.InventoryTransaction },
            { 0x0130, MatchaOpcode.ItemInfo },
            { 0x024d, MatchaOpcode.MarketBoardItemListing },
            { 0x0311, MatchaOpcode.MarketBoardItemListingCount },
            { 0x031d, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x034d, MatchaOpcode.PlayerSetup },
            { 0x036f, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0078, MatchaOpcode.ActorControlSelf },
            { 0x0120, MatchaOpcode.CEDirector },
            { 0x0142, MatchaOpcode.CompanyAirshipStatus },
            { 0x0132, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01ad, MatchaOpcode.ContentFinderNotifyPop },
            { 0x00b0, MatchaOpcode.DirectorStart },
            { 0x0260, MatchaOpcode.EventPlay },
            { 0x038b, MatchaOpcode.Examine },
            { 0x02a6, MatchaOpcode.InitZone },
            { 0x016d, MatchaOpcode.InventoryTransaction },
            { 0x0242, MatchaOpcode.ItemInfo },
            { 0x0116, MatchaOpcode.MarketBoardItemListing },
            { 0x0314, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0161, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0370, MatchaOpcode.PlayerSetup },
            { 0x029a, MatchaOpcode.PlayerSpawn },
        };
    }
}
