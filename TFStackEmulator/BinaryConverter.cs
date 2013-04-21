using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator
{
    interface BinaryConverter<T>
    {
        byte[] GetBytes(T value);
        void GetValue(byte[] buffer, out T value);
    }
}
