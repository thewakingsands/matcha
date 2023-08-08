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
            { 0x00d4, MatchaOpcode.ActorControl },
            { 0x03c1, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x034f, MatchaOpcode.CompanyAirshipStatus },
            { 0x03af, MatchaOpcode.CompanySubmersibleStatus },
            { 0x0069, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.DirectorStart },
            { 0x02db, MatchaOpcode.EventPlay },
            { 0x0200, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x0071, MatchaOpcode.InitZone },
            { 0x03db, MatchaOpcode.InventoryTransaction },
            { 0x03a4, MatchaOpcode.ItemInfo },
            { 0x01db, MatchaOpcode.MarketBoardItemListing },
            { 0x0306, MatchaOpcode.MarketBoardItemListingCount },
            { 0x02f4, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0091, MatchaOpcode.NpcSpawn },
            { 0x020e, MatchaOpcode.PlayerSetup },
            { 0x010e, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0365, MatchaOpcode.ActorControl },
            { 0x0223, MatchaOpcode.ActorControlSelf },
            { 0x0068, MatchaOpcode.CEDirector },
            { 0x0268, MatchaOpcode.CompanyAirshipStatus },
            { 0x0277, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01eb, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0251, MatchaOpcode.DirectorStart },
            { 0x03ce, MatchaOpcode.EventPlay },
            { 0x0318, MatchaOpcode.Examine },
            { 0x02b0, MatchaOpcode.FateInfo },
            { 0x018e, MatchaOpcode.InitZone },
            { 0x03d9, MatchaOpcode.InventoryTransaction },
            { 0x036b, MatchaOpcode.ItemInfo },
            { 0x01b0, MatchaOpcode.MarketBoardItemListing },
            { 0x0246, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0130, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0388, MatchaOpcode.NpcSpawn },
            { 0x01ff, MatchaOpcode.PlayerSetup },
            { 0x0367, MatchaOpcode.PlayerSpawn },
        };
    }
}
