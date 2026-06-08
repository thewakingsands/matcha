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
        ResumeEventScene32,
        EventPlay,
        EventStart,
        Examine,
        FateInfo,
        InitZone,
        InventoryTransaction,
        ItemInfo,
        MarketBoardItemListing,
        MarketBoardItemListingCount,
        MarketBoardItemListingHistory,
        MarketBoardRequestItemListingInfo,
        NpcSpawn,
        PlayerSetup,
        PlayerSpawn,
        WorldVisitQueue,
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x01e8, MatchaOpcode.ActorControl },
            { 0x01c9, MatchaOpcode.ActorControlSelf },
            { 0x03da, MatchaOpcode.CEDirector },
            { 0x0081, MatchaOpcode.CompanyAirshipStatus },
            { 0x0068, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02f2, MatchaOpcode.ContentFinderNotifyPop },
            { 0x030c, MatchaOpcode.ResumeEventScene32 },
            { 0x007e, MatchaOpcode.EventPlay },
            { 0x0251, MatchaOpcode.EventStart },
            { 0x03a2, MatchaOpcode.Examine },
            { 0x03a9, MatchaOpcode.FateInfo },
            { 0x0096, MatchaOpcode.InitZone },
            { 0x028e, MatchaOpcode.InventoryTransaction },
            { 0x033e, MatchaOpcode.ItemInfo },
            { 0x0244, MatchaOpcode.MarketBoardItemListing },
            { 0x0287, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0113, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8292, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x026b, MatchaOpcode.NpcSpawn },
            { 0x023f, MatchaOpcode.PlayerSetup },
            { 0x0255, MatchaOpcode.PlayerSpawn },
            { 0x0066, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x01e8, MatchaOpcode.ActorControl },
            { 0x01c9, MatchaOpcode.ActorControlSelf },
            { 0x03da, MatchaOpcode.CEDirector },
            { 0x0081, MatchaOpcode.CompanyAirshipStatus },
            { 0x0068, MatchaOpcode.CompanySubmersibleStatus },
            { 0x02f2, MatchaOpcode.ContentFinderNotifyPop },
            { 0x030c, MatchaOpcode.ResumeEventScene32 },
            { 0x007e, MatchaOpcode.EventPlay },
            { 0x0251, MatchaOpcode.EventStart },
            { 0x03a2, MatchaOpcode.Examine },
            { 0x03a9, MatchaOpcode.FateInfo },
            { 0x0096, MatchaOpcode.InitZone },
            { 0x028e, MatchaOpcode.InventoryTransaction },
            { 0x033e, MatchaOpcode.ItemInfo },
            { 0x0244, MatchaOpcode.MarketBoardItemListing },
            { 0x0287, MatchaOpcode.MarketBoardItemListingCount },
            { 0x0113, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x8292, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x026b, MatchaOpcode.NpcSpawn },
            { 0x023f, MatchaOpcode.PlayerSetup },
            { 0x0255, MatchaOpcode.PlayerSpawn },
            { 0x0066, MatchaOpcode.WorldVisitQueue },
        };
    }
}
