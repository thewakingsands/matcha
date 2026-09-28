// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;
    using Cafe.Matcha.Models;

    internal sealed class PlayerSetupHandler : AbstractHandler
    {
        internal const int MinimumPayloadLength = 2951;
        private readonly InitialDataStore store;
        private readonly Func<FishingNotebookData> getMapping;
        private readonly int session;

        public PlayerSetupHandler(Action<BaseDTO> fireEvent, InitialDataStore store,
            Func<FishingNotebookData> getMapping, int session) : base(fireEvent)
        {
            this.store = store;
            this.getMapping = getMapping;
            this.session = session;
        }

        public override bool Handle(Packet packet)
        {
            if (!packet.Valid || !packet.Known || packet.Sender != Packet.PacketSender.Server
                || packet.MatchaOpcode != MatchaOpcode.PlayerSetup)
            {
                return false;
            }

            if (packet.DataLength < MinimumPayloadLength)
            {
                store.Replace(session, new InitialDataSnapshot(string.Empty, packet.Timestamp,
                    Array.Empty<uint>(), Array.Empty<uint>(), "初始数据包不完整，请重新登录游戏获取。"));
                return true;
            }

            var payload = packet.GetRawData();
            var nameLength = 0;
            while (nameLength < 32 && payload[0x34c + nameLength] != 0)
            {
                nameLength++;
            }

            var name = Encoding.UTF8.GetString(payload, 0x34c, nameLength);
            var mapping = getMapping();
            var fish = new HashSet<uint>();
            var spearfish = new HashSet<uint>();
            string error = null;
            if (mapping == null || !mapping.IsValid)
            {
                error = "鱼种映射数据不可用，请更新插件数据。";
            }
            else
            {
                var fishComplete = ReadCompletion(payload, 0x771, 191, 0, mapping.Fish, fish);
                var spearfishComplete = ReadCompletion(payload, 0x85b, 38, 20000, mapping.Spearfish, spearfish);
                if (!fishComplete || !spearfishComplete)
                {
                    error = "部分已完成鱼种缺少物品映射，请更新插件数据后重新登录游戏。";
                }
            }

            store.Replace(session, new InitialDataSnapshot(name, packet.Timestamp, fish, spearfish, error));
            // Valid packets also reach the existing Universalis observer.
            return false;
        }

        private static bool ReadCompletion(byte[] payload, int offset, int length, int firstId,
            Dictionary<int, uint> mapping, HashSet<uint> items)
        {
            var complete = true;
            for (var index = 0; index < length * 8; index++)
            {
                if ((payload[offset + index / 8] & (1 << (index % 8))) == 0)
                {
                    continue;
                }

                if (!mapping.TryGetValue(firstId + index, out var itemId))
                {
                    complete = false;
                }
                else if (itemId != 0)
                {
                    items.Add(itemId);
                }
            }

            return complete;
        }
    }
}
