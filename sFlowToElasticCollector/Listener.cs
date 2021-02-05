using BelowAverage.sFlow;
using System.Net.Sockets;
using System.Net;
using System;
using NetCoreServer;

namespace BelowAverage
{
    class Listener : UdpServer
    {
        public Listener(IPAddress address, int port) : base(address, port) { }
        protected override void OnStarted()
        {
            ReceiveAsync();
        }
        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            byte[] bufferClone = (byte[])buffer.Clone();
            ReceiveAsync();
            new sFlowDatagram(bufferClone).SendToElastic();
        }
        protected override void OnError(SocketError error)
        {
            Console.WriteLine(error.ToString());
        }
    }
}