using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    class Int32ValueDecorator : SingleValueDecorator<Int32>
    {
        public Int32ValueDecorator(UID uid, byte getValue, byte setCBPeriod, byte getCBPeriod, byte valueCB, Device decoratedDevice = null)
            : base(uid, getValue, setCBPeriod, getCBPeriod, valueCB, decoratedDevice)
        {
        }

        protected override byte[] GetBytesForValue(int value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
