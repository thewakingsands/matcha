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
            { 0x0187, MatchaOpcode.ActorControl },
            { 0x0141, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x021f, MatchaOpcode.CompanyAirshipStatus },
            { 0x017d, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0181, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.DirectorStart },
            { 0x020e, MatchaOpcode.EventPlay },
            { 0x028a, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x03dd, MatchaOpcode.InitZone },
            { 0x008f, MatchaOpcode.InventoryTransaction },
            { 0x0305, MatchaOpcode.ItemInfo },
            { 0x00fc, MatchaOpcode.MarketBoardItemListing },
            { 0x01d7, MatchaOpcode.MarketBoardItemListingCount },
            { 0x022b, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0300, MatchaOpcode.NpcSpawn },
            { 0x0121, MatchaOpcode.PlayerSetup },
            { 0x0338, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x019b, MatchaOpcode.ActorControl },
            { 0x033f, MatchaOpcode.ActorControlSelf },
            { 0x00af, MatchaOpcode.CEDirector },
            { 0x0398, MatchaOpcode.CompanyAirshipStatus },
            { 0x0377, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02c7, MatchaOpcode.ContentFinderNotifyPop },
            { 0x01b0, MatchaOpcode.DirectorStart },
            { 0x0230, MatchaOpcode.EventPlay },
            { 0x0233, MatchaOpcode.Examine },
            { 0x01b9, MatchaOpcode.FateInfo },
            { 0x00e1, MatchaOpcode.InitZone },
            { 0x02b9, MatchaOpcode.InventoryTransaction },
            { 0x037f, MatchaOpcode.ItemInfo },
            { 0x0079, MatchaOpcode.MarketBoardItemListing },
            { 0x032e, MatchaOpcode.MarketBoardItemListingCount },
            { 0x012c, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0261, MatchaOpcode.NpcSpawn },
            { 0x01f3, MatchaOpcode.PlayerSetup },
            { 0x0301, MatchaOpcode.PlayerSpawn },
        };
    }
}
