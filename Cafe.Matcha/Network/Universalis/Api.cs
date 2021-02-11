namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
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

        public async void Upload(MarketBoardItemRequest request)
        {
            using (var client = new WebClient())
            {
                _packetProcessor.Log?.Invoke(this, "Starting Universalis upload.");
                var uploader = _packetProcessor.LocalContentId;

                var listingsRequestObject = new UniversalisItemListingsUploadRequest();
                listingsRequestObject.WorldId = (int)_packetProcessor.CurrentWorldId;
                listingsRequestObject.UploaderId = uploader;
                listingsRequestObject.ItemId = request.CatalogId;

                listingsRequestObject.Listings = new List<UniversalisItemListingsEntry>();
                foreach (var marketBoardItemListing in request.Listings)
                {
                    var universalisListing = new UniversalisItemListingsEntry
                    {
                        Hq = marketBoardItemListing.IsHq,
                        SellerId = marketBoardItemListing.RetainerOwnerId,
                        RetainerName = marketBoardItemListing.RetainerName,
                        RetainerId = marketBoardItemListing.RetainerId,
                        CreatorId = marketBoardItemListing.ArtisanId,
                        CreatorName = marketBoardItemListing.PlayerName,
                        OnMannequin = marketBoardItemListing.OnMannequin,
                        LastReviewTime = ((DateTimeOffset)marketBoardItemListing.LastReviewTime).ToUnixTimeSeconds(),
                        PricePerUnit = marketBoardItemListing.PricePerUnit,
                        Quantity = marketBoardItemListing.ItemQuantity,
                        RetainerCity = marketBoardItemListing.RetainerCityId
                    };

                    universalisListing.Materia = new List<UniversalisItemMateria>();
                    foreach (var itemMateria in marketBoardItemListing.Materia)
                    {
                        universalisListing.Materia.Add(new UniversalisItemMateria
                        {
                            MateriaId = itemMateria.MateriaId,
                            SlotId = itemMateria.Index
                        });
                    }

                    listingsRequestObject.Listings.Add(universalisListing);
                }

                await Request.SendAsJson($"{ApiBase}/upload/{_apiKey}", "", listingsRequestObject);

                var historyRequestObject = new UniversalisHistoryUploadRequest();
                historyRequestObject.WorldId = (int)_packetProcessor.CurrentWorldId;
                historyRequestObject.UploaderId = uploader;
                historyRequestObject.ItemId = request.CatalogId;

                historyRequestObject.Entries = new List<UniversalisHistoryEntry>();
                foreach (var marketBoardHistoryListing in request.History)
                {
                    historyRequestObject.Entries.Add(new UniversalisHistoryEntry
                    {
                        BuyerName = marketBoardHistoryListing.BuyerName,
                        Hq = marketBoardHistoryListing.IsHq,
                        OnMannequin = marketBoardHistoryListing.OnMannequin,
                        PricePerUnit = marketBoardHistoryListing.SalePrice,
                        Quantity = marketBoardHistoryListing.Quantity,
                        Timestamp = ((DateTimeOffset)marketBoardHistoryListing.PurchaseTime).ToUnixTimeSeconds()
                    });
                }

                await Request.SendAsJson($"{ApiBase}/upload/{_apiKey}", "", historyRequestObject);

                _packetProcessor.Log?.Invoke(this,
                    $"Universalis data upload for item#{request.CatalogId} to world#{historyRequestObject.WorldId} completed.");
            }
        }

        public static async Task<Dictionary<int, List<UniversalisItem>>> ListByDC(int worldId, uint itemId)
        {
            var hasWorld = Data.Instance.Worlds.TryGetValue(worldId, out var world);
            if (!hasWorld)
            {
                return null;
            }

            var url = $"{ApiBase}/api/{world.DataCenterEnglish}/{itemId}";
            var res = await Request.Send(url);

            try
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UniversalisQueryResponse>(content);
                var ret = new Dictionary<int, List<UniversalisItem>>();

                foreach (var item in data.ListingItems)
                {
                    var itemWorld = Data.Instance.Worlds
                        .FirstOrDefault(row => item.WorldName == row.Value.Chinese || item.WorldName == row.Value.English);

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
                Log.Error($"[Universalis] 获取物价信息失败 {e.Message}");
            }

            return null;
        }
    }
}