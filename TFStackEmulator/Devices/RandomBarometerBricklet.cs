using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public class RandomBarometerBricklet : EnumeratableDevice
    {
        private Device Decorators;
        private ValueDecorator<Int32> Altitude;
        private ValueDecorator<Int32> AirPressure;
        private ValueDecorator<Int16> Temperature;

        private Random Random = new Random();

        public RandomBarometerBricklet(UID uid)
            : base(uid, DeviceIdentifier.BrickletBarometer)
        {
            AirPressure = new ValueDecorator<Int32>(UID, 1, 3, 4, 15);
            Altitude = new ValueDecorator<Int32>(UID, 2, 5, 6, 16, AirPressure);
            Temperature = new ValueDecorator<Int16>(UID, 14, Altitude);
            Decorators = Temperature;

            AirPressure.CurrentValue = 200000;
            CalculateAltitude();
            Temperature.CurrentValue = 2300;
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            return Decorators.HandleRequest(packet);
        }

        public override void OnTick(PacketSink sink)
        {
            RandomizeValues();
            Decorators.OnTick(sink);
        }

        private void RandomizeValues()
        {
            AirPressure.CurrentValue += Random.Next(21) - 10;
            CalculateAltitude();

            Temperature.CurrentValue += (short)(Random.Next(21) - 10);
        }

        private void CalculateAltitude()
        {
            Altitude.CurrentValue = 1000000 - AirPressure.CurrentValue; //TODO: calculate this correctly
        }
    }
}
