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
        private NpcSpawn npcTelemetry = new NpcSpawn();

        private bool ToMatchaOpcode(ushort opcode, out MatchaOpcode matchaOpcode)
        {
            var region = Config.Instance.Region;
            switch (region)
            {
                case Region.Global:
                    return OpcodeStorage.Global.TryGetValue(opcode, out matchaOpcode);

                case Region.China:
                    return OpcodeStorage.China.TryGetValue(opcode, out matchaOpcode);

                default:
                    matchaOpcode = default;
                    return false;
            }
        }

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

            var processed = HandleMessageByOpcode(message);
            if (!processed)
            {
                TryHandleMessage(message);
            }
        }

        private void TryHandleMessage(byte[] message)
        {
            var data = message.Skip(32).ToArray();
            // Treasure Shifting Wheel Result
            if (message.Length == 88)
            {
                var level = BitConverter.ToUInt32(data, 24);
                if (
                    level == 7636061 || // G10 运河宝物库神殿
                    level == 8508181 // G12 梦羽宝殿
                )
                {
                    var result = (TreasureShiftingWheelResultType)data[40];
                    switch (result)
                    {
                        case TreasureShiftingWheelResultType.Low:
                            FireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-low"
                            });
                            break;
                        case TreasureShiftingWheelResultType.Medium:
                            FireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-medium"
                            });
                            break;
                        case TreasureShiftingWheelResultType.High:
                            FireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-high"
                            });
                            break;
                        case TreasureShiftingWheelResultType.Shift:
                            FireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-shift"
                            });
                            break;
                        case TreasureShiftingWheelResultType.Special:
                            FireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-special"
                            });
                            break;
                        case TreasureShiftingWheelResultType.End:
                            FireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-end"
                            });
                            break;
                    }
                }
            }
            else if (message.Length == 96)
            {
                var flag = BitConverter.ToUInt32(data, 16);
                if (flag == 0x04482c03)
                {
                    FireEvent(new TreasureResultDTO()
                    {
                        Round = data[32] + 1,
                        Value = data[40] == 1 ? "gate-open" : "gate-fail"
                    });
                }
            }
        }

        private bool HandleMessageByOpcode(byte[] message)
        {
            if (!ToMatchaOpcode(BitConverter.ToUInt16(message, 18), out var opcode))
            {
                return false;
            }

            Universalis.Client.HandlePacket(opcode, message);

            var data = message.Skip(32).ToArray();

            if (opcode == MatchaOpcode.DirectorStart)
            {
                if (message.Length != 168)
                {
                    return false;
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
            else if (opcode == MatchaOpcode.NpcSpawn)
            {
                if (message.Length != 672)
                {
                    return false;
                }

                var bNpcName = BitConverter.ToUInt32(data, 68);
                var hpMax = BitConverter.ToUInt32(data, 92);
                var hpCur = BitConverter.ToUInt32(data, 96);
                var fateId = BitConverter.ToUInt16(data, 104);
                var level = data[127];

                if (fateId != 0 || IsSpecialNpcName(bNpcName))
                {
                    const int posOffset = 508;
                    npcTelemetry.Send(
                        bNpcName,
                        fateId,
                        BitConverter.ToSingle(data, posOffset),
                        BitConverter.ToSingle(data, posOffset + 4),
                        BitConverter.ToSingle(data, posOffset + 8),
                        level,
                        hpMax
                    );
                }
            }
            else if (opcode == MatchaOpcode.ActorControlSelf)
            {
                if (message.Length != 64)
                {
                    return false;
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
                    return false;
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
                    return false;
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
                    return false;
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
                    return false;
                }

                State.Instance.ServerId = BitConverter.ToUInt16(data, 0);
                State.Instance.ZoneId = BitConverter.ToUInt16(data, 2);
                State.Instance.InstanceId = BitConverter.ToUInt16(data, 4);

                FireEvent(new InitZoneDTO()
                {
                    Zone = State.Instance.ZoneId,
                    Instance = BitConverter.ToUInt16(data, 6)
                });
            }
            else if (opcode == MatchaOpcode.EventPlay)
            {
                if (message.Length != 72)
                {
                    return false;
                }

                var targetActorId = BitConverter.ToUInt32(message, 8);
                var fishActorId = BitConverter.ToUInt32(data, 0);

                if (targetActorId != fishActorId)
                {
                    return true;
                }

                var type = (FishEventType)BitConverter.ToUInt16(data, 12);
                var biteType = BitConverter.ToUInt16(data, 28);

                if (type != FishEventType.Bite)
                {
                    return true;
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
                    return false;
                }

                var itemId = BitConverter.ToUInt32(data, 0);
                var count = data[0x0B];

                FireEvent(new MarketBoardItemListingCountDTO()
                {
                    Item = (int)itemId,
                    Count = count,
                    World = State.Instance.WorldId
                });
                ThreadPool.QueueUserWorkItem(o => Universalis.Client.QueryItem(State.Instance.WorldId, itemId, FireEvent));
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListing)
            {
                if (message.Length != 1560)
                {
                    return false;
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
                        Price = (int)(pricePerUnit * 1.05),
                        Quantity = (int)quantity,
                        HQ = hq != 0
                    });
                }

                FireEvent(new MarketBoardItemListingDTO()
                {
                    Item = itemId,
                    Data = items,
                    World = State.Instance.WorldId
                });
            }
            else if (opcode == MatchaOpcode.ItemInfo)
            {
                if (message.Length != 96)
                {
                    return false;
                }

                // Filter out non-equipped items
                var container = BitConverter.ToUInt16(data, 0x08);
                if (container != 1000)
                {
                    return true;
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
                    return false;
                }

                // Filter out non-equipped items
                var container = BitConverter.ToUInt16(data, 0x0c);
                if (container != 1000)
                {
                    return true;
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
                    return false;
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
                    Zone = State.Instance.ZoneId,
                    Event = data[8],
                    Participants = data[9],
                    Stage = data[10],
                    Progress = data[12],
                });
            }
            else if (opcode == MatchaOpcode.PlayerSpawn)
            {
                State.Instance.WorldId = BitConverter.ToUInt16(data, 4);
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool IsSpecialNpcName(uint bNpcName)
        {
            return (bNpcName >= 2919 && bNpcName <= 2969) ||
                   (bNpcName >= 4350 && bNpcName <= 4378) ||
                    bNpcName == 4380 ||
                   (bNpcName >= 5984 && bNpcName <= 6013) ||
                   (bNpcName >= 8653 && bNpcName <= 8657) ||
                   (bNpcName >= 8890 && bNpcName <= 8916) ||
                   (bNpcName >= 10615 && bNpcName <= 10646);
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
