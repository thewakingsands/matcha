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
            { 0x03c7, MatchaOpcode.ActorControl },
            { 0x022f, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x0164, MatchaOpcode.CompanyAirshipStatus },
            { 0x0363, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01e5, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.DirectorStart },
            { 0x026f, MatchaOpcode.EventPlay },
            { 0x006a, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x0173, MatchaOpcode.InitZone },
            { 0x0197, MatchaOpcode.InventoryTransaction },
            { 0x02d0, MatchaOpcode.ItemInfo },
            { 0x03a3, MatchaOpcode.MarketBoardItemListing },
            { 0x0233, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02bf, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0359, MatchaOpcode.NpcSpawn },
            { 0x02a7, MatchaOpcode.PlayerSetup },
            { 0x0165, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0108, MatchaOpcode.ActorControl },
            { 0x0295, MatchaOpcode.ActorControlSelf },
            { 0x0363, MatchaOpcode.CEDirector },
            { 0x0370, MatchaOpcode.CompanyAirshipStatus },
            { 0x0069, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0326, MatchaOpcode.ContentFinderNotifyPop },
            { 0x029c, MatchaOpcode.DirectorStart },
            { 0x0353, MatchaOpcode.EventPlay },
            { 0x03ab, MatchaOpcode.Examine },
            { 0x01df, MatchaOpcode.FateInfo },
            { 0x0315, MatchaOpcode.InitZone },
            { 0x0286, MatchaOpcode.InventoryTransaction },
            { 0x0327, MatchaOpcode.ItemInfo },
            { 0x0081, MatchaOpcode.MarketBoardItemListing },
            { 0x020f, MatchaOpcode.MarketBoardItemListingCount },
            { 0x014d, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0276, MatchaOpcode.NpcSpawn },
            { 0x00b6, MatchaOpcode.PlayerSetup },
            { 0x0265, MatchaOpcode.PlayerSpawn },
        };
    }
}
