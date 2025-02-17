// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Utils;

    internal class FishingHandler : AbstractHandler
    {
        public FishingHandler(Action<BaseDTO> fireEvent) : base(fireEvent)
        {
        }

        public override bool Handle(Packet packet)
        {
            // Bite
            if (packet.MatchaOpcode == MatchaOpcode.EventPlay)
            {
                if (packet.Length != 72)
                {
                    return false;
                }

                var targetActorId = packet.Target;
                var fishActorId = packet.ReadUInt32(0);

                if (targetActorId != fishActorId)
                {
                    return true;
                }

                var type = (FishEventType)packet.ReadUInt16(12);
                var biteType = packet.ReadUInt16(28);

                if (type != FishEventType.Bite)
                {
                    return true;
                }

                var biteTypeParsed = ((FishEventBiteType)biteType) switch
                {
                    FishEventBiteType.Light => 1,
                    FishEventBiteType.Medium => 2,
                    FishEventBiteType.Big => 3,
                    _ => 0,
                };

                if (biteTypeParsed != 0)
                {
                    fireEvent(new FishBiteDTO()
                    {
                        Time = Helper.Now,
                        Type = 3
                    });
                }

                return true;
            }

            return false;
        }
    }
}
