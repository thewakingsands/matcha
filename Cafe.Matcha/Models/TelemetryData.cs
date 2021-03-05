// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using System;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    internal class TelemetryData
    {
        public TelemetryData()
        {
            ClientId = Config.Instance.UUID;
            Version = Data.Version;
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Server = Network.State.WorldId;
            Zone = Network.State.ZoneId;
        }

        [JsonProperty("server")]
        public uint Server = 0;

        [JsonProperty("zone")]
        public uint Zone = 0;

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
    }
}
