namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal class MarketBoardHistory
    {
        public uint CatalogId;
        public uint CatalogId2;

        public class MarketBoardHistoryListing
        {
            public uint SalePrice;
            public DateTime PurchaseTime;
            public uint Quantity;
            public bool IsHq;
            public bool OnMannequin;

            public string BuyerName;

            public uint CatalogId;
        }

        public List<MarketBoardHistoryListing> HistoryListings;

        public static MarketBoardHistory Read(byte[] message)
        {
            var output = new MarketBoardHistory();

            using (var stream = new MemoryStream(message))
            {
                using (var reader = new BinaryReader(stream))
                {
                    output.CatalogId = reader.ReadUInt32();
                    output.CatalogId2 = reader.ReadUInt32();

                    output.HistoryListings = new List<MarketBoardHistoryListing>();

                    for (var i = 0; i < 10; i++)
                    {
                        var listingEntry = new MarketBoardHistoryListing();

                        listingEntry.SalePrice = reader.ReadUInt32();
                        listingEntry.PurchaseTime = DateTimeOffset.FromUnixTimeSeconds(reader.ReadUInt32()).UtcDateTime;
                        listingEntry.Quantity = reader.ReadUInt32();
                        listingEntry.IsHq = reader.ReadBoolean();

                        reader.ReadBoolean();

                        listingEntry.OnMannequin = reader.ReadBoolean();
                        listingEntry.BuyerName = Encoding.UTF8.GetString(reader.ReadBytes(33)).TrimEnd(new[] { '\u0000' });
                        listingEntry.CatalogId = reader.ReadUInt32();

                        if (listingEntry.CatalogId != 0)
                        {
                            output.HistoryListings.Add(listingEntry);
                        }
                    }
                }
            }

            return output;
        }
    }
}
