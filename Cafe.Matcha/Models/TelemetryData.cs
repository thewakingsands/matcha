// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using System;
    using Newtonsoft.Json;

    internal abstract class TelemetryData
    {
        public TelemetryData()
        {
            ClientId = Config.Instance.Telemetry.UUID;
            Version = Data.Version;
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            World = Network.State.Instance.WorldId;
            Server = Network.State.Instance.ServerId;
            Zone = Network.State.Instance.ZoneId;
            Instance = Network.State.Instance.InstanceId;
        }

        [JsonProperty("world")]
        public ushort World = 0;

        [JsonProperty("server")]
        public ushort Server = 0;

        [JsonProperty("zone")]
        public ushort Zone = 0;

        [JsonProperty("instance")]
        public ushort Instance = 0;

        [JsonProperty("client_id")]
        public string ClientId;

        [JsonProperty("version")]
        public string Version;

        [JsonProperty("date")]
        public string Date;

        [JsonProperty("timestamp")]
        public long Timestamp;

        public bool Equals(TelemetryData fateInit)
        {
            return Server == fateInit.Server && Zone == fateInit.Zone;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((TelemetryData)obj);
        }

        public override int GetHashCode()
        {
            return Server.GetHashCode() ^ Zone.GetHashCode() ^ Timestamp.GetHashCode();
        }

        public override string ToString()
        {
            return $"TData {{ World={World}, Server={Server}, Zone={Zone}, Instance={Instance} }} {base.ToString()}";
        }

        protected void Merge(TelemetryData data)
        {
            World = data.World;
            Server = data.Server;
            Zone = data.Zone;
            Instance = data.Instance;
        }

        public abstract bool TryMerge(TelemetryData data);
    }
}
