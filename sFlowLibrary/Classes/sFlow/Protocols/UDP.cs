namespace BelowAverage.sFlow.Protocols
{
    public class UDP : Protocol
    {
        public ushort SourcePort = 0;
        public ushort DestinationPort = 0;
        public ushort Length = 0;
        public ushort Checksum = 0;
        public UDP(byte[] buffer)
        {
            SourcePort = buffer.ToUShort(0, 2);
            DestinationPort = buffer.ToUShort(2, 2);
            Length = buffer.ToUShort(4, 2);
            Checksum = buffer.ToUShort(6, 2);
        }
    }
}