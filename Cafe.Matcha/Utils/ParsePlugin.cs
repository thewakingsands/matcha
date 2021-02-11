// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using Advanced_Combat_Tracker;

    internal class ParsePlugin
    {
        public static ParsePlugin Instance { get; private set; } = null;

        public static void Init(IActPluginV1 plugin, Network.INetworkMonitor network)
        {
            Instance = new ParsePlugin(plugin, network);
        }

        private readonly FFXIV_ACT_Plugin.FFXIV_ACT_Plugin _parsePlugin;

        public Network.INetworkMonitor Network { private get; set; }

        public ParsePlugin(IActPluginV1 plugin, Network.INetworkMonitor network)
        {
            _parsePlugin = (FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)plugin;
            Network = network;
        }

        public void Start()
        {
            _parsePlugin.DataSubscription.NetworkReceived += HandleMessageReceived;
            _parsePlugin.DataSubscription.NetworkSent += HandleMessageSent;
        }

        public void Stop()
        {
            _parsePlugin.DataSubscription.NetworkReceived -= HandleMessageReceived;
            _parsePlugin.DataSubscription.NetworkSent -= HandleMessageSent;
        }

        public uint GetZone()
        {
            return _parsePlugin.DataRepository.GetCurrentTerritoryID();
        }

        public uint GetServer()
        {
            var combatantList = _parsePlugin.DataRepository.GetCombatantList();
            if (combatantList == null || combatantList.Count == 0)
            {
                return 0;
            }

            return combatantList[0].CurrentWorldID;
        }

        private void HandleMessageSent(string connection, long epoch, byte[] message)
        {
            Network?.HandleMessageSent(connection, epoch, message);
        }

        private void HandleMessageReceived(string connection, long epoch, byte[] message)
        {
            Network?.HandleMessageReceived(connection, epoch, message);
        }
    }
}
