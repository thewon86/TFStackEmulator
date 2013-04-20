using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    class Int16ValueDecorator : SingleValueDecorator<Int16>
    {
        public Int16ValueDecorator(UID uid, byte getValue, byte setCBPeriod, byte valueCB, Device decoratedDevice = null)
            : base(uid, getValue, setCBPeriod, valueCB, decoratedDevice)
        {
        }

        protected override byte[] GetBytesForValue(short value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}