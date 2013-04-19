using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator
{
    class DelegatePacketSink : PacketSink
    {
        private Action<Packet> SinkOperation;

        public DelegatePacketSink(Action<Packet> sinkOperation)
        {
            SinkOperation = sinkOperation;
        }

        public void SendPacket(Packet packet)
        {
            SinkOperation(packet);
        }
    }
}
