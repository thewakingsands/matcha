// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Telemetry
{
    using Cafe.Matcha.Models;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    internal class NpcDTO : Models.TelemetryData
    {
        public NpcDTO(uint id, Network.NpcState state) : base()
        {
            this.CharacterID = id;
            this.BNpcName = state.BNpcName;
            this.Fate = state.Fate;
            this.X = state.Position.X;
            this.Y = state.Position.Y;
            this.Z = state.Position.Z;
            this.Level = state.Level;
            this.CurHP = state.CurHP;
            this.MaxHP = state.MaxHP;
            this.Location = state.Location;
        }

        [JsonProperty("cid")]
        public uint CharacterID = 0;

        [JsonProperty("npc")]
        public uint BNpcName = 0;

        [JsonProperty("fate")]
        public uint Fate = 0;

        [JsonProperty("x")]
        public float X = 0;

        [JsonProperty("y")]
        public float Y = 0;

        [JsonProperty("z")]
        public float Z = 0;

        [JsonProperty("level")]
        public ushort Level = 0;

        [JsonProperty("cur_hp")]
        public uint CurHP = 0;

        [JsonProperty("max_hp")]
        public uint MaxHP = 0;

        [JsonProperty("location")]
        public uint Location = 0;

        public bool Equals(NpcDTO npc)
        {
            return base.Equals(npc) && BNpcName == npc.BNpcName &&
                Fate == npc.Fate && Level == npc.Level &&
                CurHP == npc.CurHP && MaxHP == npc.MaxHP &&
                Location == npc.Location;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((NpcDTO)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ BNpcName.GetHashCode() ^ Fate.GetHashCode() ^ Level.GetHashCode()
                ^ CurHP.GetHashCode() ^ MaxHP.GetHashCode() ^ Location.GetHashCode();
        }

        public override string ToString()
        {
            return $"NpcSpawn {{ Id={BNpcName}, Zone={Zone}, Fate={Fate}, Level={Level}, HP={CurHP}/{MaxHP}, X={X}, Y={Y}, Z={Z}, Location={Location} }} {base.ToString()}";
        }

        public override bool TryMerge(TelemetryData data)
        {
            if (data is NpcDTO npc && npc.CharacterID == CharacterID)
            {
                Merge(data);

                if (npc.BNpcName != 0)
                {
                    BNpcName = npc.BNpcName;
                }

                if (npc.Fate != 0)
                {
                    Fate = npc.Fate;
                }

                if (npc.Level != 0)
                {
                    Level = npc.Level;
                }

                if (npc.X != 0)
                {
                    X = npc.X;
                }

                if (npc.Y != 0)
                {
                    Y = npc.Y;
                }

                if (npc.Z != 0)
                {
                    Z = npc.Z;
                }

                if (npc.CurHP != 0)
                {
                    CurHP = npc.CurHP;
                }

                if (npc.MaxHP != 0)
                {
                    MaxHP = npc.MaxHP;
                }

                if (npc.Location != 0)
                {
                    Location = npc.Location;
                }

                return true;
            }

            return false;
        }
    }

    internal class Npc : TelemetryWorker<NpcDTO>
    {
        public Npc() : base(Constant.Secret.TelemetryNpc) { }

        public void Send(uint id, Network.NpcState state)
        {
            Send(new NpcDTO(id, state));
        }
    }
}