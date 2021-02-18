namespace BelowAverage.sFlow.Generic
{
    public class IPAddress {
        public AddressType Type = 0;
        public byte[] Bytes;
        public IPAddress(byte[] Bytes)
        {
            this.Bytes = Bytes;
            if (Bytes.Length == 4) Type = AddressType.IPv4;
            if (Bytes.Length == 16) Type = AddressType.IPv6;
        }
        public override string ToString()
        {
            if (Type == AddressType.IPv4) return Bytes[0] + "." + Bytes[1] + "." + Bytes[2] + "." + Bytes[3];
            return null;
        }
        public enum AddressType
        {
            IPv4 = 1,
            IPv6 = 2
        }
    }
}