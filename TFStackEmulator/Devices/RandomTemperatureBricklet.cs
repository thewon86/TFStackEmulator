using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    class RandomTemperatureBricklet : EnumeratableDevice
    {
        private Int16ValueDecorator Temperature;

        private Random Random = new Random();

        public RandomTemperatureBricklet(UID uid)
            : base(uid, DeviceIdentifier.BrickletTemperature)
        {
            Temperature = new Int16ValueDecorator(UID, 1, 2, 3, 8);
            Temperature.CurrentValue = 2300;
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            return Temperature.HandleRequest(packet);
        }

        public override void OnTick(PacketSink sink)
        {
            RandomizeTemperature();
            Temperature.OnTick(sink);
        }

        private void RandomizeTemperature()
        {
            Temperature.CurrentValue += (short)(Random.Next(21) - 10); //TODO: bounding?
        }
    }
}
