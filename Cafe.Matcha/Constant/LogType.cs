// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    public enum LogType
    {
        None,
        Universalis,
        LogLine,
        State,
        Event,

#if DEBUG
        Request,
        Telemetry,
        ActorControlSelf,
        InvalidPacket,
        RawPacket,
        Debug1,
#endif
    }
}
