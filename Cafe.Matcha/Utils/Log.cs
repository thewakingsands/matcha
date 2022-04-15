// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    internal class Log
    {
        public delegate void EventHandler(char type, string message);
        public static event EventHandler Handler;
        public static void Add(char type, string message)
        {
            Handler?.Invoke(type, message);
        }

        public static void Error(string message)
        {
            Add('E', message);
        }

        public static void Warn(string message)
        {
            Add('W', message);
        }

        public static void Info(string message)
        {
            Add('I', message);
        }

#if DEBUG
        public static void Debug(string message)
        {
            Add('D', message);
        }
#endif
    }
}
