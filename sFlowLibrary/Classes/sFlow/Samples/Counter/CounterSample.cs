using BelowAverage.sFlow.Samples.Counter.Records;
using System;

namespace BelowAverage.sFlow.Samples.Counter
{
    public class CounterSample : Sample 
    {
        public CounterRecord[] Records = new CounterRecord[0];
        public CounterSample(byte[] buffer) : base(buffer)
        {
            Records = new CounterRecord[buffer.ToUInt(16, 4)];
            uint recordStartIndex = 20;
            for (uint i = 0; i < Records.Length; i++)
            {
                CounterRecord record = new CounterRecord(buffer.AsSpan((int)recordStartIndex, (int)Record.HeaderLengthBytes).ToArray());
                int recordLength = (int)(record.Length + Record.HeaderLengthBytes);
                if (record.Type == RecordType.GenericInterface)
                {
                    record = new Records.Generic(buffer.AsSpan((int)recordStartIndex, recordLength).ToArray());
                }
                else if (record.Type == RecordType.EthernetInterface)
                {
                    record = new Ethernet(buffer.AsSpan((int)recordStartIndex, recordLength).ToArray());
                }
                else if (record.Type == RecordType.VLAN)
                {
                    record = new VLAN(buffer.AsSpan((int)recordStartIndex, recordLength).ToArray());
                }
                else if (record.Type == RecordType.ProcessorInformation)
                {
                    record = new ProcessorInfo(buffer.AsSpan((int)recordStartIndex, recordLength).ToArray());
                }
                recordStartIndex += (uint)recordLength;
                Records[i] = record;
            }
        }
    }
}