using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public class UInt16ValueDecorator : SingleValueDecorator<UInt16>
    {
        public UInt16ValueDecorator(UID uid, byte getValue, Device decoratedDevice = null)
            : base(uid, getValue, decoratedDevice)
        {
        }

        public UInt16ValueDecorator(UID uid, byte getValue, byte setCBPeriod, byte getCBPeriod, byte valueCB, Device decoratedDevice = null)
            : base(uid, getValue, setCBPeriod, getCBPeriod, valueCB, decoratedDevice)
        {
        }

        protected override byte[] GetBytesForValue(ushort value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
