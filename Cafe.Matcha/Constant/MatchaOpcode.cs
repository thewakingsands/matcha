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
            { 0x035a, MatchaOpcode.ActorControlSelf },
            { 0x0160, MatchaOpcode.CEDirector },
            { 0x02a5, MatchaOpcode.CompanyAirshipStatus },
            { 0x037a, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00dc, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0258, MatchaOpcode.DirectorStart },
            { 0x0369, MatchaOpcode.EventPlay },
            { 0x027c, MatchaOpcode.Examine },
            { 0x021c, MatchaOpcode.InitZone },
            { 0x02fa, MatchaOpcode.InventoryTransaction },
            { 0x02d3, MatchaOpcode.ItemInfo },
            { 0x026b, MatchaOpcode.MarketBoardItemListing },
            { 0x0277, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0320, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01e7, MatchaOpcode.PlayerSetup },
            { 0x02c1, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0077, MatchaOpcode.ActorControlSelf },
            { 0x009e, MatchaOpcode.CEDirector },
            { 0x027b, MatchaOpcode.CompanyAirshipStatus },
            { 0x0399, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0231, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0203, MatchaOpcode.DirectorStart },
            { 0x035a, MatchaOpcode.EventPlay },
            { 0x01b4, MatchaOpcode.Examine },
            { 0x0307, MatchaOpcode.InitZone },
            { 0x01a5, MatchaOpcode.InventoryTransaction },
            { 0x0322, MatchaOpcode.ItemInfo },
            { 0x0272, MatchaOpcode.MarketBoardItemListing },
            { 0x01c3, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0325, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x02c1, MatchaOpcode.PlayerSetup },
            { 0x0082, MatchaOpcode.PlayerSpawn },
        };
    }
}
