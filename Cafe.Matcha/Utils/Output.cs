// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using Advanced_Combat_Tracker;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Models;
    using Newtonsoft.Json.Linq;

    internal class Output
    {
        private static Models.ConfigOutput Config => Matcha.Config.Instance.Output;

        private static void SendNativeToast(string message)
        {
            Version currentVersion = Environment.OSVersion.Version;
            Version compareToVersion = new Version("6.2");
            if (currentVersion.CompareTo(compareToVersion) >= 0)
            {
                Windows.Data.Xml.Dom.XmlDocument toastXml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastImageAndText03);
                Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                for (int i = 0; i < stringElements.Length; i++)
                {
                    stringElements[i].AppendChild(toastXml.CreateTextNode(message));
                }

                Windows.UI.Notifications.ToastNotification toast = new Windows.UI.Notifications.ToastNotification(toastXml);
                Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier("Advanced Combat Tracker").Show(toast);
            }
            else
            {
                throw new Exception("Unsupported OS");
            }
        }

        public static void SendLog(string log)
        {
            if (string.IsNullOrEmpty(log))
            {
                return;
            }

            Log.Info($"[log] {log}");
            ActGlobals.oFormActMain.ParseRawLogLine(false, DateTime.Now, log);
        }

        public static void SendTTS(string message)
        {
            if (!Config.TTS)
            {
                return;
            }

            ActGlobals.oFormActMain.TTS(message);
        }

        public static void SendToast(string message)
        {
            if (!Config.Toast)
            {
                return;
            }

            try
            {
                SendNativeToast(message);
            }
            catch
            {
                MessageBox.Show(message, Data.Title, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        public static void SendWebhook(BaseDTO dto)
        {
            foreach (var item in Matcha.Config.Instance.Webhook)
            {
                if (item.Event != dto.EventType)
                {
                    continue;
                }

                SendWebhook(dto, item);
            }
        }

        public static void SendWebhook(BaseDTO dto, ConfigWebhook webhook)
        {
            var eventName = Enum.GetName(typeof(EventType), dto.EventType);

            if (webhook.Type == RequestType.JSON)
            {
                var content = new JObject(
                    new JProperty("event", eventName),
                    new JProperty("data", JToken.FromObject(dto)));
                _ = Request.SendJson(webhook.Endpoint, webhook.Header, content.ToString());
            }
            else
            {
                _ = Request.Send(webhook.Endpoint, webhook.Type, webhook.Header, new Dictionary<string, string>
                    {
                        { "event", eventName },
                        { "data", dto.ToJSON() }
                    });
            }
        }

        public static void Send(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                SendTTS(message);
                SendToast(message);
            }
        }
    }
}
