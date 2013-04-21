using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator
{
    //TODO: always do a little endian conversion
    internal class LEConverter : BinaryConverter<Int32>, BinaryConverter<UInt32>, BinaryConverter<Int16>, BinaryConverter<UInt16>
    {
        private static LEConverter Instance;

        /// <summary>
        /// This works for all supported types and will result in an error for unsupported types
        /// </summary>
        public static BinaryConverter<T> GetConverter<T>()
        {
            return (BinaryConverter<T>) Instance;
        }

        public static LEConverter GetInstance()
        {
            return Instance;
        }

        static LEConverter()
        {
            Instance = new LEConverter();
        }

        private LEConverter()
        {
        }

        public byte[] GetBytes(Int32 data)
        {
            return BitConverter.GetBytes(data);
        }

        public byte[] GetBytes(Int16 data)
        {
            return BitConverter.GetBytes(data);
        }

        public byte[] GetBytes(UInt16 data)
        {
            return BitConverter.GetBytes(data);
        }

        public byte[] GetBytes(UInt32 data)
        {
            return BitConverter.GetBytes(data);
        }

        public void GetValue(byte[] data, out Int32 result)
        {
            result = BitConverter.ToInt32(data, 0);
        }

        public void GetValue(byte[] data, out Int16 result)
        {
            result = BitConverter.ToInt16(data, 0);
        }

        public void GetValue(byte[] data, out UInt32 result)
        {
            result = BitConverter.ToUInt32(data, 0);
        }

        public void GetValue(byte[] data, out UInt16 result)
        {
            result = BitConverter.ToUInt16(data, 0);
        }
    }
}
