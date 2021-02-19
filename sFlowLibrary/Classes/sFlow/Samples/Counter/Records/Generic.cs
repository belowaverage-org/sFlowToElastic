using System;

namespace BelowAverage.sFlow.Samples.Counter.Records
{
    public class Generic : CounterRecord
    {
        public uint IfIndex = 0;
        public uint IfType = 0;
        public ulong IfSpeed = 0;
        public IfDirectionType IfDirection = 0;
        public IfStatusFlags IfStatus = 0;
        public ulong IfInOctets = 0;
        public uint IfInUcastPkts = 0;
        public uint IfInMulticastPkts = 0;
        public uint IfInBroadcastPkts = 0;
        public uint IfInDiscards = 0;
        public uint IfInErrors = 0;
        public uint IfInUnknownProtos = 0;
        public ulong IfOutOctets = 0;
        public uint IfOutUcastPkts = 0;
        public uint IfOutMulticastPkts = 0;
        public uint IfOutBroadcastPkts = 0;
        public uint IfOutDiscards = 0;
        public uint IfOutErrors = 0;
        public uint IfPromiscuousMode = 0;
        public Generic(byte[] buffer) : base(buffer)
        {
            IfIndex = buffer.ToUInt(8, 4);
            IfType = buffer.ToUInt(12, 4);
            IfSpeed = buffer.ToULong(16, 8);
            IfDirection = (IfDirectionType)buffer.ToUInt(24, 4);
            IfStatus = (IfStatusFlags)buffer.ToUInt(28, 4);
            IfInOctets = buffer.ToULong(32, 8);
            IfInUcastPkts = buffer.ToUInt(40, 4);
            IfInMulticastPkts = buffer.ToUInt(44, 4);
            IfInBroadcastPkts = buffer.ToUInt(48, 4);
            IfInDiscards = buffer.ToUInt(52, 4);
            IfInErrors = buffer.ToUInt(56, 4);
            IfInUnknownProtos = buffer.ToUInt(60, 4);
            IfOutOctets = buffer.ToULong(64, 8);
            IfOutUcastPkts = buffer.ToUInt(72, 4);
            IfOutMulticastPkts = buffer.ToUInt(76, 4);
            IfOutBroadcastPkts = buffer.ToUInt(80, 4);
            IfOutDiscards = buffer.ToUInt(84, 4);
            IfOutErrors = buffer.ToUInt(88, 4);
            IfPromiscuousMode = buffer.ToUInt(82, 4);
        }
        public enum IfDirectionType : uint
        {
            Unknown = 0,
            FullDuplex = 1,
            HalfDuplex = 2,
            In = 3,
            Out = 4
        }
        [Flags]
        public enum IfStatusFlags : uint
        {
            IfAdminStatusUp = 0b01,
            IfOperStatusUp = 0b10
        }
    }
}