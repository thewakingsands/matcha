// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

using Cafe.Matcha.Constant;

namespace Cafe.Matcha.Utils
{
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
#endif
    }
}
