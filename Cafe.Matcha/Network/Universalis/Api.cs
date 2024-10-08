namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Cafe.Matcha.Network.Structures;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    internal class Api
    {
        private const string ApiBase = "https://universalis.app";

        private readonly PacketProcessor _packetProcessor;
        private readonly string _apiKey;

        public Api(PacketProcessor packetProcessor, string apiKey)
        {
            _packetProcessor = packetProcessor;
            _apiKey = apiKey;
        }

        public async void Upload(uint worldId, MarketBoardItemRequest request)
        {
            _packetProcessor.Log?.Invoke(this, "Starting Universalis upload.");
            var uploader = _packetProcessor.LocalContentId;

            var uploadObject = new UniversalisItemUploadRequest
            {
                WorldId = worldId,
                UploaderId = uploader.ToString(),
                ItemId = request.Listings.FirstOrDefault()?.ItemId ?? 0,
                Listings = new List<UniversalisItemListingsEntry>(),
                Sales = new List<UniversalisHistoryEntry>(),
            };

            foreach (var marketBoardItemListing in request.Listings)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                var universalisListing = new UniversalisItemListingsEntry
                {
                    ListingId = marketBoardItemListing.ListingId.ToString(),
                    Hq = marketBoardItemListing.IsHq,
                    SellerId = marketBoardItemListing.RetainerOwnerId.ToString(),
                    RetainerName = marketBoardItemListing.RetainerName,
                    RetainerId = marketBoardItemListing.RetainerId.ToString(),
                    CreatorId = marketBoardItemListing.ArtisanId.ToString(),
                    CreatorName = marketBoardItemListing.PlayerName,
                    OnMannequin = marketBoardItemListing.OnMannequin,
                    LastReviewTime = ((DateTimeOffset)marketBoardItemListing.LastReviewTime).ToUnixTimeSeconds(),
                    PricePerUnit = marketBoardItemListing.PricePerUnit,
                    Quantity = marketBoardItemListing.ItemQuantity,
                    RetainerCity = marketBoardItemListing.RetainerCityId,
                    Materia = new List<UniversalisItemMateria>(),
                };
#pragma warning restore CS0618 // Type or member is obsolete

                foreach (var itemMateria in marketBoardItemListing.Materia)
                {
                    universalisListing.Materia.Add(new UniversalisItemMateria
                    {
                        MateriaId = itemMateria.MateriaId,
                        SlotId = itemMateria.Index,
                    });
                }

                uploadObject.Listings.Add(universalisListing);
            }

            foreach (var marketBoardHistoryListing in request.History)
            {
                uploadObject.Sales.Add(new UniversalisHistoryEntry
                {
                    BuyerName = marketBoardHistoryListing.BuyerName,
                    Hq = marketBoardHistoryListing.IsHq,
                    OnMannequin = marketBoardHistoryListing.OnMannequin,
                    PricePerUnit = marketBoardHistoryListing.SalePrice,
                    Quantity = marketBoardHistoryListing.Quantity,
                    Timestamp = ((DateTimeOffset)marketBoardHistoryListing.PurchaseTime).ToUnixTimeSeconds(),
                });
            }

            var uploadPath = "/upload";
            await Request.SendAsJson($"{ApiBase}{uploadPath}/{_apiKey}", "", uploadObject);

            _packetProcessor.Log?.Invoke(this, $"Universalis data upload for item#{request.Listings.FirstOrDefault()?.CatalogId ?? 0} completed");
        }

        public static async Task<Dictionary<int, List<UniversalisItem>>> ListByDC(ushort worldId, uint itemId)
        {
            var hasWorld = Data.Instance.Worlds.TryGetValue(worldId, out var world);
            if (!hasWorld)
            {
                return null;
            }

            var url = $"{ApiBase}/api/{world.EnglishDataCenter}/{itemId}";
            var res = await Request.Send(url);

            try
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UniversalisQueryResponse>(content);
                var ret = new Dictionary<int, List<UniversalisItem>>();

                foreach (var item in data.ListingItems)
                {
                    var itemWorld = Data.Instance.Worlds
                        .FirstOrDefault(row => item.WorldName == row.Value.LocalName || item.WorldName == row.Value.EnglishName);

                    if (itemWorld.Key == 0)
                    {
                        continue;
                    }

                    var hasWorldResult = ret.TryGetValue(itemWorld.Key, out var worldList);
                    if (!hasWorldResult)
                    {
                        worldList = new List<UniversalisItem>();
                        ret.Add(itemWorld.Key, worldList);
                    }

                    worldList.Add(item);
                }

                return ret;
            }
            catch (Exception e)
            {
                Log.Error(Constant.LogType.Universalis, $"获取物价信息失败 {e.Message}");
#if DEBUG
                Log.Debug(Constant.LogType.Universalis, e.StackTrace);
#endif
            }

            return null;
        }
    }
}
