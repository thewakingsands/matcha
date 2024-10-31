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
