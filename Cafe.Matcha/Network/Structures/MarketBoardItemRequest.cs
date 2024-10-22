// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Structures
{
    using System.Collections.Generic;
    using System.IO;

    internal class MarketBoardItemRequest
    {
        /// <summary>
        /// Gets the request status. Nonzero statuses are errors.
        /// Known values: default=0; rate limited=0x70000003.
        /// </summary>
        public uint Status { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not this request was successful.
        /// </summary>
        public bool Ok => this.Status == 0;

        /// <summary>
        /// Gets the amount to arrive.
        /// </summary>
        public uint AmountToArrive { get; private set; }

        /// <summary>
        /// Gets the offered item listings.
        /// </summary>
        public List<MarketBoardCurrentOfferings.MarketBoardItemListing> Listings { get; } = new List<MarketBoardCurrentOfferings.MarketBoardItemListing>();

        /// <summary>
        /// Gets the historical item listings.
        /// </summary>
        public List<MarketBoardHistory.MarketBoardHistoryListing> History { get; } = new List<MarketBoardHistory.MarketBoardHistoryListing>();

        /// <summary>
        /// Read a packet off the wire.
        /// </summary>
        /// <param name="data">Packet data (without header).</param>
        /// <returns>An object representing the data read.</returns>
        public static MarketBoardItemRequest Read(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var output = new MarketBoardItemRequest
                    {
                        Status = reader.ReadUInt32(),
                        AmountToArrive = reader.ReadUInt32(),
                    };

                    return output;
                }
            }
        }
    }
}
