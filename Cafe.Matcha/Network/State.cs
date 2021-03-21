namespace Cafe.Matcha.Network
{
    using Cafe.Matcha.Utils;

    internal class State : StaticBindingTarget<State>
    {
        private ushort worldId = 0;
        public ushort WorldId
        {
            get
            {
                if (worldId == 0 && ParsePlugin.Instance != null)
                {
                    return (ushort)ParsePlugin.Instance.GetServer();
                }

                return worldId;
            }

            set
            {
                worldId = value;
            }
        }

        public ushort ZoneId { get; set; } = 0;
    }
}
