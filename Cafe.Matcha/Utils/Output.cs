// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Advanced_Combat_Tracker;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Models;
    using Newtonsoft.Json.Linq;
    using Windows.Data.Xml.Dom;
    using Windows.UI.Notifications;

    internal class Output
    {
        private static Models.ConfigOutput Config => Matcha.Config.Instance.Output;
        private static bool Compat => Matcha.Config.Instance.Logger.Compat;

        private static void SendNativeToast(string message)
        {
            Version currentVersion = Environment.OSVersion.Version;
            Version compareToVersion = new Version("6.2");
            if (currentVersion.CompareTo(compareToVersion) >= 0)
            {
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                for (int i = 0; i < stringElements.Length; i++)
                {
                    stringElements[i].AppendChild(toastXml.CreateTextNode(message));
                }

                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier("Advanced Combat Tracker").Show(toast);
            }
            else
            {
                throw new Exception("Unsupported OS");
            }
        }

        public static void SendLog(string log)
        {
            ActGlobals.oFormActMain.ParseRawLogLine(false, DateTime.Now, log);
        }

        public static void SendLog(BaseDTO dto, bool writeLog = true)
        {
            var log = Formatter.GetLog(dto);
            SendLog(log);

            if (writeLog)
            {
                Log.Info(LogType.LogLine, log);
            }

            if (Compat)
            {
                SendLog(Formatter.GetLog(dto, true));
            }
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

        public static void Send(BaseDTO dto)
        {
            Send(Formatter.GetEventText(dto));
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
