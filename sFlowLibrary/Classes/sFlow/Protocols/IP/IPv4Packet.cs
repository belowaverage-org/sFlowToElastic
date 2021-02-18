using System;
using BelowAverage.sFlow.Generic;

namespace BelowAverage.sFlow.Protocols.IP
{
    public class IPv4Packet : Packet
    {
        public byte Version = 0;
        public byte InternetHeaderLength = 0;
        public byte DifferentiatedServicesCodePoint = 0;
        public byte ExplicitCongestionNotification = 0;
        public ushort TotalLength = 0;
        public ushort Identification = 0;
        public Flags FragmentFlags = 0;
        public ushort FragmentOffset = 0;
        public byte TimeToLive = 0;
        public ushort HeaderChecksum = 0;
        public IPAddress SourceAddress;
        public IPAddress DestinationAddress;
        public IPv4Packet(byte[] buffer)
        {
            Version = (byte)((buffer[0] & 0b11110000) >> 4);
            InternetHeaderLength = (byte)(buffer[0] & 0b00001111);
            DifferentiatedServicesCodePoint = (byte)((buffer[1] & 0b11111100) >> 2);
            ExplicitCongestionNotification = (byte)(buffer[1] & 0b00000011);
            TotalLength = buffer.ToUShort(2, 2);
            Identification = buffer.ToUShort(4, 2);
            ushort flagAndOffset = buffer.ToUShort(6, 2);
            FragmentFlags = (Flags)((flagAndOffset & 0b1110000000000000) >> 13);
            FragmentOffset = (ushort)(flagAndOffset & 0b0001111111111111);
            TimeToLive = buffer[8];
            ProtocolType = (ProtocolType)buffer[9];
            HeaderChecksum = buffer.ToUShort(10, 2);
            SourceAddress = new IPAddress(buffer.AsSpan(12, 4).ToArray());
            DestinationAddress = new IPAddress(buffer.AsSpan(16, 4).ToArray());
            ProcessPayload(buffer.AsSpan(InternetHeaderLength * 4).ToArray());
        }
        [Flags]
        public enum Flags : byte
        {
            Reserved = 0b000,
            DontFragment = 0b010,
            MoreFragments = 0b001
        }
    }
}