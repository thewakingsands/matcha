// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.DTO
{
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class MiniCactpotDTO : BaseDTO
    {
        public override EventType EventType
        {
            get
            {
                return EventType.MiniCactpot;
            }
        }

        [JsonProperty("isNewGame")]
        public bool IsNewGame;

        [JsonProperty("x")]
        public int X;

        [JsonProperty("y")]
        public int Y;

        [JsonProperty("value")]
        public int Value;
    }
}
