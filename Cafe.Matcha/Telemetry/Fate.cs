// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Telemetry
{
    using Cafe.Matcha.Models;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    internal class FateDTO : TelemetryData
    {
        public FateDTO(uint fateId, Network.FateState state)
        {
            this.FateId = fateId;
            this.StartTime = state.StartTime;
            this.Duration = state.Duration;
            this.Progress = state.Progress;
        }

        [JsonProperty("fate")]
        public uint FateId = 0;

        [JsonProperty("start_time")]
        public uint StartTime = 0;

        [JsonProperty("duration")]
        public uint Duration = 0;

        [JsonProperty("progress")]
        public int Progress = 0;

        public bool Equals(FateDTO fateInit)
        {
            return base.Equals(fateInit) && FateId == fateInit.FateId && StartTime == fateInit.StartTime
                && Duration == fateInit.Duration && Progress == fateInit.Progress;
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

            return obj.GetType() == GetType() && Equals((FateDTO)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ FateId.GetHashCode() ^ StartTime.GetHashCode() ^ Duration.GetHashCode() ^ Progress.GetHashCode();
        }

        public override string ToString()
        {
            return $"Fate {{ Id={FateId}, StartTime={StartTime}, Duration={Duration}, Progress={Progress} }} {base.ToString()}";
        }

        public override bool TryMerge(TelemetryData data)
        {
            if (data is FateDTO fate && fate.FateId == FateId)
            {
                Merge(data);

                if (fate.StartTime != 0)
                {
                    StartTime = fate.StartTime;
                }

                if (fate.Duration != 0)
                {
                    Duration = fate.Duration;
                }

                if (fate.Progress != 0)
                {
                    Progress = fate.Progress;
                }

                return true;
            }

            return false;
        }
    }

    internal class Fate : TelemetryWorker<FateDTO>
    {
        public Fate() : base(Constant.Secret.TelemetryFate) { }

        public void Send(uint fateId, Network.FateState state)
        {
            Send(new FateDTO(fateId, state));
        }
    }
}
