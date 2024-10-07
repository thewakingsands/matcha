namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Linq;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Utils;

    internal class Client
    {
        public static PacketProcessor UniversalisProcessor = new PacketProcessor(Secret.UniversalisKey);

        private static bool Enabled => Config.Instance.Overlay.Universalis;
        private static object objLock = new object();

        public static void HandlePacket(MatchaOpcode opcode, Packet packet)
        {
            if (opcode == MatchaOpcode.PlayerSetup
                || opcode == MatchaOpcode.MarketBoardItemListingCount
                || opcode == MatchaOpcode.MarketBoardItemListing
                || opcode == MatchaOpcode.MarketBoardItemListingHistory)
            {
                lock (objLock)
                {
                    UniversalisProcessor.ProcessZonePacket(opcode, packet);
                }
            }
        }

        public static async void QueryItem(ushort worldId, uint itemId, Action<BaseDTO> fireEvent)
        {
            if (!Enabled || ParsePlugin.Instance == null)
            {
                return;
            }

            var items = await Api.ListByDC(worldId, itemId);
            if (items == null)
            {
                return;
            }

            foreach (var pair in items)
            {
                var server = pair.Key;
                if (server == worldId)
                {
                    continue;
                }

                fireEvent(new MarketBoardItemListingCountDTO()
                {
                    Item = (int)itemId,
                    World = server,
                    Count = pair.Value.Count
                });

                fireEvent(new MarketBoardItemListingDTO()
                {
                    Item = (int)itemId,
                    World = server,
                    Data = pair.Value.Select((row) => new MarketBoardItemListingItem()
                    {
                        Price = row.PricePerUint,
                        Quantity = row.Quantity,
                        HQ = row.Hq
                    })
                });
            }
        }
    }
}
