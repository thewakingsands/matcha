// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;

    internal class TreasureHandler : AbstractHandler
    {
        public TreasureHandler(Action<BaseDTO> fireEvent) : base(fireEvent)
        {
        }

        public override bool Handle(Packet packet)
        {
            if (packet.MatchaOpcode == MatchaOpcode.ActorControlSelf)
            {
                return HandleActorControlSelf(packet);
            }

            if (packet.Known)
            {
                return false;
            }

            // Treasure shifting wheel result.
            if (packet.DataLength == 56)
            {
                var data = packet.GetRawData();
                var level = BitConverter.ToUInt32(data, 24);
                if (
                    level == 7636061 || // G10 运河宝物库神殿
                    level == 8508181 || // G12 梦羽宝殿
                    level == 9413549) // G15 育体宝殿
                {
                    var result = (TreasureShiftingWheelResultType)data[40];
                    switch (result)
                    {
                        case TreasureShiftingWheelResultType.Low:
                            fireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-low"
                            });
                            break;
                        case TreasureShiftingWheelResultType.Medium:
                            fireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-medium"
                            });
                            break;
                        case TreasureShiftingWheelResultType.High:
                            fireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-high"
                            });
                            break;
                        case TreasureShiftingWheelResultType.Shift:
                            fireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-shift"
                            });
                            break;
                        case TreasureShiftingWheelResultType.Special:
                            fireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-special"
                            });
                            break;
                        case TreasureShiftingWheelResultType.End:
                            fireEvent(new TreasureResultDTO()
                            {
                                Value = "wheel-end"
                            });
                            break;
                    }

                    return true;
                }
            }
            else if (packet.DataLength == 64)
            {
                var data = packet.GetRawData();
                var flag = BitConverter.ToUInt32(data, 16);
                if (flag == 0x04482c03)
                {
                    fireEvent(new TreasureResultDTO()
                    {
                        Round = data[32] + 1,
                        Value = data[40] == 1 ? "gate-open" : "gate-fail"
                    });
                    return true;
                }
            }

            return false;
        }

        private bool HandleActorControlSelf(Packet packet)
        {
            if (packet.Length != 72)
            {
                return false;
            }

            var data = packet.GetRawData();
            var type = (ActorControlType)BitConverter.ToUInt16(data, 0);
            if (type != ActorControlType.TreasureSpot)
            {
                return false;
            }

            fireEvent(new TreasureSpotDTO()
            {
                Item = (int)BitConverter.ToUInt32(data, 4),
                Location = (int)BitConverter.ToUInt32(data, 8),
                IsNew = BitConverter.ToUInt32(data, 12) != 0
            });
            return true;
        }
    }
}