namespace Cafe.Matcha.Network.Structures
{
    using System.IO;

    /// <summary>
    /// Represents market board purchase information. This message is received from the
    /// server when a purchase is made at a market board.
    /// </summary>
    public class MarketBoardPurchase : IMarketBoardPurchase
    {
        private MarketBoardPurchase()
        {
        }

        /// <summary>
        /// Gets the item ID of the item that was purchased.
        /// </summary>
        public uint CatalogId { get; private set; }

        /// <summary>
        /// Gets the quantity of the item that was purchased.
        /// </summary>
        public uint ItemQuantity { get; private set; }

        /// <summary>
        /// Reads market board purchase information from the struct at the provided pointer.
        /// </summary>
        /// <param name="data">Data to read.</param>
        /// <returns>An object representing the data read.</returns>
        public static MarketBoardPurchase Read(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var output = new MarketBoardPurchase();

                    output.CatalogId = reader.ReadUInt32();
                    reader.ReadBytes(0x4); // Padding
                    output.ItemQuantity = reader.ReadUInt32();

                    return output;
                }
            }
        }
    }
}
