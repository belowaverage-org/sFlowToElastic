using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using BelowAverage.sFlow;
using BelowAverage.sFlow.Samples;
using BelowAverage.sFlow.Protocols;
using BelowAverage.sFlow.Protocols.IP;
using BelowAverage.sFlow.Samples.Flow;
using BelowAverage.sFlow.Samples.Flow.Records;
using BelowAverage.sFlow.Samples.Counter.Records;
using FlowRecordType = BelowAverage.sFlow.Samples.Flow.Records.RecordType;
using CounterRecordType = BelowAverage.sFlow.Samples.Counter.Records.RecordType;
using BelowAverage.sFlow.Samples.Flow.Records.Extended;
using BelowAverage.sFlow.Samples.Counter;

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
                    doc.Add("frame_in_interface_value", flowSample.InputInterface.Value);
                    doc.Add("frame_out_interface_value", flowSample.OutputInterface.Value);
                    doc.Add("frame_out_interface_format", flowSample.OutputInterface.Format.ToString());
                    doc.Add("frame_out_interface_discard", flowSample.OutputInterface.DiscardReason.ToString());
                    foreach (FlowRecord record in flowSample.Records)
                    {
                        if (record.Type == FlowRecordType.RawPacketHeader)
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
                        else if (record.Type == FlowRecordType.ExtSwitchData)
                        {
                            SwitchData switchData = (SwitchData)record;
                            doc.Add("vlan_in", switchData.IncomingVLAN);
                            doc.Add("vlan_out", switchData.OutgoingVLAN);
                        }
                    }
                }
                else if(sample.Type == SampleType.Counter)
                {
                    CounterSample countSample = (CounterSample)sample;
                    foreach (CounterRecord record in countSample.Records)
                    {
                        if (record.Type == CounterRecordType.GenericInterface)
                        {
                            Generic gi = (Generic)record;
                            doc.Add("if_direction", gi.IfDirection.ToString());
                            doc.Add("if_in_broadcast_pkts", gi.IfInBroadcastPkts);
                            doc.Add("if_index", gi.IfIndex);
                            doc.Add("if_in_discards", gi.IfInDiscards);
                            doc.Add("if_in_errors", gi.IfInErrors);
                            doc.Add("if_in_multicast_pkts", gi.IfInMulticastPkts);
                            doc.Add("if_in_octets", gi.IfInOctets);
                            doc.Add("if_in_unicast_pkts", gi.IfInUcastPkts);
                            doc.Add("if_in_unknown_protos", gi.IfInUnknownProtos);
                            doc.Add("if_out_broadcast_pkts", gi.IfOutBroadcastPkts);
                            doc.Add("if_out_discards", gi.IfOutDiscards);
                            doc.Add("if_out_errors", gi.IfOutErrors);
                            doc.Add("if_out_multicast_pkts", gi.IfOutMulticastPkts);
                            doc.Add("if_out_octets", gi.IfOutOctets);
                            doc.Add("if_out_unicast_ptks", gi.IfOutUcastPkts);
                            doc.Add("if_promiscuous_mode", gi.IfPromiscuousMode);
                            doc.Add("if_speed", gi.IfSpeed);
                            doc.Add("if_type", gi.IfType);
                            doc.Add("if_status_up_admin", gi.IfStatus.HasFlag(Generic.IfStatusFlags.IfAdminStatusUp));
                            doc.Add("if_status_up_operational", gi.IfStatus.HasFlag(Generic.IfStatusFlags.IfOperStatusUp));
                        }
                        else if (record.Type == CounterRecordType.EthernetInterface)
                        {
                            Ethernet eth = (Ethernet)record;
                            doc.Add("eth_alignment_errors", eth.AlignmentErrors);
                            doc.Add("eth_carrier_sense_errors", eth.CarrierSenseErrors);
                            doc.Add("eth_deferred_transmissions", eth.DeferredTransmissions);
                            doc.Add("eth_excessive_collisions", eth.ExcessiveCollisions);
                            doc.Add("eth_fcs_errors", eth.FCSErrors);
                            doc.Add("eth_frame_too_longs", eth.FrameTooLongs);
                            doc.Add("eth_mac_recieve_errors", eth.InternalMacReceiveErrors);
                            doc.Add("eth_mac_transmit_errors", eth.InternalMacTransmitErrors);
                            doc.Add("eth_late_collisions", eth.LateCollisions);
                            doc.Add("eth_multiple_collision_frames", eth.MultipleCollisionFrames);
                            doc.Add("eth_single_collision_frames", eth.SingleCollisionFrames);
                            doc.Add("eth_sqe_test_errors", eth.SQETestErrors);
                            doc.Add("eth_symbol_errors", eth.SymbolErrors);
                        }
                        else if (record.Type == CounterRecordType.VLAN)
                        {
                            VLAN vlan = (VLAN)record;
                            doc.Add("vlan_multicast_pkts", vlan.MulticastPkts);
                            doc.Add("vlan_octets", vlan.Octets);
                            doc.Add("vlan_unicast_pkts", vlan.UCastPkts);
                            doc.Add("vlan_id", vlan.VlanID);
                        }
                        else if (record.Type == CounterRecordType.ProcessorInformation)
                        {
                            ProcessorInfo pi = (ProcessorInfo)record;
                            doc.Add("stats_cpu_percent_1m", pi.Cpu1mPercentage);
                            doc.Add("stats_cpu_percent", pi.Cpu5mPercentage);
                            doc.Add("stats_cpu_5s_percent", pi.Cpu5sPercentage);
                            doc.Add("stats_memory_free", pi.FreeMemory);
                            doc.Add("stats_memory_total", pi.TotalMemory);
                        }
                    }
                }
                request += "{\"create\":{\"_index\":\"" + PREFIX + sample.Type.ToString().ToLower() + "\"}}\n";
                request += JsonConvert.SerializeObject(doc) + '\n';
            }
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(request));
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