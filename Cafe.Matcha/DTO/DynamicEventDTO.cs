// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class DynamicEventDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.DynamicEvent;
            }
        }

        [JsonProperty("zone")]
        public int Zone = 0;

        [JsonProperty("event")]
        public int Event;

        [JsonProperty("participants")]
        public int Participants = 0;

        [JsonProperty("stage")]
        public int Stage = 0;

        [JsonProperty("progress")]
        public int Progress = 0;

        [JsonProperty("nextStage")]
        public uint NextStage;

        [JsonProperty("countdown")]
        public int Countdown;
    }
}
