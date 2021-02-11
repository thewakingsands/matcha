// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Telemetry;
    using Cafe.Matcha.Utils;

    internal interface INetworkMonitor
    {
        void HandleMessageReceived(string connection, long epoch, byte[] message);
        void HandleMessageSent(string connection, long epoch, byte[] message);
    }

    internal class NetworkMonitor : INetworkMonitor
    {
        private Fate fateTelemetry = new Fate();

        public void HandleMessageReceived(string connection, long epoch, byte[] message)
        {
            try
            {
                HandleMessage(message);
            }
            catch (Exception e)
            {
                try
                {
                    FireException(e);
                }
                catch { }
            }
        }

        private void HandleMessage(byte[] message)
        {
            if (message.Length < 32 || message[12] != 3)
            {
                return;
            }

            var opcode = (MatchaOpcode)BitConverter.ToUInt16(message, 18);
            Universalis.Client.HandlePacket(opcode, message);

            var data = message.Skip(32).ToArray();

            if (opcode == MatchaOpcode.DirectorStart)
            {
                if (message.Length != 168)
                {
                    return;
                }

                var category = BitConverter.ToUInt32(data, 0);
                if (category == 0x250000)
                {
                    FireEvent(new MiniCactpotDTO()
                    {
                        IsNewGame = true,
                        X = (int)BitConverter.ToUInt32(data, 12),
                        Y = (int)BitConverter.ToUInt32(data, 16),
                        Value = (int)BitConverter.ToUInt32(data, 20)
                    });
                }
            }
            else if (opcode == MatchaOpcode.ActorControlSelf)
            {
                if (message.Length != 64)
                {
                    return;
                }

                var type = (ActorControlType)BitConverter.ToUInt16(data, 0);

                switch (type)
                {
                    case ActorControlType.FateProgress:
                        {
                            FireEvent(new FateDTO()
                            {
                                Type = "progress",
                                Fate = BitConverter.ToUInt16(data, 4),
                                Progress = data[8]
                            });
                            break;
                        }

                    case ActorControlType.FateEnd:
                        {
                            FireEvent(new FateDTO()
                            {
                                Type = "end",
                                Fate = BitConverter.ToUInt16(data, 4),
                                Extra = BitConverter.ToUInt16(data, 28)
                            });
                            break;
                        }

                    case ActorControlType.FateStart:
                        {
                            var code = BitConverter.ToUInt16(data, 4);
                            FireEvent(new FateDTO()
                            {
                                Type = "start",
                                Fate = code
                            });
                            fateTelemetry.Send(code);
                            break;
                        }

                    case ActorControlType.DirectorUpdate:
                        {
                            var category = BitConverter.ToUInt32(data, 4);
                            var subCategory = BitConverter.ToUInt32(data, 8);
                            if (category == 0x250000 && subCategory == 2)
                            {
                                FireEvent(new MiniCactpotDTO()
                                {
                                    IsNewGame = false,
                                    X = (int)BitConverter.ToUInt32(data, 12),
                                    Y = (int)BitConverter.ToUInt32(data, 16),
                                    Value = (int)BitConverter.ToUInt32(data, 20)
                                });
                            }

                            break;
                        }

                    case ActorControlType.TreasureSpot:
                        {
                            FireEvent(new TreasureSpotDTO()
                            {
                                Item = (int)BitConverter.ToUInt32(data, 4),
                                Location = (int)BitConverter.ToUInt32(data, 8),
                                IsNew = BitConverter.ToUInt32(data, 12) != 0
                            });
                            break;
                        }
                }
            }
            else if (opcode == MatchaOpcode.ContentFinderNotifyPop)
            {
                if (message.Length != 64)
                {
                    return;
                }

                var roulette = BitConverter.ToUInt16(data, 2);
                var instance = roulette == 0 ? BitConverter.ToUInt16(data, 20) : 0;

                FireEvent(new MatchAlertDTO()
                {
                    Roulette = roulette,
                    Instance = instance
                });
            }
            else if (opcode == MatchaOpcode.CompanyAirshipStatus)
            {
                if (message.Length != 176)
                {
                    return;
                }

                var list = new List<CompanyVoyageStatusItem>();
                for (int i = 0; i < 4; ++i)
                {
                    list.Add(new CompanyVoyageStatusItem
                    {
                        ReturnTime = BitConverter.ToUInt32(data, i * 36),
                        MaxDistance = BitConverter.ToUInt16(data, i * 36 + 4),
                        Name = Helper.ReadString(data, i * 36 + 6, 22),
                        Destination = new int[]
                        {
                            (sbyte)data[i * 36 + 29],
                            (sbyte)data[i * 36 + 30],
                            (sbyte)data[i * 36 + 31],
                            (sbyte)data[i * 36 + 32],
                            (sbyte)data[i * 36 + 33]
                        }
                    });
                }

                FireEvent(new CompanyVoyageStatusDTO()
                {
                    Type = "airship",
                    List = list
                });
            }
            else if (opcode == MatchaOpcode.CompanySubmersibleStatus)
            {
                if (message.Length != 176)
                {
                    return;
                }

                var list = new List<CompanyVoyageStatusItem>();
                for (int i = 0; i < 4; ++i)
                {
                    list.Add(new CompanyVoyageStatusItem
                    {
                        ReturnTime = BitConverter.ToUInt32(data, i * 36),
                        MaxDistance = BitConverter.ToUInt16(data, i * 36 + 4),
                        Name = Helper.ReadString(data, i * 36 + 8, 22),
                        Destination = new int[]
                        {
                            (sbyte)data[i * 36 + 31],
                            (sbyte)data[i * 36 + 32],
                            (sbyte)data[i * 36 + 33],
                            (sbyte)data[i * 36 + 34],
                            (sbyte)data[i * 36 + 35]
                        }
                    });
                }

                FireEvent(new CompanyVoyageStatusDTO()
                {
                    Type = "submersible",
                    List = list
                });
            }
            else if (opcode == MatchaOpcode.InitZone)
            {
                if (message.Length != 128)
                {
                    return;
                }

                FireEvent(new InitZoneDTO()
                {
                    Zone = BitConverter.ToUInt16(data, 2),
                    Instance = BitConverter.ToUInt16(data, 6)
                });
            }
            else if (opcode == MatchaOpcode.EventPlay)
            {
                if (message.Length != 72)
                {
                    return;
                }

                var targetActorId = BitConverter.ToUInt32(message, 8);
                var fishActorId = BitConverter.ToUInt32(data, 0);

                if (targetActorId != fishActorId)
                {
                    return;
                }

                var type = (FishEventType)BitConverter.ToUInt16(data, 12);
                var biteType = BitConverter.ToUInt16(data, 28);

                if (type != FishEventType.Bite)
                {
                    return;
                }

                switch ((FishEventBiteType)biteType)
                {
                    case FishEventBiteType.Big:
                        FireEvent(new FishBiteDTO()
                        {
                            Type = 3
                        });
                        break;
                    case FishEventBiteType.Light:
                        FireEvent(new FishBiteDTO()
                        {
                            Type = 1
                        });
                        break;
                    case FishEventBiteType.Medium:
                        FireEvent(new FishBiteDTO()
                        {
                            Type = 2
                        });
                        break;
                }
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListingCount)
            {
                if (message.Length != 48)
                {
                    return;
                }

                var itemId = BitConverter.ToUInt32(data, 0);
                var count = data[0x0B];

                FireEvent(new MarketBoardItemListingCountDTO()
                {
                    Item = (int)itemId,
                    Count = count,
                    World = (int)ParsePlugin.Instance.GetServer()
                });
                ThreadPool.QueueUserWorkItem(o => Universalis.Client.QueryItem(itemId, FireEvent));
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListing)
            {
                if (message.Length != 1560)
                {
                    return;
                }

                var detail = new List<int>();

                var itemId = (int)BitConverter.ToUInt32(data, 0x2c);
                var items = new List<MarketBoardItemListingItem>();

                const int LISTING_LENGTH = 152;
                for (int i = 0; i < 10; i++)
                {
                    var pricePerUnit = BitConverter.ToUInt32(data, 0x20 + (LISTING_LENGTH * i));
                    if (pricePerUnit == 0)
                    {
                        break;
                    }

                    var quantity = BitConverter.ToUInt32(data, 0x28 + (LISTING_LENGTH * i));
                    var hq = data[0x8c + (LISTING_LENGTH * i)];
                    items.Add(new MarketBoardItemListingItem()
                    {
                        Price = (int)pricePerUnit,
                        Quantity = (int)quantity,
                        HQ = hq != 0
                    });
                }

                FireEvent(new MarketBoardItemListingDTO()
                {
                    Item = itemId,
                    Data = items,
                    World = (int)ParsePlugin.Instance.GetServer()
                });
            }
            else if (opcode == MatchaOpcode.ItemInfo)
            {
                if (message.Length != 96)
                {
                    return;
                }

                // Filter out non-equipped items
                var container = BitConverter.ToUInt16(data, 0x08);
                if (container != 1000)
                {
                    return;
                }

                var materias = new List<Materia>();
                for (int i = 0; i < 5; ++i)
                {
                    materias.Add(new Materia()
                    {
                        Type = BitConverter.ToUInt16(data, 0x2C + 2 * i),
                        Tier = data[0x36 + i]
                    });
                }

                FireEvent(new GearsetDTO()
                {
                    IsSelf = true,
                    Slot = BitConverter.ToUInt16(data, 0x0A),
                    Item = (int)BitConverter.ToUInt32(data, 0x10),
                    HQ = data[0x20] != 0,
                    Glamour = (int)BitConverter.ToUInt32(data, 0x28),
                    Materias = materias
                });
            }
            else if (opcode == MatchaOpcode.InventoryTransaction)
            {
                if (message.Length != 80)
                {
                    return;
                }

                // Filter out non-equipped items
                var container = BitConverter.ToUInt16(data, 0x0c);
                if (container != 1000)
                {
                    return;
                }

                FireEvent(new GearsetDTO()
                {
                    IsSelf = true,
                    Slot = BitConverter.ToUInt16(data, 0x10),
                    Item = 0,
                    HQ = false,
                    Glamour = 0,
                    Materias = new List<Materia>() { }
                });
            }
            else if (opcode == MatchaOpcode.Examine)
            {
                if (message.Length != 1016)
                {
                    return;
                }

                const int offset = 0x40;
                const int length = 0x28;
                for (int slot = 0; slot < 14; ++slot)
                {
                    var detail = new List<int>();

                    var materias = new List<Materia>();
                    for (int i = 0; i < 5; ++i)
                    {
                        materias.Add(new Materia()
                        {
                            Type = BitConverter.ToUInt16(data, offset + slot * length + 18 + 4 * i),
                            Tier = BitConverter.ToUInt16(data, offset + slot * length + 20 + 4 * i)
                        });
                    }

                    FireEvent(new GearsetDTO()
                    {
                        IsSelf = false,
                        Slot = slot,
                        Item = (int)BitConverter.ToUInt32(data, offset + slot * length),
                        HQ = data[offset + slot * length + 0x10] != 0,
                        Glamour = (int)BitConverter.ToUInt32(data, offset + slot * length + 4),
                        Materias = materias
                    });
                }
            }
            else if (opcode == MatchaOpcode.CEDirector)
            {
                FireEvent(new DynamicEventDTO()
                {
                    NextStage = BitConverter.ToUInt32(data, 0),
                    Countdown = (int)BitConverter.ToUInt32(data, 4),
                    Event = data[8],
                    Participants = data[9],
                    Stage = data[10],
                    Progress = data[12],
                });
            }
        }

        public delegate void ExceptionHandler(Exception e);
        public event ExceptionHandler OnException;
        private void FireException(Exception e)
        {
            OnException?.Invoke(e);
        }

        public delegate void EventHandler(BaseDTO args);
        public event EventHandler OnReceiveEvent;
        private void FireEvent(BaseDTO args)
        {
            OnReceiveEvent?.Invoke(args);
        }

        public void HandleMessageSent(string connection, long epoch, byte[] message)
        {
        }
    }
}
