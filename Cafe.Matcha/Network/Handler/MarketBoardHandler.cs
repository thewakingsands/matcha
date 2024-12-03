// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Network.Structures;

    internal class MarketBoardHandler : AbstractHandler
    {
        /// <summary>
        /// ItemId sent in the last MarketBoard query.
        /// </summary>
        private uint queryItemId = 0;

        /// <summary>
        /// Is next incoming MarketBoardItemListing packet the first page.
        /// </summary>
        private bool isFirstPageIncoming = false;

        private Universalis.Client universalis;

        public MarketBoardHandler(Action<BaseDTO> fireEvent) : base(fireEvent)
        {
            universalis = new Universalis.Client(fireEvent);

#if DEBUG
            universalis.UniversalisProcessor.Log += OnUniversalisLog;
#endif
        }

#if DEBUG
        private void OnUniversalisLog(object sender, string e)
        {
            Utils.Log.Info(LogType.Universalis, e);
        }
#endif

        public override bool Handle(Packet packet)
        {
            universalis.Handle(packet);

            var opcode = packet.MatchaOpcode;

            // Required by Universalis but not us.
            if (opcode == MatchaOpcode.MarketBoardItemListingHistory)
            {
                return true;
            }

            // The client packet containing itemId. Deucalion Required.
            if (opcode == MatchaOpcode.MarketBoardRequestItemListingInfo)
            {
                if (packet.DataLength != 8)
                {
                    return false;
                }

                var itemId = packet.ReadUInt32(0);
                if (itemId != 0)
                {
                    InitMarketBoardListing(itemId);
                    queryItemId = itemId;
                }

                return true;
            }

            // Indicates a new group of MarketBoardItemListing
            if (opcode == MatchaOpcode.MarketBoardItemListingCount)
            {
                isFirstPageIncoming = true;
                return true;
            }

            // Market listing response
            if (opcode == MatchaOpcode.MarketBoardItemListing)
            {
                var result = MarketBoardCurrentOfferings.Read(packet.GetRawData());
                var items = new List<MarketBoardItemListingItem>();

                uint itemId = 0;
                foreach (var item in result.ItemListings)
                {
                    if (item.PricePerUnit == 0)
                    {
                        break;
                    }

                    itemId = item.ItemId;
                    items.Add(new MarketBoardItemListingItem()
                    {
                        // Price = (int)(pricePerUnit * 1.05),
                        Price = (int)item.PricePerUnit,
                        Quantity = (int)item.ItemQuantity,
                        HQ = item.IsHq
                    });
                }

                if (itemId == 0)
                {
                    return true;
                }

                // Call `MarketBoardRequestItemListingInfo` only when
                // 1.`queryItemId == 0`, which means client packet `MarketBoardRequestItemListingInfo` is not correctly handled.
                // 2. This is the first listing page (otherwise it have already been called)

                if (isFirstPageIncoming && queryItemId == 0)
                {
                    InitMarketBoardListing(itemId);
                }

                fireEvent(new MarketBoardItemListingDTO()
                {
                    Item = (int)itemId,
                    Data = items,
                    World = State.Instance.WorldId
                });

                // Always reset isFirstPageIncoming and queryItemId
                isFirstPageIncoming = false;
                queryItemId = 0;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Init (reset overlay status) for specified itemId.
        /// </summary>
        /// <param name="itemId">ItemId</param>
        private void InitMarketBoardListing(uint itemId)
        {
            fireEvent(new MarketBoardItemListingCountDTO()
            {
                Item = (int)itemId,
                World = State.Instance.WorldId
            });
            ThreadPool.QueueUserWorkItem(o => universalis.QueryItem(State.Instance.WorldId, itemId));
            queryItemId = itemId;
        }
    }
}
