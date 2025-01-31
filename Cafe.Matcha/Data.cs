// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    public class Data : StaticBindingTarget<Data>
    {
#if GLOBAL
        public const string Title = "Matcha";
#else
        public const string Title = "抹茶 Matcha";
#endif
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public Dictionary<int, Models.FateData> Fates { get; set; }
        public Dictionary<int, Models.DynamicEventData> DynamicEvents;
        public Dictionary<int, Models.InstanceData> Instances;
        public Dictionary<int, Models.ItemName> InstanceTypes;
        public Dictionary<int, Models.ItemName> Territories;
        public Dictionary<int, Models.ItemName> Patches;
        public Dictionary<int, Models.WorldData> Worlds;
        public List<Models.Template> Templates { get; set; }
        public Dictionary<int, Models.ItemName> Roulettes;

        private bool ReadData<T>(string path, string file, out T dict) where T : new()
        {
            try
            {
                string content = File.ReadAllText(Path.Combine(path, file));
                dict = JsonConvert.DeserializeObject<T>(content);
                return true;
            }
            catch
            {
                MessageBox.Show(string.Format("无法找到数据文件 {0} 或读取时发生错误，请检查是否对插件目录进行过修改，或尝试重新安装本插件。", file), Data.Title);
                dict = new T();
                return false;
            }
        }

        public void Init()
        {
            var dataRoot = Path.Combine(Helper.GetPluginDir(), "data");

            ReadData(dataRoot, "instance.json", out Instances);
            ReadData(dataRoot, "type.json", out InstanceTypes);
            ReadData(dataRoot, "territory.json", out Territories);
            ReadData(dataRoot, "roulette.json", out Roulettes);
            ReadData(dataRoot, "patch.json", out Patches);
            ReadData(dataRoot, "world.json", out Worlds);
            ReadData(dataRoot, "dynamic-event.json", out DynamicEvents);

            ReadData(dataRoot, "fate.json", out Dictionary<int, Models.FateData> fates);
            Fates = fates;
            ReadData(dataRoot, "template.json", out List<Models.Template> templates);
            Templates = templates;

            IsLoaded = true;
            DataLoaded?.Invoke(this, EventArgs.Empty);
        }

        public bool IsLoaded = false;
        public event EventHandler DataLoaded;
    }
}
