// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Structures
{
    using Newtonsoft.Json;
    public class Materia
    {
        [JsonProperty("type")]
        public int Type { get; internal set; }

        [JsonProperty("tier")]
        public int Tier { get; internal set; }
    }
}
