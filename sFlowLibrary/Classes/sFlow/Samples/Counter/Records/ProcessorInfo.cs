namespace BelowAverage.sFlow.Samples.Counter.Records
{
    public class ProcessorInfo : CounterRecord
    {
        public double Cpu5sPercentage = 0;
        public double Cpu1mPercentage = 0;
        public double Cpu5mPercentage = 0;
        public ulong TotalMemory = 0;
        public ulong FreeMemory = 0;
        public ProcessorInfo(byte[] buffer) : base(buffer)
        {
            Cpu5sPercentage = buffer.ToUInt(8, 4) / 100;
            Cpu1mPercentage = buffer.ToUInt(12, 4) / 100;
            Cpu5mPercentage = buffer.ToUInt(16, 4) / 100;
            TotalMemory = buffer.ToULong(20, 8);
            FreeMemory = buffer.ToULong(28, 8);
        }
    }
}