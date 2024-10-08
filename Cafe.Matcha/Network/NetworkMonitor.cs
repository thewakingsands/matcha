// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Text;
    using System.Threading;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Network.Structures;
    using Cafe.Matcha.Utils;

    internal interface INetworkMonitor
    {
        void HandleMessageReceived(string connection, long epoch, byte[] message);
        void HandleMessageSent(string connection, long epoch, byte[] message);
    }

    internal class NetworkMonitor : INetworkMonitor
    {
        public void HandleMessageReceived(string connection, long epoch, byte[] message)
        {
            try
            {
                HandleMessage(new Packet(message));
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

        private void HandleMessage(Packet packet)
        {
            if (!packet.Valid)
            {
                return;
            }

            var processed = HandleMessageByOpcode(packet);
            if (!processed)
            {
#if DEBUG
                if (packet.GetMatchaOpcode(out var opcode))
                {
                    LogIncorrectPacketSize(opcode, packet.Length);
                    Log.Packet(packet.Bytes);
                }
#endif

                TryHandleMessage(packet);
            }
        }

        private void TryHandleMessage(Packet packet)
        {
            // Treasure Shifting Wheel Result
            if (packet.DataLength == 88)
            {
                var data = packet.GetRawData();
                var level = BitConverter.ToUInt32(data, 24);
                if (
                    level == 7636061 || // G10 运河宝物库神殿
                    level == 8508181 || // G12 梦羽宝殿
                    level == 9413549 // G15 育体宝殿
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
            else if (packet.DataLength == 96)
            {
                var data = packet.GetRawData();
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

        private bool HandleMessageByOpcode(Packet packet)
        {
            if (!packet.GetMatchaOpcode(out var opcode))
            {
                return false;
            }

            Universalis.Client.HandlePacket(opcode, packet);

            var data = packet.GetRawData();
            if (opcode == MatchaOpcode.DirectorStart)
            {
                if (packet.Length != 168)
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
                if (packet.Length != 680)
                {
                    return false;
                }

                var bNpcName = BitConverter.ToUInt32(data, 68);
                var location = BitConverter.ToUInt32(data, 72);
                var hpMax = BitConverter.ToUInt32(data, 92);
                var hpCur = BitConverter.ToUInt32(data, 96);
                var fateId = BitConverter.ToUInt16(data, 104);
                var level = data[127];

                if (fateId != 0 || IsSpecialNpcName(bNpcName))
                {
                    const int posOffset = 508;
                    var pos = new Vector3(
                        BitConverter.ToSingle(data, posOffset),
                        BitConverter.ToSingle(data, posOffset + 4),
                        BitConverter.ToSingle(data, posOffset + 8));

                    State.Instance.Npc.Update(packet.Source, (npc) =>
                    {
                        if (npc.BNpcName != bNpcName)
                        {
                            npc.BNpcName = bNpcName;
                            npc.Location = location;
                            npc.Fate = fateId;
                            npc.Level = level;
                            npc.CurHP = hpCur;
                            npc.MaxHP = hpMax;
                            npc.Position = pos;
                            return true;
                        }

                        return false;
                    });
                }
            }
            else if (opcode == MatchaOpcode.ActorControl)
            {
                if (packet.Length != 56)
                {
                    return false;
                }

                var type = (ActorControlType)BitConverter.ToUInt16(data, 0);
                switch (type)
                {
                    case ActorControlType.SetStatus:
                        {
                            var status = BitConverter.ToUInt32(data, 4);
                            if (status == 2)
                            {
                                State.Instance.Npc.Remove(packet.Source);
                            }

                            break;
                        }
                }
            }
            else if (opcode == MatchaOpcode.FateInfo)
            {
                if (packet.Length != 56)
                {
                    return false;
                }

                var fateId = BitConverter.ToUInt16(data, 0);
                var startTime = BitConverter.ToUInt32(data, 8);
                var duration = BitConverter.ToUInt32(data, 16);

                State.Instance.Fate.Update(fateId, (state) =>
                {
                    bool updated = false;
                    if (state.StartTime != startTime)
                    {
                        state.StartTime = startTime;
                        updated = true;
                    }

                    if (state.Duration != duration)
                    {
                        state.Duration = duration;
                        updated = true;
                    }

                    return updated;
                });
            }
            else if (opcode == MatchaOpcode.ActorControlSelf)
            {
                if (packet.Length != 64)
                {
                    return false;
                }

                var type = (ActorControlType)BitConverter.ToUInt16(data, 0);
#if DEBUG
                var sb = new StringBuilder();
                for (int i = 4; i < 32; i += 4)
                {
                    sb.Append(BitConverter.ToUInt32(data, 4));
                    sb.Append(' ');
                }

                Log.Debug(LogType.ActorControlSelf, $"type {type}, {sb}");
#endif
                switch (type)
                {
                    case ActorControlType.FateProgress:
                        {
                            var fateId = BitConverter.ToUInt16(data, 4);
                            var progress = data[8];

                            State.Instance.Fate.Update(fateId, (fate) =>
                            {
                                if (fate.Progress != progress)
                                {
                                    fate.Progress = progress;

                                    if (progress == 100)
                                    {
                                        return true;
                                    }
                                }

                                return false;
                            });
                            FireEvent(new FateDTO()
                            {
                                Type = "progress",
                                Fate = fateId,
                                Progress = progress
                            });
                            break;
                        }

                    case ActorControlType.FateEnd:
                        {
                            var fateId = BitConverter.ToUInt16(data, 4);

                            State.Instance.Fate.Remove(fateId);
                            FireEvent(new FateDTO()
                            {
                                Type = "end",
                                Fate = fateId,
                                Extra = BitConverter.ToUInt16(data, 28)
                            });
                            break;
                        }

                    case ActorControlType.FateStart:
                        {
                            var fateId = BitConverter.ToUInt16(data, 4);

                            State.Instance.Fate.Update(fateId, (fate) => false);
                            FireEvent(new FateDTO()
                            {
                                Type = "start",
                                Fate = fateId
                            });
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
                if (packet.Length != 72)
                {
                    return false;
                }

                var roulette = BitConverter.ToUInt16(data, 2);
                var instance = roulette == 0 ? BitConverter.ToUInt16(data, 0x1c) : 0;

                FireEvent(new MatchAlertDTO()
                {
                    Roulette = roulette,
                    Instance = instance
                });
            }
            else if (opcode == MatchaOpcode.CompanyAirshipStatus)
            {
                if (packet.Length != 176)
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
                if (packet.Length != 176)
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
                if (packet.Length != 136)
                {
                    return false;
                }

                var serverId = BitConverter.ToUInt16(data, 0);
                var zoneId = BitConverter.ToUInt16(data, 2);
                var instanceId = BitConverter.ToUInt16(data, 4);
                var contentId = BitConverter.ToUInt16(data, 6);

                State.Instance.HandleInitZone(serverId, zoneId, instanceId, contentId);
                FireEvent(new InitZoneDTO()
                {
                    Zone = zoneId,
                    Instance = contentId
                });
            }
            else if (opcode == MatchaOpcode.EventPlay)
            {
                if (packet.Length != 72)
                {
                    return false;
                }

                var targetActorId = packet.Target;
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
            else if (opcode == MatchaOpcode.MarketBoardItemListingHistory)
            {
                return true;
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListingCount)
            {
                // Useless as ItemId is removed in 7.0
                return true;
            }
            else if (opcode == MatchaOpcode.MarketBoardItemListing)
            {
                var result = MarketBoardCurrentOfferings.Read(data);
                var items = new List<MarketBoardItemListingItem>();

                uint itemId = 0;
                foreach (var item in result.ItemListings)
                {
                    if (item.PricePerUnit == 0)
                    {
                        break;
                    }

                    itemId = item.ItemId;
                    items.Add(new MarketBoardItemListingItem()
                    {
                        // Price = (int)(pricePerUnit * 1.05),
                        Price = (int)item.PricePerUnit,
                        Quantity = (int)item.ItemQuantity,
                        HQ = item.IsHq
                    });
                }

                if (itemId != 0)
                {
                    FireEvent(new MarketBoardItemListingDTO()
                    {
                        Item = (int)itemId,
                        Data = items,
                        World = State.Instance.WorldId
                    });
                }
            }
            else if (opcode == MatchaOpcode.ItemInfo)
            {
                if (packet.Length != 96)
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
                if (packet.Length != 80)
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
                if (packet.Length != 960)
                {
                    return false;
                }

                const int offset = 0x50;
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
                var isCurrentPlayer = packet.Source == packet.Target;
                var currentWorldId = BitConverter.ToUInt16(data, 4);

                State.Instance.HandleWorldId(currentWorldId, isCurrentPlayer);
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

#if DEBUG
        private void LogIncorrectPacketSize(MatchaOpcode opcode, int size)
        {
            Log.Warn(LogType.InvalidPacket, $"{Enum.GetName(typeof(MatchaOpcode), opcode)} length {size}");
        }
#endif
    }
}
