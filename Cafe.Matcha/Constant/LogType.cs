namespace Cafe.Matcha.Constant
{
    public enum LogType
    {
        None,
        Universalis,
        LogLine,
        State,

#if DEBUG
        Event,
        Request,
        Telemetry,
        ActorControlSelf,
        InvalidPacket,
#endif
    }
}
