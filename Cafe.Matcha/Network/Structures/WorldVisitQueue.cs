// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Structures
{
    using System.IO;

    public class WorldVisitQueue
    {
        public uint Stage { get; internal set; }
        public uint Order { get; internal set; }
        public uint Time { get; internal set; }

        /// <summary>
        /// Read a <see cref="WorldVisitQueue"/> object from memory.
        /// </summary>
        /// <param name="data">Data to read.</param>
        /// <returns>A new <see cref="WorldVisitQueue"/> object.</returns>
        public static WorldVisitQueue Read(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var output = new WorldVisitQueue();
                    output.Stage = reader.ReadUInt32();
                    output.Order = reader.ReadUInt32();
                    output.Time = reader.ReadUInt32();
                    return output;
                }
            }
        }
    }
}
