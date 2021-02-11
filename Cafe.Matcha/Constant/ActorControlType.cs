// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    internal enum ActorControlType : ushort
    {
#if GLOBAL
        TreasureSpot = 84,
        DirectorUpdate = 109,
        FateStart = 116,
        FateEnd = 121,
        FateProgress = 155
#else
        TreasureSpot = 84,
        DirectorUpdate = 109,
        FateStart = 2357,
        FateEnd = 2358,
        FateProgress = 2366
#endif
    }
}
