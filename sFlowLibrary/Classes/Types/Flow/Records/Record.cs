namespace BelowAverage.sFlow.Types.Flow.Records
{
    public class Record
    {
        public const uint HeaderLength = 8;
        public RecordType Type = 0;
        public uint Length = 0;
        public Record(byte[] buffer)
        {
            uint type = buffer.ToUInt(0, 4);
            Type = (RecordType)(type & 0b00000000000000000000111111111111);
            Length = buffer.ToUInt(4, 4);
        }
    }
    public enum RecordType : ushort
    {
        Unknown = 0,
        RawPacketHeader = 1,
        EthernetFrameData = 2,
        IPv4Data = 3,
        IPv6Data = 4,
        ExtSwitchData = 1001,
        ExtRouterData = 1002,
        ExtGatewayData = 1003,
        ExtUserData = 1004,
        ExtURLData = 1005,
        ExtNATData = 1007,
        ExtVLANTunnel = 1012
    }
}