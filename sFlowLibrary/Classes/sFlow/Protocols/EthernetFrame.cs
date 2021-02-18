using BelowAverage.sFlow.Generic;
using BelowAverage.sFlow.Protocols.IP;
using System;

namespace BelowAverage.sFlow.Protocols
{
    public class EthernetFrame : Header
    {
        public const uint HeaderLength = 14;
        public MACAddress DestinationMAC;
        public MACAddress SourceMAC;
        public PacketType PacketType = 0;
        public Packet Packet;
        public EthernetFrame(byte[] buffer)
        {
            DestinationMAC = new MACAddress(buffer.AsSpan(0, 6).ToArray());
            SourceMAC = new MACAddress(buffer.AsSpan(6, 6).ToArray());
            PacketType = (PacketType)buffer.ToUShort(12, 2);
            if(PacketType == PacketType.IPv4)
            {
                Packet = new IPv4Packet(buffer.AsSpan((int)HeaderLength).ToArray());
            }
            if(PacketType == PacketType.IPv6)
            {
                Packet = new IPv6Packet(buffer.AsSpan((int)HeaderLength).ToArray());
            }
        }
    }
}