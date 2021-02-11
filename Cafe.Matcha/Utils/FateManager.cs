// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    internal class FateManager
    {
        public static List<int> Load(string fileName)
        {
            try
            {
                string content = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<List<int>>(content);
            }
            catch
            {
                return null;
            }
        }

        public static void Save(string fileName, IList<int> fates)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(fates, Formatting.Indented));
        }
    }
}
