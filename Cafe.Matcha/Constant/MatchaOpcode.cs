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
            { 0x01a4, MatchaOpcode.ActorControl },
            { 0x0203, MatchaOpcode.ActorControlSelf },
            { 0x0070, MatchaOpcode.CEDirector },
            { 0x00da, MatchaOpcode.CompanyAirshipStatus },
            { 0x0263, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02a1, MatchaOpcode.ContentFinderNotifyPop },
            { 0x009d, MatchaOpcode.DirectorStart },
            { 0x03b8, MatchaOpcode.EventPlay },
            { 0x0246, MatchaOpcode.Examine },
            { 0x0078, MatchaOpcode.FateInfo },
            { 0x0118, MatchaOpcode.InitZone },
            { 0x006e, MatchaOpcode.InventoryTransaction },
            { 0x01c2, MatchaOpcode.ItemInfo },
            { 0x01ed, MatchaOpcode.MarketBoardItemListing },
            { 0x031a, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0176, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x03d5, MatchaOpcode.NpcSpawn },
            { 0x0287, MatchaOpcode.PlayerSetup },
            { 0x00f9, MatchaOpcode.PlayerSpawn },
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
