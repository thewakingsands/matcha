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
            { 0x0096, MatchaOpcode.ActorControlSelf },
            { 0x01f8, MatchaOpcode.CEDirector },
            { 0x01c7, MatchaOpcode.CompanyAirshipStatus },
            { 0x0211, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0183, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x013f, MatchaOpcode.EventPlay },
            { 0x02ab, MatchaOpcode.Examine },
            { 0x0137, MatchaOpcode.InitZone },
            { 0x0269, MatchaOpcode.InventoryTransaction },
            { 0x029f, MatchaOpcode.ItemInfo },
            { 0x01f2, MatchaOpcode.MarketBoardItemListing },
            { 0x02a3, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02db, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x03dd, MatchaOpcode.PlayerSetup },
            { 0x0338, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0303, MatchaOpcode.ActorControlSelf },
            { 0x024e, MatchaOpcode.CEDirector },
            { 0x0381, MatchaOpcode.CompanyAirshipStatus },
            { 0x01df, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00c3, MatchaOpcode.ContentFinderNotifyPop },
            { 0x00c0, MatchaOpcode.DirectorStart },
            { 0x03a0, MatchaOpcode.EventPlay },
            { 0x0219, MatchaOpcode.Examine },
            { 0x0367, MatchaOpcode.InitZone },
            { 0x0182, MatchaOpcode.InventoryTransaction },
            { 0x0305, MatchaOpcode.ItemInfo },
            { 0x01a2, MatchaOpcode.MarketBoardItemListing },
            { 0x006c, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02fc, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x00ea, MatchaOpcode.PlayerSetup },
            { 0x00f1, MatchaOpcode.PlayerSpawn },
        };
    }
}
