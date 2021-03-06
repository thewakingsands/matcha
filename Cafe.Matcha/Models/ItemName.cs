// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using Newtonsoft.Json;

    public class ItemName
    {
        [JsonProperty("chs")]
        public string Chinese = null;
        [JsonProperty("en")]
        public string English = null;
        [JsonProperty("ja")]
        public string Japanese = null;
        [JsonProperty("de")]
        public string German = null;
        [JsonProperty("fr")]
        public string French = null;

        public override string ToString()
        {
            switch (Config.Instance.Language)
            {
                case FFXIV_ACT_Plugin.Common.Language.French:
                    return French ?? English;
                case FFXIV_ACT_Plugin.Common.Language.German:
                    return German ?? English;
                case FFXIV_ACT_Plugin.Common.Language.Japanese:
                    return Japanese ?? English;
                case FFXIV_ACT_Plugin.Common.Language.Chinese:
                    return Chinese ?? English;
                default:
                    return English;
            }
        }
    }
}
