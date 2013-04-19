using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public interface Device
    {
        UID UID { get; }

        Packet HandleRequest(Packet packet);

        void OnTick(PacketSink sink);
    }
}
