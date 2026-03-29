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
            { 0x0311, MatchaOpcode.ActorControl },
            { 0x03c9, MatchaOpcode.ActorControlSelf },
            { 0xf002, MatchaOpcode.CEDirector },
            { 0x00c3, MatchaOpcode.CompanyAirshipStatus },
            { 0x007f, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01b4, MatchaOpcode.ContentFinderNotifyPop },
            { 0xf006, MatchaOpcode.ResumeEventScene32 },
            { 0x02fe, MatchaOpcode.EventPlay },
            { 0x0181, MatchaOpcode.EventStart },
            { 0x0271, MatchaOpcode.Examine },
            { 0xf00a, MatchaOpcode.FateInfo },
            { 0x02f2, MatchaOpcode.InitZone },
            { 0x02bb, MatchaOpcode.InventoryTransaction },
            { 0x0159, MatchaOpcode.ItemInfo },
            { 0x022d, MatchaOpcode.MarketBoardItemListing },
            { 0x006f, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00f2, MatchaOpcode.MarketBoardItemListingHistory },
            { 0xf011, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0328, MatchaOpcode.NpcSpawn },
            { 0x008e, MatchaOpcode.PlayerSetup },
            { 0x0109, MatchaOpcode.PlayerSpawn },
            { 0xf015, MatchaOpcode.WorldVisitQueue },
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
            { 0x0311, MatchaOpcode.ActorControl },
            { 0x03c9, MatchaOpcode.ActorControlSelf },
            { 0x0224, MatchaOpcode.CEDirector },
            { 0x00c3, MatchaOpcode.CompanyAirshipStatus },
            { 0x007f, MatchaOpcode.CompanySubmersibleStatus },
            { 0x01b4, MatchaOpcode.ContentFinderNotifyPop },
            { 0x0299, MatchaOpcode.ResumeEventScene32 },
            { 0x02fe, MatchaOpcode.EventPlay },
            { 0x0181, MatchaOpcode.EventStart },
            { 0x014d, MatchaOpcode.Examine },
            { 0x01b0, MatchaOpcode.FateInfo },
            { 0x02f2, MatchaOpcode.InitZone },
            { 0x02bb, MatchaOpcode.InventoryTransaction },
            { 0x0159, MatchaOpcode.ItemInfo },
            { 0x022d, MatchaOpcode.MarketBoardItemListing },
            { 0x006f, MatchaOpcode.MarketBoardItemListingCount },
            { 0x00f2, MatchaOpcode.MarketBoardItemListingHistory },
            { 0x82ee, MatchaOpcode.MarketBoardRequestItemListingInfo },
            { 0x0328, MatchaOpcode.NpcSpawn },
            { 0x008e, MatchaOpcode.PlayerSetup },
            { 0x0109, MatchaOpcode.PlayerSpawn },
            { 0x02ee, MatchaOpcode.WorldVisitQueue },
        };
    }
}
