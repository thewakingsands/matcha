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
            { 0x02b6, MatchaOpcode.ActorControlSelf },
            { 0x0104, MatchaOpcode.CEDirector },
            { 0x0166, MatchaOpcode.CompanyAirshipStatus },
            { 0x0247, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0327, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01dd, MatchaOpcode.DirectorStart },
            { 0x016b, MatchaOpcode.EventPlay },
            { 0x0365, MatchaOpcode.Examine },
            { 0x0320, MatchaOpcode.InitZone },
            { 0x027f, MatchaOpcode.InventoryTransaction },
            { 0x01cc, MatchaOpcode.ItemInfo },
            { 0x0076, MatchaOpcode.MarketBoardItemListing },
            { 0x0068, MatchaOpcode.MarketBoardItemListingCount },
            { 0x01ba, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01d5, MatchaOpcode.PlayerSetup },
            { 0x01d8, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0190, MatchaOpcode.ActorControlSelf },
            { 0x0308, MatchaOpcode.CEDirector },
            { 0x0171, MatchaOpcode.CompanyAirshipStatus },
            { 0x02ba, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00ee, MatchaOpcode.ContentFinderNotifyPop },
            { 0x02da, MatchaOpcode.DirectorStart },
            { 0x014d, MatchaOpcode.EventPlay },
            { 0x0334, MatchaOpcode.Examine },
            { 0x036e, MatchaOpcode.InitZone },
            { 0x0376, MatchaOpcode.InventoryTransaction },
            { 0x0322, MatchaOpcode.ItemInfo },
            { 0x0315, MatchaOpcode.MarketBoardItemListing },
            { 0x00d8, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03e0, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x01bf, MatchaOpcode.PlayerSetup },
            { 0x0323, MatchaOpcode.PlayerSpawn },
        };
    }
}
