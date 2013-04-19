using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator
{
    public interface PacketSink
    {
        void SendPacket(Packet packet);
    }
}
