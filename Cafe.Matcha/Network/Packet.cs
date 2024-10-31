// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network
{
    using System;
    using System.Linq;
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
        /// Is this packet known.
        /// </summary>
        public bool Known = false;

        /// <summary>
        /// SegmentType.
        /// </summary>
        public byte SegmentType;

        /// <summary>
        /// Sender.
        /// </summary>
        public PacketSender Sender;

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
        /// Opcode.
        /// </summary>
        public MatchaOpcode MatchaOpcode;

        /// <summary>
        /// Gets packet length, including headers.
        /// </summary>
        public int Length => Bytes.Length;

        public int DataLength => Bytes.Length - HeaderLength;

        public Packet(PacketSender sender, byte[] bytes)
        {
            Sender = sender;
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

            Known = GetMatchaOpcode(out MatchaOpcode);
        }

        private bool GetMatchaOpcode(out MatchaOpcode matchaOpcode)
        {
            var key = Sender == PacketSender.Server ? Opcode : (ushort)(0x8000 | Opcode);
            var region = Config.Instance.Region;
            switch (region)
            {
                case Region.Global:
                    return OpcodeStorage.Global.TryGetValue(key, out matchaOpcode);

                case Region.China:
                    return OpcodeStorage.China.TryGetValue(key, out matchaOpcode);

                default:
                    matchaOpcode = default;
                    return false;
            }
        }

        public byte[] GetRawData()
        {
            return Bytes.Skip(HeaderLength).ToArray();
        }

        public uint ReadUInt32(int startIndex)
        {
            return BitConverter.ToUInt32(Bytes, startIndex + HeaderLength);
        }

        public enum PacketSender
        {
            Server,
            Client
        }
    }
}
