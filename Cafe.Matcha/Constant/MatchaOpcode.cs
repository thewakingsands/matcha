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
            { 0x03d5, MatchaOpcode.ActorControlSelf },
            { 0x01f5, MatchaOpcode.CEDirector },
            { 0x0206, MatchaOpcode.CompanyAirshipStatus },
            { 0x013b, MatchaOpcode.CompanySubmersibleStatus },
            { 0x026e, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01d8, MatchaOpcode.DirectorStart },
            { 0x0276, MatchaOpcode.EventPlay },
            { 0x0261, MatchaOpcode.Examine },
            { 0x0233, MatchaOpcode.InitZone },
            { 0x02ee, MatchaOpcode.InventoryTransaction },
            { 0x0175, MatchaOpcode.ItemInfo },
            { 0x016b, MatchaOpcode.MarketBoardItemListing },
            { 0x00c0, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01c3, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01e9, MatchaOpcode.PlayerSetup },
            { 0x01ab, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x007c, MatchaOpcode.ActorControlSelf },
            { 0x0144, MatchaOpcode.CEDirector },
            { 0x0271, MatchaOpcode.CompanyAirshipStatus },
            { 0x0345, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02d0, MatchaOpcode.ContentFinderNotifyPop },
            { 0x02f2, MatchaOpcode.DirectorStart },
            { 0x01b9, MatchaOpcode.EventPlay },
            { 0x0316, MatchaOpcode.Examine },
            { 0x02d2, MatchaOpcode.InitZone },
            { 0x00a8, MatchaOpcode.InventoryTransaction },
            { 0x031a, MatchaOpcode.ItemInfo },
            { 0x0158, MatchaOpcode.MarketBoardItemListing },
            { 0x0280, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01f3, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01f9, MatchaOpcode.PlayerSetup },
            { 0x00d1, MatchaOpcode.PlayerSpawn },
        };
    }
}
