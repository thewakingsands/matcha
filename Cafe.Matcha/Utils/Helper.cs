// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Interop;
    using Advanced_Combat_Tracker;

    internal class Helper
    {
        private static readonly int[] WaitTime = { 5, 5, 10, 15, 25 };
        public static IActPluginV1 Instance = null;
        public static async Task<IActPluginV1> GetFFXIVPlugin()
        {
            for (int i = 0; i < WaitTime.Length; ++i)
            {
                var plugin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.cbEnabled.Checked && x.lblPluginTitle.Text == "FFXIV_ACT_Plugin.dll");
                if (plugin != null)
                {
                    return plugin.pluginObj;
                }

                await Task.Delay(WaitTime[i] * 1000);
            }

            return null;
        }

        public static string GetConfigDir()
        {
            return Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config");
        }

        public static string GetPluginDir()
        {
            if (Instance != null)
            {
                foreach (ActPluginData plugin in ActGlobals.oFormActMain.ActPlugins)
                {
                    if (plugin.pluginObj == Instance)
                    {
                        return plugin.pluginFile.Directory.FullName;
                    }
                }
            }

            foreach (ActPluginData plugin in ActGlobals.oFormActMain.ActPlugins)
            {
                if (plugin.pluginFile.Name == "Cafe.Matcha.dll")
                {
                    return plugin.pluginFile.Directory.FullName;
                }
            }

            string libPath = Assembly.GetExecutingAssembly().Location;
            if (libPath == "")
            {
                throw new Exception("Failed to locate the library.");
            }

            return Path.GetDirectoryName(libPath);
        }

        public static void CheckLicenseNotice()
        {
            MD5 md5 = MD5.Create();
            var pathHash = md5.ComputeHash(Encoding.UTF8.GetBytes(GetPluginDir()));
            var hash = BitConverter.ToString(pathHash).Replace("-", "");

            if (hash != Config.Instance.Hash)
            {
                Config.Instance.Hash = hash;
                var dialog = new Views.License();
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.Topmost = true;
                dialog.ShowDialog();
            }
        }

#pragma warning disable SA1310 // Field names should not contain underscore
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
#pragma warning restore SA1310 // Field names should not contain underscore

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
          int x, int y, int width, int height, uint flags);

        public static void SetDialog(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public static string ReadString(byte[] data, int offset, int length)
        {
            return Encoding.UTF8.GetString(data, offset, length).TrimEnd((char)0);
        }
    }
}
