using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public class RandomAmbientLightBricklet : SingleValueDevice<UInt16>
    {
        private Random Random = new Random();

        protected override byte FunctionGetValue { get { return 1; } }

        protected override byte FunctionSetValueCallbackPeriod { get { return 3; } }

        protected override byte FunctionValueCallback { get { return 13; } }

        public RandomAmbientLightBricklet(UID uid)
            : base(uid, 21)
        {
            CurrentValue = 5000;
        }

        protected override byte[] GetBytesForValue(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        public override void OnTick(PacketSink sink)
        {
            if (Random.Next(2) == 0)
            {
                CurrentValue++;
            }
            else
            {
                CurrentValue--;
            }

            base.OnTick(sink);
        }
    }
}
