namespace BelowAverage.sFlow.Types.Flow.Records.Extended
{
    public class SwitchData : Record
    {
        public uint IncomingVLAN = 0;
        public uint IncomingPriority = 0;
        public uint OutgoingVLAN = 0;
        public uint OutgoingPriority = 0;
        public SwitchData(byte[] buffer) : base(buffer)
        {
            IncomingVLAN = buffer.ToUInt(8, 4);
            IncomingPriority = buffer.ToUInt(12, 4);
            OutgoingVLAN = buffer.ToUInt(16, 4);
            OutgoingPriority = buffer.ToUInt(20, 4);
        }
    }
}