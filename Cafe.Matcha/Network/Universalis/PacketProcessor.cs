namespace Cafe.Matcha.Network.Universalis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.Network.Structures;

    internal class PacketProcessor : IDisposable
    {
        private readonly Api _uploader;

        private readonly IDisposable handleMarketBoardItemRequest;

        private event Action<Packet> MarketBoardHistoryReceived;
        private event Action<Packet> MarketBoardItemRequestStartReceived;
        private event Action<Packet> MarketBoardOfferingsReceived;

        /// <summary>
        /// Gets an observable to track marketboard history events.
        /// </summary>
        public IObservable<MarketBoardHistory> MbHistoryObservable { get; }

        /// <summary>
        /// Gets an observable to track marketboard item request events.
        /// </summary>
        public IObservable<MarketBoardItemRequest> MbItemRequestObservable { get; }

        /// <summary>
        /// Gets an observable to track marketboard offerings events.
        /// </summary>
        public IObservable<MarketBoardCurrentOfferings> MbOfferingsObservable { get; }

        public ushort CurrentWorldId
        {
            get
            {
                return State.Instance.WorldId;
            }
        }

        public ulong LocalContentId { get; set; }

        public EventHandler<string> Log;

        public PacketProcessor(string apiKey)
        {
            _uploader = new Api(this, apiKey);
            MbHistoryObservable = Observable.Create<MarketBoardHistory>(observer =>
            {
                MarketBoardHistoryReceived += Observe;
                return () => { MarketBoardHistoryReceived -= Observe; };

                void Observe(Packet packet)
                {
                    observer.OnNext(MarketBoardHistory.Read(packet.GetRawData()));
                }
            });

            MbItemRequestObservable = Observable.Create<MarketBoardItemRequest>(observer =>
            {
                MarketBoardItemRequestStartReceived += Observe;
                return () => MarketBoardItemRequestStartReceived -= Observe;

                void Observe(Packet packet)
                {
                    observer.OnNext(MarketBoardItemRequest.Read(packet.GetRawData()));
                }
            });

            MbOfferingsObservable = Observable.Create<MarketBoardCurrentOfferings>(observer =>
            {
                MarketBoardOfferingsReceived += Observe;
                return () => { MarketBoardOfferingsReceived -= Observe; };

                void Observe(Packet packet)
                {
                    observer.OnNext(MarketBoardCurrentOfferings.Read(packet.GetRawData()));
                }
            });

            handleMarketBoardItemRequest = HandleMarketBoardItemRequest();
        }

        /// <summary>
        /// Process a zone proto message and scan for relevant data.
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        /// <param name="packet">The packet.</param>
        public void ProcessZonePacket(MatchaOpcode opcode, Packet packet)
        {
            if (opcode == MatchaOpcode.PlayerSetup)
            {
                // Mask lower-32bit for privacy concern
                LocalContentId = BitConverter.ToUInt64(packet.Bytes, 0x20) & 0xffffffff00000000;
                LocalContentId = LocalContentId | GetClientIdentifier();
                Log?.Invoke(this, $"New CID: {LocalContentId.ToString("X")}");
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListingCount)
            {
                MarketBoardItemRequestStartReceived?.Invoke(packet);
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListing)
            {
                MarketBoardOfferingsReceived?.Invoke(packet);
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListingHistory)
            {
                MarketBoardHistoryReceived?.Invoke(packet);
            }
        }

        private IObservable<List<MarketBoardCurrentOfferings.MarketBoardItemListing>> OnMarketBoardListingsBatch(
            IObservable<MarketBoardItemRequest> start)
        {
            var offeringsObservable = MbOfferingsObservable.Publish().RefCount();

            void LogEndObserved(MarketBoardCurrentOfferings offerings)
            {
                Log?.Invoke(this, $"Observed end of request {offerings.RequestId}");
            }

            void LogOfferingsObserved(MarketBoardCurrentOfferings offerings)
            {
                Log?.Invoke(this, $"Observed element of request {offerings.RequestId} with {offerings.InternalItemListings.Count} listings");
            }

            IObservable<MarketBoardCurrentOfferings> UntilBatchEnd(MarketBoardItemRequest request)
            {
                var totalPackets = Convert.ToInt32(Math.Ceiling((double)request.AmountToArrive / 10));
                if (totalPackets == 0)
                {
                    return Observable.Empty<MarketBoardCurrentOfferings>();
                }

                return offeringsObservable
                       .Where(offerings => offerings.InternalItemListings.All(l => l.CatalogId != 0))
                       .Skip(totalPackets - 1)
                       .Do(LogEndObserved);
            }

            // When a start packet is observed, begin observing a window of listings packets
            // according to the count described by the start packet. Aggregate the listings
            // packets, and then flatten them to the listings themselves.
            return offeringsObservable
                   .Do(LogOfferingsObserved)
                   .Window(start, UntilBatchEnd)
                   .SelectMany(
                       o => o.Aggregate(
                           new List<MarketBoardCurrentOfferings.MarketBoardItemListing>(),
                           (agg, next) =>
                           {
                               agg.AddRange(next.InternalItemListings);
                               return agg;
                           }));
        }

        private IObservable<List<MarketBoardHistory.MarketBoardHistoryListing>> OnMarketBoardSalesBatch(
            IObservable<MarketBoardItemRequest> start)
        {
            var historyObservable = MbHistoryObservable.Publish().RefCount();

            void LogHistoryObserved(MarketBoardHistory history)
            {
                Log?.Invoke(this, $"Observed history for item {history.CatalogId} with {history.InternalHistoryListings.Count} sales");
            }

            IObservable<MarketBoardHistory> UntilBatchEnd(MarketBoardItemRequest request)
            {
                return historyObservable
                       .Where(history => history.CatalogId != 0)
                       .Take(1);
            }

            // When a start packet is observed, begin observing a window of history packets.
            // We should only get one packet, which the window closing function ensures.
            // This packet is flattened to its sale entries and emitted.
            return historyObservable
                   .Do(LogHistoryObserved)
                   .Window(start, UntilBatchEnd)
                   .SelectMany(
                       o => o.Aggregate(
                           new List<MarketBoardHistory.MarketBoardHistoryListing>(),
                           (agg, next) =>
                           {
                               agg.AddRange(next.InternalHistoryListings);
                               return agg;
                           }));
        }

        private IDisposable HandleMarketBoardItemRequest()
        {
            void LogStartObserved(MarketBoardItemRequest request)
            {
                Log?.Invoke(this, $"Observed start of request for item with {request.AmountToArrive} expected listings");
            }

            var startObservable = MbItemRequestObservable
                                      .Where(request => request.Ok).Do(LogStartObserved)
                                      .Publish()
                                      .RefCount();
            return Observable.When(
                                 startObservable
                                     .And(OnMarketBoardSalesBatch(startObservable))
                                     .And(OnMarketBoardListingsBatch(startObservable))
                                     .Then((request, sales, listings) => (request, sales, listings)))
                .Where(ShouldUpload)
                             .SubscribeOn(ThreadPoolScheduler.Instance)
                             .Subscribe(
                                 data =>
                                 {
                                     var (request, sales, listings) = data;
                                     UploadMarketBoardData(request, sales, listings);
                                 },
                                 ex => Log?.Invoke(this, $"Failed to handle Market Board item request event: {ex}"));
        }

        private void UploadMarketBoardData(
            MarketBoardItemRequest request,
            ICollection<MarketBoardHistory.MarketBoardHistoryListing> sales,
            ICollection<MarketBoardCurrentOfferings.MarketBoardItemListing> listings)
        {
            var catalogId = listings.FirstOrDefault()?.CatalogId ?? 0;
            if (catalogId == 0)
            {
                Log?.Invoke(this, $"Wrong catalogId of Market Board listings received for request: item#{catalogId}");
                return;
            }

            if (listings.Count != request.AmountToArrive)
            {
                Log?.Invoke(this, $"Wrong number of Market Board listings received for request: {listings.Count} != {request.AmountToArrive} item#{catalogId}");
                return;
            }

            Log?.Invoke(this, $"Market Board request resolved, starting upload: item#{catalogId} listings#{listings.Count} sales#{sales.Count}");

            request.Listings.AddRange(listings);
            request.History.AddRange(sales);

            Task.Run(() => _uploader.Upload(CurrentWorldId, request))
                .ContinueWith(
                    task => Log?.Invoke(this, "Market Board offerings data upload failed"),
                    TaskContinuationOptions.OnlyOnFaulted);
        }

        private uint GetClientIdentifier()
        {
            var uuid = Config.Instance.Telemetry.UUID;
            if (string.IsNullOrEmpty(uuid))
            {
                return 0;
            }

            if (Guid.TryParse(uuid, out var guid))
            {
                var array = guid.ToByteArray();
                return BitConverter.ToUInt32(array, 0);
            }
            else
            {
                return 0;
            }
        }

        private bool ShouldUpload<T>(T any)
        {
            return Config.Instance.Overlay.Universalis;
        }

        public void Dispose()
        {
            handleMarketBoardItemRequest.Dispose();
        }
    }
}