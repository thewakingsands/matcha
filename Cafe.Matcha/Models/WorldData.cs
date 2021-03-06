// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using Newtonsoft.Json;

    public class WorldData
    {
        [JsonProperty("name")]
        public string LocalName;
        [JsonProperty("name_en")]
        public string EnglishName;
        [JsonProperty("dc")]
        public string LocalDataCenter;
        [JsonProperty("dc_en")]
        public string EnglishDataCenter;

        public override string ToString()
        {
            return LocalName;
        }
    }
}
