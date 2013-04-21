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
        private UInt16ValueDecorator AnalogValue;
        private Device Decorators;

        public RandomAmbientLightBricklet(UID uid)
            : base(uid, DeviceIdentifier.BrickletAmbientLight)
        {
            AnalogValue = new UInt16ValueDecorator(UID, 2, 5, 14);
            Illuminance = new UInt16ValueDecorator(UID, 1, 3, 13, AnalogValue);
            Decorators = Illuminance;

            Illuminance.CurrentValue = 5000;
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            return Decorators.HandleRequest(packet);
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

            AnalogValue.CurrentValue = Illuminance.CurrentValue; //TODO: more realistic implementation

            Decorators.OnTick(sink);
        }
    }
}
