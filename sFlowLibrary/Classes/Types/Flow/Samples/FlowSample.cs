﻿using BelowAverage.sFlow.Types.Flow.Records;
using BelowAverage.sFlow.Types.Flow.Records.Extended;
using System;

namespace BelowAverage.sFlow.Types.Flow.Samples
{
    public class FlowSample : Sample
    {
        public uint SamplingRate = 0;
        public uint SamplingPool = 0;
        public uint DroppedPackets = 0;
        public sFlowInterface InputInterface = null;
        public sFlowInterface OutputInterface = null;
        public Record[] Records = new Record[0];
        public FlowSample(byte[] buffer) : base (buffer)
        {
            SamplingRate = buffer.ToUInt(16, 4);
            SamplingPool = buffer.ToUInt(20, 4);
            DroppedPackets = buffer.ToUInt(24, 4);
            InputInterface = buffer.ToUInt(28, 4).ToSflowInterface();
            OutputInterface = buffer.ToUInt(32, 4).ToSflowInterface();
            uint records = buffer.ToUInt(36, 4);
            Records = new Record[records];
            uint recordStartIndex = 40;
            for (uint i = 0; i < Records.Length; i++)
            {
                Record record = new Record(buffer.AsSpan((int)recordStartIndex, (int)Record.HeaderLength).ToArray());
                int recordLength = (int)(record.Length + Record.HeaderLength);
                if(record.Type == RecordType.RawPacketHeader)
                {
                    record = new RawPacketHeader(buffer.AsSpan((int)recordStartIndex, recordLength).ToArray());
                }
                else if(record.Type == RecordType.ExtSwitchData)
                {
                    record = new SwitchData(buffer.AsSpan((int)recordStartIndex, recordLength).ToArray());
                }
                recordStartIndex += (uint)recordLength;
                Records[i] = record;
            }
        }
    }
}