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

        Debug1,
#endif
    }
}
