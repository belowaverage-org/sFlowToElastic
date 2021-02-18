namespace BelowAverage.sFlow.Protocols.IP
{
    public class Packet
    {
        public ProtocolType ProtocolType = 0;
        public Protocol Payload;
        protected void ProcessPayload(byte[] buffer)
        {
            if (ProtocolType == ProtocolType.TCP) Payload = new TCP(buffer);
            if (ProtocolType == ProtocolType.UDP) Payload = new UDP(buffer);
        }
    }
    public enum PacketType : ushort
    {
        Unknown = 0,
        ARP = 0x0806,
        IPv4 = 0x0800,
        IPv6 = 0x86dd
    }
}