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
        private const int TEMPERATURE_CALLBACK = 8;
        private const int BINDINGS_PING = 128;

        public int TemperatureCallbackPeriod { get; private set; }

        public Int16 CurrentTemperature { get; private set; }

        private Int16 LastCallbackTemperature;
        private long LastCallbackTime;

        private Random Random = new Random();

        public RandomTemperatureBricklet(UID uid)
            : base(uid)
        {
            CurrentTemperature = 2300;
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            switch (packet.FunctionID)
            {
                case GET_TEMPERATURE:
                    packet.Payload = BitConverter.GetBytes(CurrentTemperature);
                    return packet;
                case SET_TEMPERATURE_CALLBACK_PERIOD:
                    TemperatureCallbackPeriod = BitConverter.ToInt32(packet.Payload, 0);
                    ResetTemperatureCallback();
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

        private void ResetTemperatureCallback()
        {
            LastCallbackTemperature = CurrentTemperature;
            LastCallbackTime = Environment.TickCount;
        }

        public override void OnTick(PacketSink sink)
        {
            RandomizeTemperature();
            ProcessTemperatureCallback(sink);
        }

        private void RandomizeTemperature()
        {
            CurrentTemperature += (short)(Random.Next(21) - 10); //TODO: bounding?
        }

        private void ProcessTemperatureCallback(PacketSink sink)
        {
            if (TemperatureCallbackPeriod == 0)
            {
                return;
            }

            long elapsedTime = Environment.TickCount - LastCallbackTime;
            if (elapsedTime > TemperatureCallbackPeriod && CurrentTemperature != LastCallbackTemperature)
            {
                var packet = new Packet(UID, 2, TEMPERATURE_CALLBACK, 0, false);
                packet.Payload = BitConverter.GetBytes(CurrentTemperature);
                sink.SendPacket(packet);

                ResetTemperatureCallback();
            }
        }
    }
}
