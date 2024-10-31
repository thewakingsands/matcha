// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using Cafe.Matcha.DTO;

    internal abstract class AbstractHandler
    {
        protected Action<BaseDTO> fireEvent;
        protected AbstractHandler(Action<BaseDTO> fireEvent)
        {
            this.fireEvent = fireEvent;
        }

        public abstract bool Handle(Packet packet);
    }
}
