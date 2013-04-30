using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public class RandomAmbientLightBricklet : EnumeratableDevice
    {
        private Random Random = new Random();

        private ValueDecorator<UInt16> Illuminance;
        private ValueDecorator<UInt16> AnalogValue;
        private Device Decorators;

        public RandomAmbientLightBricklet(UID uid)
            : base(uid, DeviceIdentifier.BrickletAmbientLight)
        {
            AnalogValue = new ValueDecorator<UInt16>(UID, 2, 5, 6, 14);
            Illuminance = new ValueDecorator<UInt16>(UID, 1, 3, 4, 13, AnalogValue);
            Decorators = Illuminance;

            Illuminance.CurrentValue = 5000;
        }

        protected override Packet DoHandleRequest(Packet packet)
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
