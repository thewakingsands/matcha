// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Structures
{
    using System.IO;
    using System.Windows.Markup;

    /// <summary>
    /// Represents market board purchase information. This message is sent from the
    /// client when a purchase is made at a market board.
    /// </summary>
    public class MarketBoardPurchaseHandler : IMarketBoardPurchaseHandler
    {
        private MarketBoardPurchaseHandler()
        {
        }

        /// <summary>
        /// Gets the object ID of the retainer associated with the sale.
        /// </summary>
        public ulong RetainerId { get; private set; }

        /// <summary>
        /// Gets the object ID of the item listing.
        /// </summary>
        public ulong ListingId { get; private set; }

        /// <summary>
        /// Gets the item ID of the item that was purchased.
        /// </summary>
        public uint CatalogId { get; private set; }

        /// <summary>
        /// Gets the quantity of the item that was purchased.
        /// </summary>
        public uint ItemQuantity { get; private set; }

        /// <summary>
        /// Gets the unit price of the item.
        /// </summary>
        public uint PricePerUnit { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the item is HQ.
        /// </summary>
        public bool IsHq { get; private set; }

        /// <summary>
        /// Gets the total tax.
        /// </summary>
        public uint TotalTax { get; private set; }

        /// <summary>
        /// Gets the city ID of the retainer selling the item.
        /// </summary>
        public int RetainerCityId { get; private set; }

        /// <summary>
        /// Reads market board purchase information from the struct at the provided pointer.
        /// </summary>
        /// <param name="data">Data to read.</param>
        /// <returns>An object representing the data read.</returns>
        public static MarketBoardPurchaseHandler Read(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var output = new MarketBoardPurchaseHandler();

                    output.RetainerId = reader.ReadUInt64();
                    output.ListingId = reader.ReadUInt64();

                    output.CatalogId = reader.ReadUInt32();
                    output.ItemQuantity = reader.ReadUInt32();
                    output.PricePerUnit = reader.ReadUInt32();
                    output.TotalTax = reader.ReadUInt32();

                    reader.ReadUInt16(); // Slot

                    output.IsHq = reader.ReadBoolean();
                    output.RetainerCityId = reader.ReadByte();

                    return output;
                }
            }
        }
    }
}
