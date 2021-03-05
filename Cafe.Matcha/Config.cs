// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha
{
    using System.IO;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;
    using ThomasJaworski.ComponentModel;

    public sealed class Config : BindingTarget
    {
        private static string configPath = Path.Combine(Helper.GetConfigDir(), "Cafe.Matcha.config");
        public static Models.ConfigData Instance { get; private set; } = new Models.ConfigData();
        public static void Load()
        {
            try
            {
                string content = File.ReadAllText(configPath);
                Instance = JsonConvert.DeserializeObject<Models.ConfigData>(content);
            }
            catch { }

            var listener = ChangeListener.Create(Instance);
            listener.PropertyChanged += (_, e) => Save();
            listener.CollectionChanged += (_, e) => Save();
        }

        public static void Save()
        {
            File.WriteAllText(configPath, JsonConvert.SerializeObject(Instance, Formatting.Indented));
        }

        public static string GetLanguageString()
        {
            switch (Instance.Language)
            {
                case FFXIV_ACT_Plugin.Common.Language.Chinese:
                    return "zh";
                case FFXIV_ACT_Plugin.Common.Language.English:
                    return "en";
                case FFXIV_ACT_Plugin.Common.Language.French:
                    return "fr";
                case FFXIV_ACT_Plugin.Common.Language.German:
                    return "de";
                case FFXIV_ACT_Plugin.Common.Language.Japanese:
                    return "ja";
                default:
                    return "en";
            }
        }
    }
}
