// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal sealed class InitialDataSnapshot
    {
        public InitialDataSnapshot(string characterName, long receivedAt, IEnumerable<uint> fish,
            IEnumerable<uint> spearfish, string error = null)
        {
            CharacterName = characterName;
            ReceivedAt = receivedAt;
            Fish = Array.AsReadOnly(fish.Distinct().OrderBy(id => id).ToArray());
            Spearfish = Array.AsReadOnly(spearfish.Distinct().OrderBy(id => id).ToArray());
            Error = error;
        }

        public string CharacterName { get; }

        public long ReceivedAt { get; }

        public ReadOnlyCollection<uint> Fish { get; }

        public ReadOnlyCollection<uint> Spearfish { get; }

        public string Error { get; }

        public bool CanExport => Error == null;

        public string ToFishcakeJson()
        {
            if (!CanExport)
            {
                throw new InvalidOperationException(Error);
            }

            return new JObject
            {
                ["completed"] = new JArray(Fish.Concat(Spearfish).Distinct().OrderBy(id => id)),
                ["pinned"] = new JArray(),
                ["alarmFish"] = new JArray()
            }.ToString(Formatting.Indented);
        }

        public void SaveFishcake(string path)
        {
            File.WriteAllText(path, ToFishcakeJson(), new UTF8Encoding(false));
        }
    }
}
