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
            { 0x02d3, MatchaOpcode.ActorControl },
            { 0x029b, MatchaOpcode.ActorControlSelf },
            { 0x018c, MatchaOpcode.CEDirector },
            { 0x0221, MatchaOpcode.CompanyAirshipStatus },
            { 0x008c, MatchaOpcode.CompanySubmersibleStatus },
            { 0x03d5, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0256, MatchaOpcode.DirectorStart },
            { 0x00c9, MatchaOpcode.EventPlay },
            { 0x027c, MatchaOpcode.Examine },
            { 0xf009, MatchaOpcode.FateInfo },
            { 0x03e5, MatchaOpcode.InitZone },
            { 0x0381, MatchaOpcode.InventoryTransaction },
            { 0x014d, MatchaOpcode.ItemInfo },
            { 0x0066, MatchaOpcode.MarketBoardItemListing },
            { 0x0241, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00bc, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x03cd, MatchaOpcode.NpcSpawn },
            { 0x0178, MatchaOpcode.PlayerSetup },
            { 0x02c2, MatchaOpcode.PlayerSpawn },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x00e4, MatchaOpcode.ActorControl },
            { 0x0125, MatchaOpcode.ActorControlSelf },
            { 0x019a, MatchaOpcode.CEDirector },
            { 0x01a5, MatchaOpcode.CompanyAirshipStatus },
            { 0x0112, MatchaOpcode.CompanySubmersibleStatus },
            { 0x021b, MatchaOpcode.ContentFinderNotifyPop },
            { 0x017b, MatchaOpcode.DirectorStart },
            { 0x0101, MatchaOpcode.EventPlay },
            { 0x0382, MatchaOpcode.Examine },
            { 0x007a, MatchaOpcode.FateInfo },
            { 0x026f, MatchaOpcode.InitZone },
            { 0x01b9, MatchaOpcode.InventoryTransaction },
            { 0x0255, MatchaOpcode.ItemInfo },
            { 0x0168, MatchaOpcode.MarketBoardItemListing },
            { 0x01ac, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0254, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x0349, MatchaOpcode.NpcSpawn },
            { 0x036d, MatchaOpcode.PlayerSetup },
            { 0x0220, MatchaOpcode.PlayerSpawn },
        };
    }
}
