namespace BelowAverage.sFlow.Samples
{
    public class Record
    {
        public const uint HeaderLengthBytes = 8;
        public uint Type = 0;
        public uint Length = 0;
        public Record(byte[] buffer)
        {
            uint type = buffer.ToUInt(0, 4);
            Type = type & 0b00000000000000000000111111111111;
            Length = buffer.ToUInt(4, 4);
        }
    }
}