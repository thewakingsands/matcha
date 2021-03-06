// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using System.Collections.Generic;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    public class Template : BindingTarget
    {
        public string LocalName
        {
            get
            {
                return Name.ToString();
            }
        }

        [JsonProperty("name")]
        public ItemName Name { get; set; }

        [JsonProperty("fate")]
        public List<int> Fates = new List<int>();
    }
}
