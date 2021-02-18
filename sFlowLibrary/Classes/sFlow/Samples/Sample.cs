namespace BelowAverage.sFlow.Samples
{
    public class Sample
    {
        public const uint StartIndexBytes = 8;
        public const uint HeaderLengthBytes = 16;
        public ushort Enterprise = 0;
        public SampleType Type = 0;
        public uint Length = 0;
        public uint Sequence = 0;
        public SourceIDType SourceIDType = 0;
        public uint SourceIDIndex = 0;
        public Sample(byte[] buffer)
        {
            uint entAndType = buffer.ToUInt(0, 4);
            Enterprise = (ushort)((entAndType & 0b11111111111111111111000000000000) >> 12);
            Type = (SampleType)(entAndType & 0b00000000000000000000111111111111);
            Length = buffer.ToUInt(4, 4);
            Sequence = buffer.ToUInt(8, 4);
            uint source = buffer.ToUInt(12, 4);
            SourceIDType = (SourceIDType)((source & 0b11111111000000000000000000000000) >> 24);
            SourceIDIndex = source & 0b00000000111111111111111111111111;
        }
    }
    public enum SampleType : ushort
    {
        Unknown = 0,
        Flow = 1,
        Counter = 2
    }
    public enum SourceIDType : byte
    {
        IfIndex = 0,
        SMonVlanDataSource = 1,
        EntPhysicalEntry = 2
    }
}