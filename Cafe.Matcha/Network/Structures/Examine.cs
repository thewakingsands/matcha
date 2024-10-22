// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Structures
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// This class represents the "Result Dialog" packet. This is also used e.g. for reduction results, but we only care about tax rates.
    /// We can do that by checking the "Category" field.
    /// </summary>
    public class Examine
    {
        public class Gear
        {
            public uint ItemId { get; internal set; }
            public uint Glamour { get; internal set; }
            public bool HQ { get; internal set; }
            public List<Materia> Materias { get; internal set; }
        }

        public byte ClassJob { get; internal set; }
        public byte Level { get; internal set; }
        public ushort WorldId { get; internal set; }
        public List<Gear> Gears { get; internal set; }
        public string Name { get; internal set; }

        /// <summary>
        /// Read a <see cref="Examine"/> object from memory.
        /// </summary>
        /// <param name="data">Data to read.</param>
        /// <returns>A new <see cref="Examine"/> object.</returns>
        public static Examine Read(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var output = new Examine();

                    stream.Position += 2;
                    output.ClassJob = reader.ReadByte();
                    output.Level = reader.ReadByte();
                    stream.Position += 0x2E;
                    output.WorldId = reader.ReadUInt16();
                    stream.Position = 0x50;

                    output.Gears = new List<Gear>();
                    for (int i = 0; i < 14; i++)
                    {
                        var gear = new Gear();
                        gear.ItemId = reader.ReadUInt32();
                        gear.Glamour = reader.ReadUInt32();
                        stream.Position += 8;
                        gear.HQ = reader.ReadBoolean();
                        stream.Position += 1;

                        gear.Materias = new List<Materia>();
                        for (int j = 0; j < 5; j++)
                        {
                            var materia = new Materia();
                            materia.Type = reader.ReadUInt16();
                            materia.Tier = reader.ReadUInt16();
                            gear.Materias.Add(materia);
                        }

                        output.Gears.Add(gear);
                        stream.Position += 2;
                    }

                    output.Name = Encoding.UTF8.GetString(reader.ReadBytes(0x20)).TrimEnd('\u0000');
                    return output;
                }
            }
        }
    }
}
