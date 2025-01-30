// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class QueueDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.Queue;
            }
        }

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("stage")]
        public string Stage;

        [JsonProperty("order")]
        public uint Order = 0;

        [JsonProperty("time")]
        public uint Time = 0;
    }
}
