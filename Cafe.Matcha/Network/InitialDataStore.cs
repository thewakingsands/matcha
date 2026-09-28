// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network
{
    using System;
    using Cafe.Matcha.Models;

    internal sealed class InitialDataStore
    {
        private readonly object sync = new object();
        private InitialDataSnapshot current;
        private bool active;
        private int session;

        public static InitialDataStore Instance { get; } = new InitialDataStore();

        public event EventHandler Changed;

        public InitialDataSnapshot Current
        {
            get
            {
                lock (sync)
                {
                    return current;
                }
            }
        }

        public int BeginSession()
        {
            int result;
            lock (sync)
            {
                result = ++session;
                active = true;
                current = null;
            }

            Changed?.Invoke(this, EventArgs.Empty);
            return result;
        }

        public void EndSession()
        {
            lock (sync)
            {
                active = false;
                current = null;
            }

            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void Replace(int expectedSession, InitialDataSnapshot snapshot)
        {
            lock (sync)
            {
                if (!active || expectedSession != session || (current != null && snapshot.ReceivedAt < current.ReceivedAt))
                {
                    return;
                }

                current = snapshot;
            }

            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
