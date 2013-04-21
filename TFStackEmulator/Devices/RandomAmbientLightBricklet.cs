using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public class RandomAmbientLightBricklet : EnumeratableDevice
    {
        private Random Random = new Random();

        private UInt16ValueDecorator Illuminance;

        public RandomAmbientLightBricklet(UID uid)
            : base(uid, DeviceIdentifier.BrickletAmbientLight)
        {
            Illuminance = new UInt16ValueDecorator(UID, 1, 3, 13);
            Illuminance.CurrentValue = 5000;
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            return Illuminance.HandleRequest(packet);
        }

        public override void OnTick(PacketSink sink)
        {
            if (Random.Next(2) == 0)
            {
                Illuminance.CurrentValue++;
            }
            else
            {
                Illuminance.CurrentValue--;
            }

            Illuminance.OnTick(sink);
        }
    }
}
