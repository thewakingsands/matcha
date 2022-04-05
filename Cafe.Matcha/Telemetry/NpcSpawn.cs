namespace Cafe.Matcha.Telemetry
{
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    class NpcSpawnDTO : Models.TelemetryData
    {
        static float FromUShort(ushort pos)
        {
            return (pos - 0x8000) / 32.767f;
        }

        public NpcSpawnDTO(uint BNpcName, uint Fate, float X, float Y, float Z, ushort Level, uint HP) : base()
        {
            this.BNpcName = BNpcName;
            this.Fate = Fate;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Level = Level;
            this.HP = HP;
        }

        public NpcSpawnDTO(uint BNpcName, uint Fate, ushort X, ushort Y, ushort Z, ushort Level, uint HP) :
            this(BNpcName, Fate, FromUShort(X), FromUShort(Y), FromUShort(Z), Level, HP)
        { }

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

        [JsonProperty("hp")]
        public uint HP = 0;

        public bool Equals(NpcSpawnDTO npcSpawn)
        {
            return base.Equals(npcSpawn) && BNpcName == npcSpawn.BNpcName &&
                Fate == npcSpawn.Fate && Level == npcSpawn.Level && HP == npcSpawn.HP;
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

            return obj.GetType() == GetType() && Equals((NpcSpawnDTO)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ BNpcName.GetHashCode() ^ Fate.GetHashCode() ^ Level.GetHashCode() ^ HP.GetHashCode();
        }

        public override string ToString()
        {
            return $"NpcSpawn {{ Id={BNpcName}, Zone={Zone}, Fate={Fate}, Level={Level}, HP={HP}, X={X}, Y={Y}, Z={Z} }} {base.ToString()}";
        }
    }

    class NpcSpawn : TelemetryWorker<NpcSpawnDTO>
    {
        public NpcSpawn() : base(Constant.Secret.TelemetryNpc) { }

        public void Send(uint BNpcName, uint Fate, float X, float Y, float Z, ushort Level, uint HP)
        {
            Send(new NpcSpawnDTO(BNpcName, Fate, X, Y, Z, Level, HP));
        }

        public void Send(uint BNpcName, uint Fate, ushort X, ushort Y, ushort Z, ushort Level, uint HP)
        {
            Send(new NpcSpawnDTO(BNpcName, Fate, X, Y, Z, Level, HP));
        }
    }
}