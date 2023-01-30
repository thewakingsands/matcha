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
            { 0x0363, MatchaOpcode.ActorControl },
            { 0x0267, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x00ad, MatchaOpcode.CompanyAirshipStatus },
            { 0x009d, MatchaOpcode.CompanySubmersibleStatus },
            { 0x03a0, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x01f5, MatchaOpcode.EventPlay },
            { 0x0121, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x0094, MatchaOpcode.InitZone },
            { 0x00d3, MatchaOpcode.InventoryTransaction },
            { 0x0335, MatchaOpcode.ItemInfo },
            { 0x0155, MatchaOpcode.MarketBoardItemListing },
            { 0x03bf, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0296, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0391, MatchaOpcode.NpcSpawn },
            { 0x0373, MatchaOpcode.PlayerSetup },
            { 0x0187, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0249, MatchaOpcode.ActorControl },
            { 0x0397, MatchaOpcode.ActorControlSelf },
            { 0x00e4, MatchaOpcode.CEDirector },
            { 0x0180, MatchaOpcode.CompanyAirshipStatus },
            { 0x0082, MatchaOpcode.CompanySubmersibleStatus },
            { 0x00cf, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0130, MatchaOpcode.DirectorStart },
            { 0x02be, MatchaOpcode.EventPlay },
            { 0x006d, MatchaOpcode.Examine },
            { 0x0208, MatchaOpcode.FateInfo },
            { 0x008b, MatchaOpcode.InitZone },
            { 0x02f1, MatchaOpcode.InventoryTransaction },
            { 0x0398, MatchaOpcode.ItemInfo },
            { 0x022b, MatchaOpcode.MarketBoardItemListing },
            { 0x03e4, MatchaOpcode.MarketBoardItemListingCount },
            { 0x03bc, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x00e7, MatchaOpcode.NpcSpawn },
            { 0x0178, MatchaOpcode.PlayerSetup },
            { 0x029f, MatchaOpcode.PlayerSpawn },
        };
    }
}
