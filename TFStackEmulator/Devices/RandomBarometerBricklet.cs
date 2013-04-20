using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public class RandomBarometerBricklet : EnumeratableDevice
    {
        private Device Decorators;
        private Int32ValueDecorator Altitude;
        private Int32ValueDecorator AirPressure;
        private Int16ValueDecorator Temperature;

        private Random Random = new Random();

        public RandomBarometerBricklet(UID uid)
            : base(uid, 221)
        {
            AirPressure = new Int32ValueDecorator(UID, 1, 3, 15);
            Altitude = new Int32ValueDecorator(UID, 2, 5, 16, AirPressure);
            Temperature = new Int16ValueDecorator(UID, 14, 0, 0, Altitude);
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
