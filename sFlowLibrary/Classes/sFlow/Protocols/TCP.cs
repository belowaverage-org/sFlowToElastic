using System;

namespace BelowAverage.sFlow.Protocols
{
    public class TCP : Protocol
    {
        public ushort SourcePort = 0;
        public ushort DestinationPort = 0;
        public byte DataOffset = 0;
        public Flags ControlFlags = 0;
        public TCP(byte[] buffer)
        {
            SourcePort = buffer.ToUShort(0, 2);
            DestinationPort = buffer.ToUShort(2, 2);
            ushort offsetAndFlags = buffer.ToUShort(12, 2);
            DataOffset = (byte)((offsetAndFlags & 0b1111000000000000) >> 12);
            ControlFlags = (Flags)(offsetAndFlags & 0b0000111111111111);
        }
        [Flags]
        public enum Flags : ushort
        {
            NS = 0b100000000,
            CWR = 0b010000000,
            ECE = 0b001000000,
            URG = 0b000100000,
            ACK = 0b000010000,
            PSH = 0b000001000,
            RST = 0b000000100,
            SYN = 0b000000010,
            FIN = 0b000000001
        }
    }
}