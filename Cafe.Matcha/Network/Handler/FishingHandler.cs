// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Network.Fishing;

    internal class FishingHandler : AbstractHandler
    {
        private const uint FishingEventId = 0x00150001;
        private readonly object sync = new object();
        private readonly HashSet<uint> statuses = new HashSet<uint>();
        private readonly Dictionary<uint, uint> statusStacks = new Dictionary<uint, uint>();
        private bool statusesKnown;
        private uint actorId;
        private uint? baitId;
        private uint? placeId;
        private FishingDTO cast;
        private string phase;
        private uint? pendingCommand;
        private long pendingTime;

        private readonly Func<FishingAttributes> readAttributes;
        private FishingCapture capture;
        private uint slap, identical, swimBait, taskBait;
        private bool taskActive;
        public FishingHandler(Action<BaseDTO> fireEvent) : this(fireEvent, () => null) { }
        public FishingHandler(Action<BaseDTO> fireEvent, Func<FishingAttributes> attributes) : base(fireEvent)
        { readAttributes = attributes; }

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

            if (packet.MatchaOpcode == MatchaOpcode.PlayerSetup && packet.DataLength >= 0x30)
            {
                baitId = packet.ReadUInt32(0x2c);
                State.Instance.FishingBait = baitId.Value;
                Emit("bait", packet.Timestamp);
                return false;
            }

            if (packet.MatchaOpcode == MatchaOpcode.InitZone || packet.MatchaOpcode == MatchaOpcode.Logout)
            {
                if (packet.MatchaOpcode == MatchaOpcode.InitZone && packet.Length != 168)
                {
                    return false;
                }

                Emit("reset", packet.Timestamp);
                cast = null;
                phase = null;
                actorId = 0;
                capture = null; slap = identical = swimBait = taskBait = 0; taskActive = false;
                baitId = placeId = pendingCommand = null;
                statuses.Clear(); statusStacks.Clear();
                statusesKnown = false;
                return false; // The normal zone handler must also run.
            }

            if (packet.Sender == Packet.PacketSender.Client)
            {
                if (packet.MatchaOpcode != MatchaOpcode.ClientTrigger || packet.DataLength < 12 || packet.ReadUInt32(0) != 0x2bd)
                {
                    return false;
                }

                var command = packet.ReadUInt32(4);
                if (command == 0 || command == 3 || command == 15)
                { pendingCommand = command; pendingTime = packet.Timestamp; }
                if (capture != null && cast != null)
                {
                    switch (command)
                    {
                        case 2: capture.Hook = 1; break;
                        case 10: capture.Hook = 2; break;
                        case 11: capture.Hook = 3; break;
                        case 16: capture.Hook = 4; break;
                        case 21: capture.Hook = 5; break;
                        case 28: capture.Hook = 6; break;
                        case 22: case 23:
                            capture.LureTarget = false;
                            break;
                    }
                }
                return true; // A request never starts a timer; wait for the server.
            }

            if (packet.MatchaOpcode == MatchaOpcode.ActorControlSelf)
            {
                if (packet.Length != 72) return false;
                var category = packet.ReadUInt16(0);
                if (category == 325)
                {
                    ObserveActor(packet.Target, packet.Timestamp);
                    baitId = packet.ReadUInt32(4);
                    State.Instance.FishingBait = baitId.Value;
                    Emit("bait", packet.Timestamp); return true;
                }
                if (packet.Target != actorId) return false;
                if (category == 338)
                {
                    var index = packet.ReadUInt32(4);
                    swimBait = index < 3 ? packet.ReadUInt32(8 + (int)index * 4) : 0;
                    return true;
                }
                if (category == 3501) { taskBait = packet.ReadUInt32(4); return true; }
                if (category == 517)
                    return HandleLog(packet.ReadUInt32(4), packet.ReadUInt32(8), packet.Timestamp);
                if (category == 320 && cast != null && (phase == "hook" || phase == "reel"))
                {
                    var packed = packet.ReadUInt32(8);
                    Catch(packet.ReadUInt32(4), (int)(packed >> 16), (int)(packed & 255),
                        (packet.ReadUInt32(12) & 16) != 0, packet.Timestamp);
                    return true;
                }
                return false;
            }

            if (packet.MatchaOpcode == MatchaOpcode.StatusEffectList || packet.MatchaOpcode == MatchaOpcode.StatusEffectList2 || packet.MatchaOpcode == MatchaOpcode.StatusEffectList3)
            {
                var start = packet.MatchaOpcode == MatchaOpcode.StatusEffectList3 ? 0 : packet.MatchaOpcode == MatchaOpcode.StatusEffectList2 ? 24 : 20;
                if (packet.DataLength < start + 360 || packet.Source != packet.Target) return false;
                ObserveActor(packet.Target, packet.Timestamp);
                var updated = new HashSet<uint>();
                for (var offset = start; offset < start + 360; offset += 12)
                {
                    var status = packet.ReadUInt16(offset);
                    if (status == 0) continue;
                    updated.Add(status);
                    ObserveStatus(status, packet.ReadUInt16(offset + 2), packet.Timestamp);
                }
                foreach (var missing in statuses.Where(id => !updated.Contains(id)).ToArray())
                { statusStacks.Remove(missing); statuses.Remove(missing); }
                if (!updated.Contains(1803)) slap = 0;
                if (!updated.Contains(1804)) identical = 0;
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
                    ObserveStatus(packet.ReadUInt32(4), packet.ReadUInt32(8), packet.Timestamp);
                }

                if (category == 21)
                {
                    var lost = packet.ReadUInt32(4);
                    statuses.Remove(lost); statusStacks.Remove(lost);
                    if (lost == 1803) slap = 0;
                    if (lost == 1804) identical = 0;
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

                return HandleLog(packet.ReadUInt32(4), packet.ReadUInt32(12), packet.Timestamp);
            }

            if (packet.MatchaOpcode == MatchaOpcode.FishCaught)
            {
                if (packet.DataLength != 16 || packet.Target != actorId || cast == null || (phase != "reel" && phase != "hook"))
                {
                    return false;
                }

                Catch(packet.ReadUInt32(0), (int)packet.ReadUInt16(4), (int)(packet.ReadUInt16(6) & 255),
                    (packet.ReadUInt32(12) & 16) != 0, packet.Timestamp);
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
            if (capture != null)
            {
                var animation = packet.ReadUInt16(28);
                if (new uint[] { 4659, 4660, 4663, 4664, 4665, 4666, 12510, 12511, 12512, 12513, 12514, 12515 }.Contains(animation))
                {
                    capture.PatiencePrecision = new uint[] { 4660, 4665, 4666, 12511, 12514, 12515 }.Contains(animation);
                    capture.PatienceSuccess = animation < 12510;
                }
            }
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
                    var mooch = commandKnown ? (bool?)(pendingCommand == 3 || pendingCommand == 15) : null;
                    if (!baitId.HasValue)
                    {
                        var memory = readAttributes();
                        if (memory != null && Utils.Helper.Now - memory.SampledAt >= 0 && Utils.Helper.Now - memory.SampledAt <= 2000)
                            baitId = memory.BaitId;
                    }
                    capture = new FishingCapture { Slap = slap, Identical = identical,
                        Unsupported = !statusesKnown || statuses.Contains(3972) || statuses.Contains(3973) };
                    uint? effective = swimBait != 0 ? swimBait : taskActive ? (taskBait == 0 ? (uint?)null : taskBait) : baitId;
                    cast = new FishingDTO
                    {
                        CastId = Guid.NewGuid().ToString("N"),
                        CastTime = packet.Timestamp, BaseBaitId = baitId,
                        BaitId = mooch == false ? effective : null,
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
                        if (capture != null) capture.Terminal = true;
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

        public void ResetSession()
        {
            lock (sync)
            {
                cast = null; capture = null; phase = null; actorId = 0;
                baitId = placeId = pendingCommand = null; slap = identical = swimBait = taskBait = 0;
                taskActive = false; statuses.Clear(); statusStacks.Clear(); statusesKnown = false;
                Emit("reset", Utils.Helper.Now);
            }
        }

        private void ObserveStatus(uint status, uint stacks, long time)
        {
            bool changed = !statusStacks.TryGetValue(status, out var previous) || previous != stacks;
            statuses.Add(status); statusStacks[status] = stacks;
            if (!changed || capture == null || phase != "cast" || (status != 3972 && status != 3973)) return;
            if (stacks < 1 || stacks > 3) { capture.Unsupported = true; return; }
            capture.Lure = status == 3972 ? (int)stacks : 8 - (int)stacks;
            capture.LureTimes = capture.LureTimes.Concat(new[] { time }).ToArray();
        }

        private uint? EffectiveBait => swimBait != 0 ? swimBait : taskActive ? (taskBait == 0 ? (uint?)null : taskBait) : baitId;

        private bool HandleLog(uint message, uint parameter, long time)
        {
            if (message == 1110 || message == 1115)
            {
                placeId = parameter;
                if (cast != null && (phase == "cast" || message == 1115))
                {
                    cast.PlaceId = placeId;
                    if (message == 1110)
                    {
                        cast.Mooch = false; cast.BaitId = EffectiveBait;
                        capture.ServerCastTime = time;
                    }
                    Emit("place", time);
                }
            }
            else if (message == 1121 && cast != null && phase == "cast" && time - cast.CastTime <= 2000)
            {
                cast.Mooch = true; cast.BaitId = parameter; cast.PlaceId = placeId;
                capture.ServerCastTime = time; Emit("place", time);
            }
            else if (message >= 1111 && message <= 1113)
            {
                Emit("quit", time); cast = null; capture = null; phase = null; placeId = null; pendingCommand = null;
            }
            else if ((message >= 1117 && message <= 1120) || message == 1127 || message == 1129 || message == 3527)
            {
                if (cast != null && phase != "catch" && phase != "end")
                {
                    phase = "end"; capture.Terminal = true; capture.GatheringFailed = message == 3527;
                    Emit("end", time);
                }
            }
            else if (message == 5505) identical = parameter;
            else if (message == 5506) slap = parameter;
            else if (message == 11154) taskActive = true;
            else if (message == 10769) taskActive = false;
            else if (capture != null)
            {
                if (message == 5564)
                {
                    capture.Hidden = parameter;
                    var stacks = capture.Lure <= 3 ? capture.Lure : 8 - capture.Lure;
                    capture.HiddenStack = capture.LureTimes.Length > 0 && (Math.Abs(time - capture.LureTimes.Last()) < 800 || stacks >= 3) ? stacks : stacks + 1;
                    if (capture.HiddenStack < 1 || capture.HiddenStack > 3) capture.Unsupported = true;
                }
                // Keep a confirmed hidden-fish observation through its removal on catch.
                if (message == 5565 || message == 5569) capture.LureTarget = true;
            }
            return true;
        }
        private void Catch(uint fish, int size, int quantity, bool large, long time)
        {
            if (fish == 0 || cast == null || capture == null || phase == "catch") return;
            cast.FishId = fish; capture.Size = size; capture.Quantity = quantity; capture.Large = large;
            capture.Terminal = true; phase = "catch"; Emit("catch", time);
        }

        private void ObserveActor(uint id, long time)
        {
            if (actorId != 0 && actorId != id)
            {
                Emit("reset", time);
                cast = null;
                phase = null;
                baitId = placeId = pendingCommand = null;
                statuses.Clear(); statusStacks.Clear();
                statusesKnown = false;
                capture = null; slap = identical = swimBait = taskBait = 0; taskActive = false;
            }

            actorId = id;
        }

        private void Emit(string action, long time)
        {
            // Never publish the mutable instance retained by the handler.
            fireEvent(new FishingDTO
            {
                Action = action, Time = time,
                CastId = cast?.CastId, CastTime = cast?.CastTime, BiteTime = cast?.BiteTime,
                HookTime = cast?.HookTime, BaitId = cast?.BaitId,
                BaseBaitId = cast != null && action != "bait" ? cast.BaseBaitId : baitId,
                PlaceId = cast != null ? cast.PlaceId : placeId,
                Tug = cast?.Tug, FishId = cast?.FishId, Mooch = cast?.Mooch,
                Chum = cast?.Chum, Snagging = cast?.Snagging,
                Statuses = cast?.Statuses,
                Capture = capture?.Copy(),
            });
        }
    }
}
