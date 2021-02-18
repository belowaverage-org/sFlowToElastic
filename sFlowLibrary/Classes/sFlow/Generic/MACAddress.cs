using System;

namespace BelowAverage.sFlow.Generic
{
    public class MACAddress
    {
        public byte[] Address = new byte[6];
        public MACAddress(byte[] Address)
        {
            this.Address = Address;
        }
        public override string ToString()
        {
            return BitConverter.ToString(Address, 0, 6);
        }
    }
}