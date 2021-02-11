// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class FateDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.Fate;
            }
        }

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("fate")]
        public int Fate;

        [JsonProperty("progress")]
        public int Progress = 0;

        [JsonProperty("extra")]
        public int Extra = 0;
    }
}
