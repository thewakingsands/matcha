// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using Newtonsoft.Json;

    public class ItemName
    {
        [JsonProperty("chs")]
        public string Chinese;
        /*
        [JsonProperty("en")]
        public string English;
        [JsonProperty("ja")]
        public string Japanese;
        [JsonProperty("de")]
        public string German;
        [JsonProperty("fr")]
        public string Franch;
        */

        public override string ToString()
        {
            return Chinese;
        }
    }
}
