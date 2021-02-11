// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Telemetry
{
    using System;
    using System.Linq;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    internal enum OpcodeEnum : ushort
    {
        None = 0,
        BossNpcSpawn,
        BossStatusEffectList,
        NpcSpawn,
    }

    internal class OpcodeDTO : Models.TelemetryData
    {
        public OpcodeDTO(OpcodeEnum type, ushort opcode, uint length, uint[] data) : this((ushort)type, opcode, length, data)
        { }

        public OpcodeDTO(ushort type, ushort opcode, uint length, uint[] data)
        {
            this.Type = type;
            this.Opcode = opcode;
            this.Length = length;
            this.Data = data;
        }

        [JsonProperty("type")]
        public ushort Type = 0;

        [JsonProperty("opcode")]
        public ushort Opcode = 0;

        [JsonProperty("length")]
        public uint Length = 0;

        [JsonProperty("data")]
        public uint[] Data;

        public bool Equals(OpcodeDTO opcode)
        {
            return base.Equals(opcode) && Type == opcode.Type && Opcode == opcode.Opcode && Length == opcode.Length &&
                Data.Length == opcode.Data.Length && Data.SequenceEqual(opcode.Data);
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType() && Equals((OpcodeDTO)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Type.GetHashCode() ^ Opcode.GetHashCode() ^ Length.GetHashCode() ^ Data.GetHashCode();
        }

        public override string ToString()
        {
#if DEBUG
            string type = Enum.GetName(typeof(OpcodeEnum), Type) ?? Type.ToString();
            return $"Opcode {{ Type={type}, Opcode={Opcode:x4}, Length={Length} }}";
#else
            return $"Opcode {{ Type={Type}, Opcode={Opcode:x4}, Length={Length} }}";
#endif
        }
    }

    internal class Opcode : TelemetryWorker<OpcodeDTO>
    {
        public Opcode() : base("143e0f60-6130-11ea-86c6-a57a0cd1d01e") { }

        public void Send(OpcodeEnum type, ushort opcode, uint length, uint[] data)
        {
            Send(new OpcodeDTO(type, opcode, length, data));
        }

        public void Send(ushort type, ushort opcode, uint length, uint[] data)
        {
            Send(new OpcodeDTO(type, opcode, length, data));
        }
    }
}
