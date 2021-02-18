namespace BelowAverage.sFlow.Samples.Flow.Records
{
    public class FlowRecord : Record
    {
        public new RecordType Type = 0;
        public FlowRecord(byte[] buffer) : base(buffer)
        {
            Type = (RecordType)base.Type;
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