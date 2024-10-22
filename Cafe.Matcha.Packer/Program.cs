// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Packer
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;

    public class Program
    {
        private static void GenerateAssembly()
        {
            string version = DateTime.UtcNow.ToString("yy.M.d.Hmm");

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Cafe.Matcha\AssemblyCopyright.cs");
            var template = @"// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

using System.Reflection;

[assembly: AssemblyTitle(""Cafe.Matcha"")]
[assembly: AssemblyDescription(""Cafe.Matcha"")]
[assembly: AssemblyCompany(""FFCafe"")]
[assembly: AssemblyVersion(""{0}"")]
[assembly: AssemblyCopyright(""Copyright © FFCafe {1}"")]

namespace Cafe.Matcha
{
    public partial class Data
    {
        public const string Version = ""{0}"";
    }
}
";
            var content = template.Replace("{0}", version).Replace("{1}", DateTime.Now.Year.ToString());
            Console.WriteLine(content);
            File.WriteAllText(path, content);
        }

        private static void Bundle(string env)
        {
            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, env);
            var entry = Path.Combine(root, "Cafe.Matcha.dll");
            var version = FileVersionInfo.GetVersionInfo(entry);

            var outName = $"Cafe.Matcha-{version.FileVersion}-{env}.zip";

            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    archive.CreateEntryFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\assets\manifest.json"), "manifest.json");

                    foreach (var dll in Directory.GetFiles(root, "*.dll"))
                    {
                        archive.CreateEntryFromFile(dll, @"Plugins/Cafe.Matcha/" + Path.GetFileName(dll));
                    }

                    foreach (var data in Directory.GetFiles(Path.Combine(root, "data"), "*.json"))
                    {
                        archive.CreateEntryFromFile(data, @"Plugins/Cafe.Matcha/data/" + Path.GetFileName(data));
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outName), FileMode.Create))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fileStream);
                }
            }
        }

        private static void Main(string[] args)
        {
            string env = args.Length >= 1 ? args[0] : "Release";
            if (env == "Assembly")
            {
                GenerateAssembly();
            }
            else
            {
                Bundle(env);
            }
        }
    }
}
