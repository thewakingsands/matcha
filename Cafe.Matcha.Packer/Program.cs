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
        private const string Manifest = @"{
  ""entryPoint"": ""Plugins/Cafe.Matcha/Cafe.Matcha.dll""
}
";
        private static void Bundle(string env)
        {
            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, env);
            var entry = Path.Combine(root, "Cafe.Matcha.dll");
            var version = FileVersionInfo.GetVersionInfo(entry);

            var outName = $"Cafe.Matcha-{version.FileVersion}-{env}.zip";

            using var ms = new MemoryStream();
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                var manifestEntry = archive.CreateEntry("manifest.json");
                using (var entryStream = manifestEntry.Open())
                using (var streamWriter = new StreamWriter(entryStream))
                {
                    streamWriter.Write(Manifest);
                }

                foreach (var dll in Directory.GetFiles(root, "*.dll"))
                {
                    archive.CreateEntryFromFile(dll, @"Plugins/Cafe.Matcha/" + Path.GetFileName(dll));
                }

                foreach (var data in Directory.GetFiles(Path.Combine(root, "data"), "*.json"))
                {
                    archive.CreateEntryFromFile(data, @"Plugins/Cafe.Matcha/data/" + Path.GetFileName(data));
                }
            }

            using var fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outName), FileMode.Create);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fileStream);
        }

        private static void Main(string[] args)
        {
            string env = args.Length >= 1 ? args[0] : "Release";
            Bundle(env);
        }
    }
}
