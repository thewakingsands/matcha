// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using Newtonsoft.Json;

    public class WorldData
    {
        [JsonProperty("chs")]
        public string Chinese;
        [JsonProperty("en")]
        public string English;
        [JsonProperty("dc_chs")]
        public string DataCenterChinese;
        [JsonProperty("dc_en")]
        public string DataCenterEnglish;

        public override string ToString()
        {
            return Chinese;
        }
    }
}
