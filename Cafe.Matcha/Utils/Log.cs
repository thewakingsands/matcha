// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System.Text;
    using Cafe.Matcha.Constant;

    internal class Log
    {
        public delegate void EventHandler(LogType type, char level, string message);
        public static event EventHandler Handler;
        public static void Add(LogType type, char level, string message)
        {
            Handler?.Invoke(type, level, message);
        }

        public static void Error(LogType type, string message)
        {
            Add(type, 'E', message);
        }

        public static void Warn(LogType type, string message)
        {
            Add(type, 'W', message);
        }

        public static void Info(LogType type, string message)
        {
            Add(type, 'I', message);
        }

#if DEBUG
        public static void Debug(LogType type, string message)
        {
            Add(type, 'D', message);
        }

        public static void Packet(byte[] byteArray)
        {
            StringBuilder hexDump = new StringBuilder();
            const int lineLength = 16;

            for (int i = 0; i < byteArray.Length; i += lineLength)
            {
                hexDump.AppendFormat("{0:X8}: ", i);
                for (int j = 0; j < lineLength; j++)
                {
                    if (i + j >= byteArray.Length)
                    {
                        break;
                    }

                    byte b = byteArray[i + j];
                    hexDump.Append(b.ToString("X2"));
                    hexDump.Append(" ");
                }

                hexDump.AppendLine();
            }

            Add(LogType.RawPacket, 'D', hexDump.ToString());
        }
#endif
    }
}
