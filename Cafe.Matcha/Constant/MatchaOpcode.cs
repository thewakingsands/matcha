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
            { 0x01db, MatchaOpcode.PlayerSetup },
            { 0x03dc, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0325, MatchaOpcode.ActorControlSelf },
            { 0x025d, MatchaOpcode.CEDirector },
            { 0x0197, MatchaOpcode.CompanyAirshipStatus },
            { 0x0169, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0336, MatchaOpcode.ContentFinderNotifyPop },
            { 0x02a9, MatchaOpcode.DirectorStart },
            { 0x02d6, MatchaOpcode.EventPlay },
            { 0x0372, MatchaOpcode.Examine },
            { 0x03e1, MatchaOpcode.InitZone },
            { 0x03e0, MatchaOpcode.InventoryTransaction },
            { 0x035d, MatchaOpcode.ItemInfo },
            { 0x01ec, MatchaOpcode.MarketBoardItemListing },
            { 0x02d5, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0285, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01a0, MatchaOpcode.PlayerSetup },
            { 0x014c, MatchaOpcode.PlayerSpawn },
        };
    }
}
