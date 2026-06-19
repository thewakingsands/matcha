// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using System.Collections.Generic;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Utils;

    internal class SubmarineHandler : AbstractHandler
    {
        private const int SubmarineCount = 4;
        private const int CompanySubmersibleItemLength = 36;
        private const int CompanySubmersiblePacketLength = 176;
        private const int SubmarineStatusItemLength = 60;
        private const int SubmarineStatusPacketDataLength = SubmarineStatusItemLength * SubmarineCount;

        public SubmarineHandler(Action<BaseDTO> fireEvent) : base(fireEvent)
        {
        }

        public override bool Handle(Packet packet)
        {
            if (packet.MatchaOpcode == MatchaOpcode.CompanySubmersibleStatus)
            {
                return HandleCompanySubmersibleStatus(packet);
            }

            if (packet.MatchaOpcode == MatchaOpcode.SubmarineStatusList)
            {
                return HandleSubmarineStatusList(packet);
            }

            return false;
        }

        private bool HandleCompanySubmersibleStatus(Packet packet)
        {
            if (packet.Length != CompanySubmersiblePacketLength)
            {
                return false;
            }

            var data = packet.GetRawData();
            var list = new List<CompanyVoyageStatusItem>();
            for (int i = 0; i < SubmarineCount; ++i)
            {
                var offset = i * CompanySubmersibleItemLength;
                list.Add(new CompanyVoyageStatusItem
                {
                    ReturnTime = BitConverter.ToUInt32(data, offset),
                    MaxDistance = BitConverter.ToUInt16(data, offset + 4),
                    Name = Helper.ReadString(data, offset + 8, 22),
                    Destination = new uint[]
                    {
                        data[offset + 31],
                        data[offset + 32],
                        data[offset + 33],
                        data[offset + 34],
                        data[offset + 35]
                    }
                });
            }

            fireEvent(new CompanyVoyageStatusDTO()
            {
                Type = "submersible",
                List = list
            });

            return true;
        }

        private bool HandleSubmarineStatusList(Packet packet)
        {
            if (packet.DataLength != SubmarineStatusPacketDataLength)
            {
                return false;
            }

            var data = packet.GetRawData();
            for (int i = 0; i < SubmarineCount; ++i)
            {
                var offset = i * SubmarineStatusItemLength;
                fireEvent(new SubmarineStatusDTO()
                {
                    Index = i,
                    Status = BitConverter.ToUInt16(data, offset),
                    Rank = BitConverter.ToUInt16(data, offset + 2),
                    Birthdate = BitConverter.ToUInt32(data, offset + 4),
                    ReturnTime = BitConverter.ToUInt32(data, offset + 8),
                    CurrentExp = BitConverter.ToUInt32(data, offset + 12),
                    TotalExpForNextRank = BitConverter.ToUInt32(data, offset + 16),
                    Capacity = BitConverter.ToUInt16(data, offset + 20),
                    Name = Helper.ReadString(data, offset + 22, 20),
                    Hull = BitConverter.ToUInt16(data, offset + 46),
                    Stern = BitConverter.ToUInt16(data, offset + 48),
                    Bow = BitConverter.ToUInt16(data, offset + 50),
                    Bridge = BitConverter.ToUInt16(data, offset + 52),
                    Destination = new uint[]
                    {
                        data[offset + 54],
                        data[offset + 55],
                        data[offset + 56],
                        data[offset + 57],
                        data[offset + 58]
                    }
                });
            }

            return true;
        }
    }
}
