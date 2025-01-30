// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Network.Structures;

    internal class QueueHandler : AbstractHandler
    {
        public QueueHandler(Action<BaseDTO> fireEvent) : base(fireEvent)
        {
        }

        public override bool Handle(Packet packet)
        {
            if (packet.MatchaOpcode == MatchaOpcode.WorldVisitQueue)
            {
                var data = WorldVisitQueue.Read(packet.GetRawData());
                fireEvent(new QueueDTO()
                {
                    Type = "world-visit",
                    Stage = data.Stage == 1 ? "waiting" : data.Stage == 2 ? "ready" : data.Stage == 3 ? "done" : "unknown",
                    Order = data.Order,
                    Time = data.Time,
                });

                return true;
            }

            return false;
        }
    }
}
