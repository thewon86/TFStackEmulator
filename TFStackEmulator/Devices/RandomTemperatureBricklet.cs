using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    class RandomTemperatureBricklet : EnumeratableDevice
    {
        public RandomTemperatureBricklet(UID uid)
            : base(uid)
        {
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            switch (packet.FunctionID)
            {
                case 1:
                    packet.Payload = BitConverter.GetBytes((UInt16)2300); //23°C was randomly chosen ;-)
                    return packet;
                default:
                    return null;
            }
        }
    }
}
