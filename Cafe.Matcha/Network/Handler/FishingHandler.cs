// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;

    internal class FishingHandler : AbstractHandler
    {
        private const uint FishingEventId = 0x00150001;
        private readonly object sync = new object();
        private readonly HashSet<uint> statuses = new HashSet<uint>();
        private bool statusesKnown;
        private uint actorId;
        private uint? baitId;
        private uint? placeId;
        private FishingDTO cast;
        private string phase;
        private uint? pendingCommand;
        private long pendingTime;

        public FishingHandler(Action<BaseDTO> fireEvent) : base(fireEvent) { }

        public override bool Handle(Packet packet)
        {
            // Sent and received callbacks may run on different ACT worker threads.
            lock (sync)
            {
                return HandlePacket(packet);
            }
        }

        private bool HandlePacket(Packet packet)
        {
            if (!packet.Valid || !packet.Known)
            {
                return false;
            }

            if (packet.MatchaOpcode == MatchaOpcode.InitZone)
            {
                if (packet.Length != 168)
                {
                    return false;
                }

                Emit("reset", packet.Timestamp);
                cast = null;
                phase = null;
                actorId = 0;
                baitId = placeId = pendingCommand = null;
                statuses.Clear();
                statusesKnown = false;
                return false; // The normal zone handler must also run.
            }

            if (packet.Sender == Packet.PacketSender.Client)
            {
                if (packet.MatchaOpcode != MatchaOpcode.ClientTrigger || packet.DataLength < 12 || packet.ReadUInt32(0) != 0x2bd)
                {
                    return false;
                }

                pendingCommand = packet.ReadUInt32(4);
                pendingTime = packet.Timestamp;
                return true; // A request never starts a timer; wait for the server.
            }

            if (packet.MatchaOpcode == MatchaOpcode.ActorControlSelf)
            {
                if (packet.Length != 72 || packet.ReadUInt16(0) != (uint)ActorControlType.FishingBaitChange)
                {
                    return false;
                }

                ObserveActor(packet.Target, packet.Timestamp);
                baitId = packet.ReadUInt32(4);
                State.Instance.FishingBait = baitId.Value;
                Emit("bait", packet.Timestamp);
                return true;
            }

            if (packet.MatchaOpcode == MatchaOpcode.StatusEffectList)
            {
                if (packet.DataLength != 384 || packet.Source != packet.Target)
                {
                    return false;
                }

                ObserveActor(packet.Target, packet.Timestamp);
                statuses.Clear();
                for (var offset = 20; offset < 380; offset += 12)
                {
                    var status = packet.ReadUInt16(offset);
                    if (status != 0)
                    {
                        statuses.Add(status);
                    }
                }

                statusesKnown = true;
                return true;
            }

            if (packet.MatchaOpcode == MatchaOpcode.ActorControl)
            {
                if (packet.Length != 56 || packet.Source != actorId)
                {
                    return false;
                }

                var category = packet.ReadUInt16(0);
                if (category == 20)
                {
                    statuses.Add(packet.ReadUInt32(4));
                }

                if (category == 21)
                {
                    statuses.Remove(packet.ReadUInt32(4));
                }

                // Changes do not retroactively alter the cast's status snapshot.
                return false;
            }

            if (packet.MatchaOpcode == MatchaOpcode.SystemLogMessage)
            {
                if (packet.DataLength != 24 || packet.ReadUInt32(0) != FishingEventId)
                {
                    return false;
                }

                var message = packet.ReadUInt32(4);
                if (message == 1110 || message == 1115)
                {
                    placeId = packet.ReadUInt32(12);
                    if (cast != null && (phase == "cast" || message == 1115))
                    {
                        cast.PlaceId = placeId;
                        if (message == 1110)
                        {
                            cast.Mooch = false;
                            cast.BaitId = baitId;
                        }

                        Emit("place", packet.Timestamp);
                    }
                }

                // Server confirmation identifies the effective live bait, including Mooch II.
                // Do not guess undocumented ClientTrigger subcommands.
                else if (message == 1121 && cast != null && phase == "cast" && packet.Timestamp - cast.CastTime <= 2000)
                {
                    cast.Mooch = true;
                    cast.BaitId = packet.ReadUInt32(12);
                    cast.PlaceId = placeId;
                    Emit("place", packet.Timestamp);
                }
                else if (message >= 1111 && message <= 1113)
                {
                    Emit("quit", packet.Timestamp);
                    cast = null;
                    phase = null;
                    placeId = null;
                    pendingCommand = null;
                }
                else if ((message >= 1117 && message <= 1120) || message == 1127 || message == 1129)
                {
                    if (cast != null && phase != "catch" && phase != "end")
                    {
                        phase = "end";
                        Emit("end", packet.Timestamp);
                    }
                }

                return true;
            }

            if (packet.MatchaOpcode == MatchaOpcode.FishCaught)
            {
                if (packet.DataLength != 16 || packet.Target != actorId || cast == null || (phase != "reel" && phase != "hook"))
                {
                    return false;
                }

                var fish = packet.ReadUInt32(0);
                if (fish == 0)
                {
                    return true;
                }

                cast.FishId = fish;
                phase = "catch";
                Emit("catch", packet.Timestamp);
                return true;
            }

            if (packet.MatchaOpcode != MatchaOpcode.EventPlay && packet.MatchaOpcode != MatchaOpcode.EventPlay4)
            {
                return false;
            }

            var expectedLength = packet.MatchaOpcode == MatchaOpcode.EventPlay ? 72 : 80;
            if (packet.Length != expectedLength || packet.ReadUInt32(8) != FishingEventId)
            {
                return false;
            }

            if (packet.ReadUInt32(0) != packet.Target)
            {
                return true;
            }

            ObserveActor(packet.Target, packet.Timestamp);
            var type = (FishEventType)packet.ReadUInt16(12);
            switch (type)
            {
                case FishEventType.Cast:
                    // Duplicate delivery of the same network transition must not restart a cast.
                    if (cast != null && cast.CastTime == packet.Timestamp)
                    {
                        return true;
                    }

                    var commandKnown = pendingCommand.HasValue && packet.Timestamp >= pendingTime && packet.Timestamp - pendingTime < 5000;
                    var mooch = commandKnown && pendingCommand == 0 ? (bool?)false : null;
                    cast = new FishingDTO
                    {
                        CastId = Guid.NewGuid().ToString("N"), ActorId = actorId,
                        CastTime = packet.Timestamp, BaseBaitId = baitId,
                        BaitId = mooch == false ? baitId : null,
                        Mooch = mooch, Chum = statusesKnown ? (bool?)statuses.Contains(0x2fb) : null,
                        Snagging = statusesKnown ? (bool?)statuses.Contains(0x2f9) : null,
                        Statuses = statusesKnown ? statuses.OrderBy(id => id).ToArray() : null,
                    };
                    pendingCommand = null;
                    phase = "cast";
                    Emit("cast", packet.Timestamp);
                    break;
                case FishEventType.Bite:
                    var tug = (int)packet.ReadUInt16(28) - (int)FishEventBiteType.Light + 1;
                    if (tug < 1 || tug > 3)
                    {
                        return true;
                    }

                    // Keep the existing FishBite event for older consumers.
                    if (cast == null || phase == "cast")
                    {
                        fireEvent(new FishBiteDTO { Time = packet.Timestamp, Type = tug });
                    }

                    if (cast == null || phase != "cast")
                    {
                        return true;
                    }

                    cast.BiteTime = packet.Timestamp;
                    cast.Tug = tug;
                    phase = "bite";
                    Emit("bite", packet.Timestamp);
                    break;
                case FishEventType.Hook:
                    if (cast == null || (phase != "cast" && phase != "bite"))
                    {
                        return true;
                    }

                    cast.HookTime = packet.Timestamp;
                    phase = "hook";
                    Emit("hook", packet.Timestamp);
                    break;
                case FishEventType.ReelIn:
                    if (cast == null || phase == "catch" || phase == "end")
                    {
                        return true;
                    }

                    phase = "reel";
                    Emit("reel", packet.Timestamp);
                    break;
                case FishEventType.Ready:
                    if (cast != null && phase != "catch" && phase != "end")
                    {
                        phase = "end";
                        Emit("end", packet.Timestamp);
                    }

                    break;
                case FishEventType.End:
                    Emit("quit", packet.Timestamp);
                    cast = null;
                    placeId = null;
                    phase = null;
                    break;
            }

            return true;
        }

        private void ObserveActor(uint id, long time)
        {
            if (actorId != 0 && actorId != id)
            {
                Emit("reset", time);
                cast = null;
                phase = null;
                baitId = placeId = pendingCommand = null;
                statuses.Clear();
                statusesKnown = false;
            }

            actorId = id;
        }

        private void Emit(string action, long time)
        {
            // Never publish the mutable instance retained by the handler.
            fireEvent(new FishingDTO
            {
                Action = action, Time = time, ActorId = actorId,
                CastId = cast?.CastId, CastTime = cast?.CastTime, BiteTime = cast?.BiteTime,
                HookTime = cast?.HookTime, BaitId = cast?.BaitId,
                BaseBaitId = cast != null && action != "bait" ? cast.BaseBaitId : baitId,
                PlaceId = cast != null ? cast.PlaceId : placeId,
                Tug = cast?.Tug, FishId = cast?.FishId, Mooch = cast?.Mooch,
                Chum = cast?.Chum, Snagging = cast?.Snagging,
                Statuses = cast?.Statuses,
            });
        }
    }
}
