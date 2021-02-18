namespace BelowAverage.sFlow.Samples.Counter.Records
{
    public class CounterRecord : Record
    {
        public new RecordType Type = 0;
        public CounterRecord(byte[] buffer) : base(buffer)
        {
            Type = (RecordType)base.Type;
        }
    }
    public enum RecordType : ushort
    {
        Unknown = 0,
        GenericInterface = 1,
        EthernetInterface = 2,
        TokenRing = 3,
        BaseVG100Interface = 4,
        VLAN = 5,
        ProcessorInformation = 6
    }
}