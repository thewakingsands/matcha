// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Telemetry
{
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    internal class FateInitDTO : Models.TelemetryData
    {
        public FateInitDTO(uint fateId)
        {
            this.FateId = fateId;
        }

        [JsonProperty("fate")]
        public uint FateId = 0;

        public bool Equals(FateInitDTO fateInit)
        {
            return base.Equals(fateInit) && FateId == fateInit.FateId;
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

            return obj.GetType() == GetType() && Equals((FateInitDTO)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ FateId.GetHashCode();
        }

        public override string ToString()
        {
            return $"Fate {{ Id={FateId}, Zone={Zone} }} {base.ToString()}";
        }
    }

    internal class Fate : TelemetryWorker<FateInitDTO>
    {
        public Fate() : base(Constant.Secret.TelemetryFate) { }

        public void Send(uint fateId)
        {
            Send(new FateInitDTO(fateId));
        }
    }
}
