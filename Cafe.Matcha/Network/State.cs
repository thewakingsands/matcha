namespace Cafe.Matcha.Network
{
    using Cafe.Matcha.Utils;

    internal class State : StaticBindingTarget<State>
    {
        private ushort worldId = 0;
        private ushort zoneId = 0;

        public ushort InstanceId { get; set; } = 0;

        public ushort ServerId { get; set; } = 0;

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

        public ushort ZoneId
        {
            get
            {
                if (zoneId == 0 && ParsePlugin.Instance != null)
                {
                    return (ushort)ParsePlugin.Instance.GetCurrentTerritoryID();
                }

                return zoneId;
            }
            set
            {
                zoneId = value;
            }
        }
    }
}
