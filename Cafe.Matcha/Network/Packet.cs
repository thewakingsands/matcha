namespace Cafe.Matcha.Network
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Cafe.Matcha.Constant;

    internal class Packet
    {
        private const int HeaderLength = 32;

        /// <summary>
        /// Raw packet, including headers.
        /// </summary>
        public byte[] Bytes;

        /// <summary>
        /// Is this packet valid.
        /// </summary>
        public bool Valid = false;

        /// <summary>
        /// SegmentType.
        /// </summary>
        public byte SegmentType;

        /// <summary>
        /// Source.
        /// </summary>
        public uint Source;

        /// <summary>
        /// Target.
        /// </summary>
        public uint Target;

        /// <summary>
        /// Opcode.
        /// </summary>
        public ushort Opcode;

        /// <summary>
        /// Gets packet length, including headers.
        /// </summary>
        public int Length => Bytes.Length;

        public int DataLength => Bytes.Length - HeaderLength;

        public Packet(byte[] bytes)
        {
            Bytes = bytes;
            if (bytes.Length < HeaderLength)
            {
                return;
            }

            // Deucalion gives wrong type (0)
            SegmentType = bytes[12];
            if (SegmentType != 0 && SegmentType != 3)
            {
                return;
            }

            Source = BitConverter.ToUInt32(Bytes, 4);
            Target = BitConverter.ToUInt32(Bytes, 8);
            Opcode = BitConverter.ToUInt16(Bytes, 18);
            Valid = true;
        }

        public bool GetMatchaOpcode(out MatchaOpcode matchaOpcode)
        {
            var region = Config.Instance.Region;
            switch (region)
            {
                case Region.Global:
                    return OpcodeStorage.Global.TryGetValue(Opcode, out matchaOpcode);

                case Region.China:
                    return OpcodeStorage.China.TryGetValue(Opcode, out matchaOpcode);

                default:
                    matchaOpcode = default;
                    return false;
            }
        }

        public byte[] GetRawData()
        {
            return Bytes.Skip(HeaderLength).ToArray();
        }
    }
}
