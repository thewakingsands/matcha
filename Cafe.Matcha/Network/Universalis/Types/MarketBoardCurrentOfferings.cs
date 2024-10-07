namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal class MarketBoardCurrentOfferings
    {
        public class MarketBoardItemListing
        {
            public ulong ListingId;
            public ulong RetainerId;
            public ulong RetainerOwnerId;
            public ulong ArtisanId;
            public uint PricePerUnit;
            public uint TotalTax;
            public uint ItemQuantity;
            public uint CatalogId;
            public DateTime LastReviewTime;

            public class ItemMateria
            {
                public int MateriaId;
                public int Index;
            }

            public List<ItemMateria> Materia;

            public string RetainerName;
            public string PlayerName;
            public bool IsHq;
            public int MateriaCount;
            public bool OnMannequin;
            public int RetainerCityId;
            public int StainId;
        }

        public List<MarketBoardItemListing> ItemListings;

        public int ListingIndexEnd;
        public int ListingIndexStart;
        public int RequestId;

        public static MarketBoardCurrentOfferings Read(byte[] message)
        {
            var output = new MarketBoardCurrentOfferings();

            using (var stream = new MemoryStream(message))
            {
                using (var reader = new BinaryReader(stream))
                {
                    output.ItemListings = new List<MarketBoardItemListing>();

                    for (var i = 0; i < 10; i++)
                    {
                        var listingEntry = new MarketBoardItemListing();

                        listingEntry.ListingId = reader.ReadUInt64();
                        listingEntry.RetainerId = reader.ReadUInt64();
                        listingEntry.RetainerOwnerId = reader.ReadUInt64();
                        listingEntry.ArtisanId = reader.ReadUInt64();
                        listingEntry.PricePerUnit = reader.ReadUInt32();
                        listingEntry.TotalTax = reader.ReadUInt32();
                        listingEntry.ItemQuantity = reader.ReadUInt32();
                        listingEntry.CatalogId = reader.ReadUInt32();
                        listingEntry.LastReviewTime = DateTimeOffset.UtcNow.AddSeconds(-reader.ReadUInt16()).DateTime;

                        reader.ReadUInt16(); // container
                        reader.ReadUInt32(); // slot
                        reader.ReadUInt16(); // durability
                        reader.ReadUInt16(); // spiritbond

                        listingEntry.Materia = new List<MarketBoardItemListing.ItemMateria>();

                        for (var materiaIndex = 0; materiaIndex < 5; materiaIndex++)
                        {
                            var materiaVal = reader.ReadUInt16();

                            var materiaEntry = new MarketBoardItemListing.ItemMateria();
                            materiaEntry.MateriaId = (materiaVal & 0xFF0) >> 4;
                            materiaEntry.Index = materiaVal & 0xF;

                            if (materiaEntry.MateriaId != 0)
                            {
                                listingEntry.Materia.Add(materiaEntry);
                            }
                        }

                        reader.ReadUInt16();
                        reader.ReadUInt32();

                        listingEntry.RetainerName = Encoding.UTF8.GetString(reader.ReadBytes(32)).TrimEnd(new[] { '\u0000' });
                        listingEntry.PlayerName = Encoding.UTF8.GetString(reader.ReadBytes(32)).TrimEnd(new[] { '\u0000' });
                        listingEntry.IsHq = reader.ReadBoolean();
                        listingEntry.MateriaCount = reader.ReadByte();
                        listingEntry.OnMannequin = reader.ReadBoolean();
                        listingEntry.RetainerCityId = reader.ReadByte();
                        listingEntry.StainId = reader.ReadUInt16();

                        reader.ReadUInt16();
                        reader.ReadUInt32();

                        if (listingEntry.CatalogId != 0)
                        {
                            output.ItemListings.Add(listingEntry);
                        }
                    }

                    output.ListingIndexEnd = reader.ReadByte();
                    output.ListingIndexStart = reader.ReadByte();
                    output.RequestId = reader.ReadUInt16();
                }
            }

            return output;
        }
    }
}
