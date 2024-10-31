namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Linq;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Network.Handler;
    using Cafe.Matcha.Utils;

    internal class Client : AbstractHandler
    {
        public PacketProcessor UniversalisProcessor = new PacketProcessor(Secret.UniversalisKey);

        private bool Enabled => Config.Instance.Overlay.Universalis;
        private object objLock = new object();

        public Client(Action<BaseDTO> fireEvent) : base(fireEvent)
        {
        }

        public override bool Handle(Packet packet)
        {
            var opcode = packet.MatchaOpcode;
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

            return false;
        }

        public async void QueryItem(ushort worldId, uint itemId)
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
