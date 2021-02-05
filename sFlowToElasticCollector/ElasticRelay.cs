using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using BelowAverage.sFlow;
using BelowAverage.sFlow.Types.IPPackets;
using BelowAverage.sFlow.Types.Protocols;
using BelowAverage.sFlow.Types.Flow.Records;
using BelowAverage.sFlow.Types.Flow.Records.Extended;
using BelowAverage.sFlow.Types.Flow.Samples;

namespace BelowAverage
{
    static class ElasticRelay
    {
        public static string URI = "";
        public static string PREFIX = "";
        public static string ELASTIC_USER = null;
        public static string ELASTIC_PASS = null;
        public static string BASIC_HEADER = null;
        private static HttpClient HC = new HttpClient();
        public static void Setup()
        {
            if (ELASTIC_PASS == null) return;
            HC.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(ELASTIC_USER + ":" + ELASTIC_PASS)));
        }
        public static async void SendToElastic(this sFlowDatagram datagram)
        {
            string request = "";
            string datetime = DateTime.Now.ToString("o");
            foreach (Sample sample in datagram.Samples)
            {
                if (sample == null) continue;
                Dictionary<string, object> doc = new Dictionary<string, object>();
                doc.Add("@timestamp", datetime);
                doc.Add("sflow_source", datagram.AgentAddress.ToString());
                doc.Add("sflow_sequence", datagram.SequenceNumber);
                if (sample.Type == SampleType.Flow)
                {
                    FlowSample flowSample = (FlowSample)sample;
                    doc.Add("sampling_rate", flowSample.SamplingRate);
                    doc.Add("sampling_pool", flowSample.SamplingPool);
                    doc.Add("dropped_packets", flowSample.DroppedPackets);
                    doc.Add("frame_in_interface", flowSample.InputInterface);
                    doc.Add("frame_out_interface", flowSample.OutputInterface);
                    foreach (Record record in flowSample.Records)
                    {
                        if (record.Type == RecordType.RawPacketHeader)
                        {
                            RawPacketHeader rawPacketHeader = (RawPacketHeader)record;
                            if (rawPacketHeader.HeaderProtocol == HeaderProtocol.Ethernet)
                            {
                                EthernetFrame ethFrame = (EthernetFrame)rawPacketHeader.Header;
                                doc.Add("mac_source", ethFrame.SourceMAC.ToString());
                                doc.Add("mac_destination", ethFrame.DestinationMAC.ToString());
                                doc.Add("packet_type", ethFrame.PacketType.ToString());
                                ProtocolType ProtocolType = 0;
                                if (ethFrame.PacketType == PacketType.IPv4)
                                {
                                    IPv4Packet packet = (IPv4Packet)ethFrame.Packet;
                                    doc.Add("ip_source", packet.SourceAddress.ToString());
                                    doc.Add("ip_destination", packet.DestinationAddress.ToString());
                                    doc.Add("ip_ttl", packet.TimeToLive);
                                    doc.Add("protocol_type", packet.ProtocolType.ToString());
                                    ProtocolType = packet.ProtocolType;
                                }
                                else if (ethFrame.PacketType == PacketType.IPv6)
                                {
                                    //IPv6Packet packet = (IPv6Packet)ethernetFrame.Packet;
                                }
                                else if (ethFrame.PacketType == PacketType.ARP)
                                {
                                    //Maybe
                                }
                                if (ProtocolType == ProtocolType.TCP)
                                {
                                    TCP TCP = (TCP)ethFrame.Packet.Payload;
                                    doc.Add("port_source", TCP.SourcePort);
                                    doc.Add("port_destination", TCP.DestinationPort);
                                }
                                else if (ProtocolType == ProtocolType.UDP)
                                {
                                    UDP UDP = (UDP)ethFrame.Packet.Payload;
                                    doc.Add("port_source", UDP.SourcePort);
                                    doc.Add("port_destination", UDP.DestinationPort);
                                }
                            }
                        }
                        else if (record.Type == RecordType.ExtSwitchData)
                        {
                            SwitchData switchData = (SwitchData)record;
                            doc.Add("vlan_in", switchData.IncomingVLAN);
                            doc.Add("vlan_out", switchData.OutgoingVLAN);
                        }
                    }
                }
                request += "{\"create\":{\"_index\":\"" + PREFIX + sample.Type.ToString().ToLower() + "\"}}\n";
                request += JsonConvert.SerializeObject(doc) + '\n';
            }
            try
            {
                MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(request));
                await HC.PostAsync(URI + "/_bulk", new StreamContent(ms)
                {
                    Headers =
                        {
                            ContentType = MediaTypeHeaderValue.Parse("application/json")
                        }
                });
                ms.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}