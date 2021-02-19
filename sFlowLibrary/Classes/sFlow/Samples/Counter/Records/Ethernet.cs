namespace BelowAverage.sFlow.Samples.Counter.Records
{
    public class Ethernet : CounterRecord
    {
        public uint AlignmentErrors = 0;
        public uint FCSErrors = 0;
        public uint SingleCollisionFrames = 0;
        public uint MultipleCollisionFrames = 0;
        public uint SQETestErrors = 0;
        public uint DeferredTransmissions = 0;
        public uint LateCollisions = 0;
        public uint ExcessiveCollisions = 0;
        public uint InternalMacTransmitErrors = 0;
        public uint CarrierSenseErrors = 0;
        public uint FrameTooLongs = 0;
        public uint InternalMacReceiveErrors = 0;
        public uint SymbolErrors = 0;
        public Ethernet(byte[] buffer) : base(buffer)
        {
            AlignmentErrors = buffer.ToUInt(8, 4);
            FCSErrors = buffer.ToUInt(12, 4);
            SingleCollisionFrames = buffer.ToUInt(16, 4);
            MultipleCollisionFrames = buffer.ToUInt(20, 4);
            SQETestErrors = buffer.ToUInt(24, 4);
            DeferredTransmissions = buffer.ToUInt(28, 4);
            LateCollisions = buffer.ToUInt(32, 4);
            ExcessiveCollisions = buffer.ToUInt(36, 4);
            InternalMacTransmitErrors = buffer.ToUInt(40, 4);
            CarrierSenseErrors = buffer.ToUInt(44, 4);
            FrameTooLongs = buffer.ToUInt(48, 4);
            InternalMacReceiveErrors = buffer.ToUInt(52, 4);
            SymbolErrors = buffer.ToUInt(56, 4);
        }
    }
}