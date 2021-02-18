using BelowAverage.sFlow.Generic;
using BelowAverage.sFlow.Samples;
using BelowAverage.sFlow.Samples.Counter;
using BelowAverage.sFlow.Samples.Flow;
using System;

namespace BelowAverage.sFlow
{
    public class sFlowDatagram
    {
        public const uint HeaderLength = 28;
        public uint DatagramVersion = 0;
        public IPAddress AgentAddress = null;
        public uint SubAgentID = 0;
        public uint SequenceNumber = 0;
        public uint SystemUptimeMS = 0;
        public Sample[] Samples;
        public sFlowDatagram(byte[] UDPDatagramBuffer)
        {
            DatagramVersion = UDPDatagramBuffer.ToUInt(0, 4);
            AgentAddress = new IPAddress(UDPDatagramBuffer.AsSpan(8, 4).ToArray());
            SubAgentID = UDPDatagramBuffer.ToUInt(12, 4);
            SequenceNumber = UDPDatagramBuffer.ToUInt(16, 4);
            SystemUptimeMS = UDPDatagramBuffer.ToUInt(20, 4);
            uint sampleCount = UDPDatagramBuffer.ToUInt(24, 4);
            Samples = new Sample[sampleCount];
            uint sampleIndex = 0;
            for (uint i = 0; i < sampleCount; i++)
            {
                Sample sample = new Sample(UDPDatagramBuffer.AsSpan((int)(HeaderLength + sampleIndex), (int)Sample.HeaderLengthBytes).ToArray());
                byte[] buffer = UDPDatagramBuffer.AsSpan((int)(HeaderLength + sampleIndex), (int)(sample.Length + Sample.StartIndexBytes)).ToArray();
                if (sample.Type == SampleType.Flow)
                {
                    Samples[i] = new FlowSample(buffer);
                }
                else if (sample.Type == SampleType.Counter)
                {
                    Samples[i] = new CounterSample(buffer);
                }
                sampleIndex += sample.Length + Sample.StartIndexBytes;
            }
        }
    }
}