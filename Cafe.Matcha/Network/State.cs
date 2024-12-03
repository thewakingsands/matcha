// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using Cafe.Matcha.Telemetry;
    using Cafe.Matcha.Utils;

    internal class FateState
    {
        public uint StartTime;
        public uint Duration;
        public int Progress;
    }

    internal class NpcState
    {
        public uint BNpcName;
        public uint Location;
        public uint Fate;
        public ushort Level;
        public uint CurHP;
        public uint MaxHP;
        public Vector3 Position;
    }

    internal class State : StaticBindingTarget<State>
    {
        private ushort worldId = 0;
        private ushort zoneId = 0;

        public long LastZoneChange { get; private set; } = 0;

        public ushort InstanceId { get; private set; } = 0;

        public ushort ServerId { get; private set; } = 0;

        public ushort ContentId { get; private set; } = 0;

        public uint FishingBait { get; set; } = 0;

        public readonly StateManager<FateState> Fate = new StateManager<FateState>();
        public readonly StateManager<NpcState> Npc = new StateManager<NpcState>();

        private Fate fateTelemetry = new Fate();
        private Npc npcTelemetry = new Npc();

        public ushort WorldId
        {
            get
            {
                if (worldId == 0 && ParsePlugin.Instance != null)
                {
                    return (ushort)ParsePlugin.Instance.GetServer();
                }

                return worldId;
            }

            private set
            {
                worldId = value;
            }
        }

        public ushort ZoneId
        {
            get
            {
                if (zoneId == 0 && ParsePlugin.Instance != null)
                {
                    return (ushort)ParsePlugin.Instance.GetCurrentTerritoryID();
                }

                return zoneId;
            }
            private set
            {
                zoneId = value;
            }
        }

        public State()
        {
            Fate.OnChanged += Fate_OnChanged;
            Fate.OnRemoved += Fate_OnRemoved;
            Npc.OnChanged += Npc_OnChanged;
            Npc.OnRemoved += Npc_OnRemoved;
        }

        private void Fate_OnChanged(uint id, FateState state)
        {
            fateTelemetry.Send(id, state);
        }

        private void Fate_OnRemoved(uint id, FateState state)
        {
            // Emit only on progress=100 or expired
            if (state.Progress == 100 || IsFateNearEnd(state))
            {
                state.Progress = -1;
                fateTelemetry.Send(id, state);
            }
        }

        private bool IsFateNearEnd(FateState state)
        {
            if (state.StartTime == 0 || state.Duration == 0)
            {
                return false;
            }

            return Helper.Now / 1000 + 30 > state.StartTime + state.Duration;
        }

        private void Npc_OnChanged(uint id, NpcState state)
        {
            npcTelemetry.Send(id, state);
        }

        private void Npc_OnRemoved(uint id, NpcState state)
        {
            state.CurHP = 0;
            npcTelemetry.Send(id, state);
        }

        public void HandleInitZone(ushort serverId, ushort zoneId, ushort instanceId, ushort contentId)
        {
            ServerId = serverId;
            ZoneId = zoneId;
            InstanceId = instanceId;
            ContentId = contentId;

            LastZoneChange = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Fate.Clear();
            Npc.Clear();

            Log.Info(Constant.LogType.State, $"InitZone: server={serverId}, zone={zoneId}, instance={instanceId}, time={LastZoneChange}");
        }

        public void HandleWorldId(ushort worldId, bool isCurrentPlayer)
        {
            if ((ContentId == 0 || isCurrentPlayer) && this.worldId != worldId)
            {
                Log.Info(Constant.LogType.State, $"WorldId: {worldId}");
                WorldId = worldId;
            }
        }

        internal class StateManager<T> where T : new()
        {
            private readonly Dictionary<uint, T> store = new Dictionary<uint, T>();
            private readonly object storeLock = new object();

            public void Update(uint id, Func<T, bool> action)
            {
                bool isCreate = false;
                T state;
                lock (storeLock)
                {
                    if (!store.TryGetValue(id, out state))
                    {
                        isCreate = true;
                        state = new T();
                        store.Add(id, state);
                    }
                }

                if (action(state) || isCreate)
                {
#if DEBUG
                    Log.Debug(Constant.LogType.State, $"<{typeof(T).Name}> Emitting OnChanged for Id={id}, isCreate={isCreate}, {state}");
#endif
                    OnChanged?.Invoke(id, state);
                }
            }

            public T this[uint id]
            {
                get { return store[id]; }
            }

            public bool Contains(uint id)
            {
                return store.ContainsKey(id);
            }

            public void Remove(uint id)
            {
                if (store.TryGetValue(id, out var state))
                {
                    store.Remove(id);
                    OnRemoved?.Invoke(id, state);
                }
            }

            public void Clear()
            {
                store.Clear();
            }

            public delegate void EventHandler(uint id, T state);
            public event EventHandler OnChanged;
            public event EventHandler OnRemoved;
        }
    }
}
