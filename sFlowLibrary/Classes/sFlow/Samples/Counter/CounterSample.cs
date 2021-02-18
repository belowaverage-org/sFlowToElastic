using BelowAverage.sFlow.Samples.Counter.Records;

namespace BelowAverage.sFlow.Samples.Counter
{
    public class CounterSample : Sample 
    {
        public CounterRecord[] Records = new CounterRecord[0];
        public CounterSample(byte[] buffer) : base(buffer)
        {
            Records = new CounterRecord[buffer.ToUInt(16, 4)];
        }
    }
}