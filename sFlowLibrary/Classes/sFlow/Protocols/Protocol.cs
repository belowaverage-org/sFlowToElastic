namespace BelowAverage.sFlow.Protocols
{
    public class Protocol { }
    public enum ProtocolType : byte
    {
        ICMP = 0x01,
        TCP = 0x06,
        UDP = 0x11,
        ICMPv6 = 0x3a
    }
}