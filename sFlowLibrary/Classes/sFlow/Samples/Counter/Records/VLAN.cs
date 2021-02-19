namespace BelowAverage.sFlow.Samples.Counter.Records
{
    public class VLAN : CounterRecord
    {
        public uint VlanID = 0;
        public ulong Octets = 0;
        public uint UCastPkts = 0;
        public uint MulticastPkts = 0;
        public uint BroadcastPkts = 0;
        public uint Discards = 0;
        public VLAN(byte[] buffer) : base(buffer)
        {
            VlanID = buffer.ToUInt(8, 4);
            Octets = buffer.ToULong(12, 8);
            UCastPkts = buffer.ToUInt(20, 4);
            MulticastPkts = buffer.ToUInt(24, 4);
            BroadcastPkts = buffer.ToUInt(28, 4);
            Discards = buffer.ToUInt(32, 4);
        }
    }
}