using BelowAverage.sFlow.Protocols;
using System;

namespace BelowAverage.sFlow.Samples.Flow.Records
{
    public class RawPacketHeader : FlowRecord
    {
        public HeaderProtocol HeaderProtocol = 0;
        public uint FrameLength = 0;
        public uint PayloadRemoved = 0;
        public uint OriginalPacketLength = 0;
        public Header Header;
        public RawPacketHeader(byte[] buffer) : base(buffer)
        {
            HeaderProtocol = (HeaderProtocol)buffer.ToUInt(8, 4);
            FrameLength = buffer.ToUInt(12, 4);
            PayloadRemoved = buffer.ToUInt(16, 4);
            OriginalPacketLength = buffer.ToUInt(20, 4);
            if (HeaderProtocol == HeaderProtocol.Ethernet)
            {
                Header = new EthernetFrame(buffer.AsSpan(24, (int)OriginalPacketLength).ToArray());
            }
        }
    }
}