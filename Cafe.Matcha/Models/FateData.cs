// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using Newtonsoft.Json;

    public class FateData
    {
        [JsonProperty("name")]
        public ItemName Name;

        [JsonProperty("level")]
        public int Level;

        [JsonProperty("patch")]
        public int Patch;

        [JsonProperty("location")]
        public int Location;
    }
}
