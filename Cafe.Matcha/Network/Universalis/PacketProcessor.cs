namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.Utils;

    internal class PacketProcessor
    {
        private readonly List<MarketBoardItemRequest> _marketBoardRequests = new List<MarketBoardItemRequest>();
        private readonly Api _uploader;

        public uint CurrentWorldId
        {
            get
            {
                return ParsePlugin.Instance.GetServer();
            }
        }

        public ulong LocalContentId { get; set; }

        public EventHandler<string> Log;
        public EventHandler<ulong> LocalContentIdUpdated;
        public EventHandler RequestContentIdUpdate;

        public PacketProcessor(string apiKey)
        {
            _uploader = new Api(this, apiKey);
        }

        /// <summary>
        /// Process a zone proto message and scan for relevant data.
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        /// <param name="message">The message bytes.</param>
        /// <returns>True if an upload succeeded.</returns>
        public bool ProcessZonePacket(MatchaOpcode opcode, byte[] message)
        {
            MarketBoardItemRequest request;
            lock (_marketBoardRequests)
            {
                request = GetRequestData(opcode, message);
                if (request != null)
                {
                    _marketBoardRequests.Remove(request);
                }
            }

            if (request != null)
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        _uploader.Upload(request);
                    }
                    catch (Exception ex)
                    {
                        Log?.Invoke(this, "[ERROR] Market Board data upload failed:\n" + ex);
                    }
                });
                return true;
            }

            return false;
        }

        /// <summary>
        /// Process a zone proto message and scan for relevant data.
        /// </summary>
        /// <param name="message">The message bytes.</param>
        /// <returns>True if an upload succeeded.</returns>
        private MarketBoardItemRequest GetRequestData(MatchaOpcode opcode, byte[] message)
        {
            if (opcode == MatchaOpcode.PlayerSetup)
            {
                LocalContentId = BitConverter.ToUInt64(message, 0x20);
                LocalContentId = LocalContentId & 0xffffffff00000000 | (uint)Config.Instance.UUIDHash; // mask the lower 32 bit for privacy concern
                Log?.Invoke(this, $"New CID: {LocalContentId.ToString("X")}");
                LocalContentIdUpdated?.Invoke(this, LocalContentId);
                return null;
            }

            if (opcode == MatchaOpcode.MarketBoardItemListingCount)
            {
                var catalogId = (uint)BitConverter.ToInt32(message, 0x20);
                var amount = message[0x2B];

                var request = _marketBoardRequests.LastOrDefault(r => r.CatalogId == catalogId);
                if (request == null)
                {
                    _marketBoardRequests.Add(new MarketBoardItemRequest
                    {
                        CatalogId = catalogId,
                        AmountToArrive = amount,
                        Listings = new List<MarketBoardCurrentOfferings.MarketBoardItemListing>(),
                        History = new List<MarketBoardHistory.MarketBoardHistoryListing>()
                    });
                }
                else
                {
                    request.AmountToArrive = amount;
                    request.Listings.Clear();
                }

                Log?.Invoke(this, $"NEW MB REQUEST START: item#{catalogId} amount#{amount}");
                return null;
            }

            if (opcode == MatchaOpcode.MarketBoardItemListing)
            {
                var listing = MarketBoardCurrentOfferings.Read(message.Skip(0x20).ToArray());

                var request =
                    _marketBoardRequests.LastOrDefault(
                        r => r.CatalogId == listing.ItemListings[0].CatalogId && !r.IsDone);

                if (request == null)
                {
                    Log?.Invoke(this,
                        $"[ERROR] Market Board data arrived without a corresponding request: item#{listing.ItemListings[0].CatalogId}");
                    return null;
                }

                if (request.Listings.Count + listing.ItemListings.Count > request.AmountToArrive)
                {
                    Log?.Invoke(this,
                        $"[ERROR] Too many Market Board listings received for request: {request.Listings.Count + listing.ItemListings.Count} > {request.AmountToArrive} item#{listing.ItemListings[0].CatalogId}");
                    _marketBoardRequests.Remove(request);
                    return null;
                }

                if (request.ListingsRequestId != -1 && request.ListingsRequestId != listing.RequestId)
                {
                    Log?.Invoke(this,
                        $"[ERROR] Non-matching RequestIds for Market Board data request: {request.ListingsRequestId}, {listing.RequestId}");
                    _marketBoardRequests.Remove(request);
                    return null;
                }

                if (request.ListingsRequestId == -1 && request.Listings.Count > 0)
                {
                    Log?.Invoke(this,
                        $"[ERROR] Market Board data request sequence break: {request.ListingsRequestId}, {request.Listings.Count}");
                    _marketBoardRequests.Remove(request);
                    return null;
                }

                if (request.ListingsRequestId == -1)
                {
                    request.ListingsRequestId = listing.RequestId;
                    Log?.Invoke(this, $"First Market Board packet in sequence: {listing.RequestId}");
                }

                request.Listings.AddRange(listing.ItemListings);

                Log?.Invoke(this,
                    $"Added {listing.ItemListings.Count} ItemListings to request#{request.ListingsRequestId}, now {request.Listings.Count}/{request.AmountToArrive}, item#{request.CatalogId}");

                if (request.IsDone)
                {
                    if (CurrentWorldId == 0)
                    {
                        Log?.Invoke(this, "[ERROR] Not sure about your current world. Please move your character between zones once to start uploading.");
                        _marketBoardRequests.Remove(request);
                        return null;
                    }

                    RequestContentIdUpdate?.Invoke(this, null);

                    if (LocalContentId == 0)
                    {
                        Log?.Invoke(this, "Not sure about your character information. Please log in once with your character while having the program open to verify it.");
                    }

                    LocalContentIdUpdated?.Invoke(this, LocalContentId);

                    Log?.Invoke(this,
                        $"Market Board request finished, starting upload: request#{request.ListingsRequestId} item#{request.CatalogId} amount#{request.AmountToArrive}");
                    return request;
                }

                return null;
            }

            if (opcode == MatchaOpcode.MarketBoardItemListingHistory)
            {
                var listing = MarketBoardHistory.Read(message.Skip(0x20).ToArray());

                var request = _marketBoardRequests.LastOrDefault(r => r.CatalogId == listing.CatalogId);

                if (request == null)
                {
                    request = new MarketBoardItemRequest
                    {
                        CatalogId = listing.CatalogId,
                        AmountToArrive = 0,
                        Listings = new List<MarketBoardCurrentOfferings.MarketBoardItemListing>(),
                        History = new List<MarketBoardHistory.MarketBoardHistoryListing>()
                    };
                    _marketBoardRequests.Add(request);
                }

                request.History.AddRange(listing.HistoryListings);

                if (request.IsDone)
                {
                    if (CurrentWorldId == 0)
                    {
                        Log?.Invoke(this, "[ERROR] Not sure about your current world. Please move your character between zones once to start uploading.");
                        _marketBoardRequests.Remove(request);
                        return null;
                    }

                    RequestContentIdUpdate?.Invoke(this, null);

                    if (LocalContentId == 0)
                    {
                        Log?.Invoke(this, "Not sure about your character information. Please log in once with your character while having the program open to verify it.");
                    }

                    LocalContentIdUpdated?.Invoke(this, LocalContentId);

                    Log?.Invoke(this,
                        $"Market Board request finished, starting upload: request#{request.ListingsRequestId} item#{request.CatalogId} amount#{request.AmountToArrive}");
                    return request;
                }

                Log?.Invoke(this, $"Added history for item#{listing.CatalogId}");
                return null;
            }

            return null;
        }
    }
}