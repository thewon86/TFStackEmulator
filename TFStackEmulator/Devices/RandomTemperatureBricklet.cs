using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    class RandomTemperatureBricklet : EnumeratableDevice
    {
        private const int GET_TEMPERATURE = 1;
        private const int SET_TEMPERATURE_CALLBACK_PERIOD = 2;
        private const int BINDINGS_PING = 128;

        public RandomTemperatureBricklet(UID uid)
            : base(uid)
        {
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            switch (packet.FunctionID)
            {
                case GET_TEMPERATURE:
                    packet.Payload = BitConverter.GetBytes((UInt16)2300); //23°C was randomly chosen ;-)
                    return packet;
                case SET_TEMPERATURE_CALLBACK_PERIOD:
                    //NOP
                    if (packet.ResponseExpected)
                    {
                        return packet;
                    }
                    return null;
                case BINDINGS_PING:
                    return null;
                default:
                    Console.WriteLine("Unknown function called: {0} (response was {1}expected)", packet.FunctionID, packet.ResponseExpected ? "" : "not ");
                    return null;
            }
        }
    }
}
