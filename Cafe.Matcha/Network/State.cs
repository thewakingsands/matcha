using Cafe.Matcha.Utils;

namespace Cafe.Matcha.Network
{
    class State : StaticBindingTarget<State>
    {
        public ushort WorldId { get; set; } = 0;
        public ushort ZoneId { get; set; } = 0;
    }
}
