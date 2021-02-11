// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class InitZoneDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.InitZone;
            }
        }

        [JsonProperty("zone")]
        public int Zone;

        [JsonProperty("instance")]
        public int Instance;
    }
}
