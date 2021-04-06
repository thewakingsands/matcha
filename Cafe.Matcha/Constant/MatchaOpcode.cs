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
            { 0x030c, MatchaOpcode.ActorControlSelf },
            { 0x0129, MatchaOpcode.CEDirector },
            { 0x0179, MatchaOpcode.CompanyAirshipStatus },
            { 0x0135, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02b1, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01b9, MatchaOpcode.DirectorStart },
            { 0x0374, MatchaOpcode.EventPlay },
            { 0x012c, MatchaOpcode.Examine },
            { 0x0245, MatchaOpcode.InitZone },
            { 0x033b, MatchaOpcode.InventoryTransaction },
            { 0x01c9, MatchaOpcode.ItemInfo },
            { 0x03b5, MatchaOpcode.MarketBoardItemListing },
            { 0x0193, MatchaOpcode.MarketBoardItemListingCount },
            { 0x012d, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x010d, MatchaOpcode.PlayerSetup },
            { 0x030a, MatchaOpcode.PlayerSpawn },
        };
    }
}
