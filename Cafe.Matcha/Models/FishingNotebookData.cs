// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public sealed class FishingNotebookData
    {
        [JsonProperty("fish")]
        public Dictionary<int, uint> Fish { get; set; }

        [JsonProperty("spearfish")]
        public Dictionary<int, uint> Spearfish { get; set; }

        [JsonIgnore]
        public bool IsValid => Fish != null && Fish.Count > 0 && Fish.Keys.All(id => id >= 0)
            && Spearfish != null && Spearfish.Count > 0 && Spearfish.Keys.All(id => id >= 20000);
    }
}
