using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    class Int16ValueDecorator : SingleValueDecorator<Int16>
    {
        public Int16ValueDecorator(UID uid, byte getValue, Device decoratedDevice = null)
            : base(uid, getValue, decoratedDevice)
        {
        }

        public Int16ValueDecorator(UID uid, byte getValue, byte setCBPeriod, byte getCBPeriod, byte valueCB, Device decoratedDevice = null)
            : base(uid, getValue, setCBPeriod, getCBPeriod, valueCB, decoratedDevice)
        {
        }

        protected override byte[] GetBytesForValue(short value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}