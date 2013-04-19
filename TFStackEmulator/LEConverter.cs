/*
 * Extracted from Tinkerforge/generators source code:
 * 
 * Copyright (C) 2012 Matthias Bolte <matthias@tinkerforge.com>
 * Copyright (C) 2011-2012 Olaf Lüke <olaf@tinkerforge.com>
 *
 * Redistribution and use in source and binary forms of this file,
 * with or without modification, are permitted.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator
{
    internal static class LEConverter
    {
        static public void To(string data, int position, int len, byte[] array)
        {
            for (int i = 0; i < Math.Min(len, data.Length); i++)
            {
                array[position + i] = (byte)data[i];
            }

            for (int i = Math.Min(len, data.Length); i < len; i++)
            {
                array[position + i] = 0;
            }
        }

        static public void To(long data, int position, byte[] array)
        {
            array[position + 0] = (byte)data;
            array[position + 1] = (byte)(((ulong)data >> 8) & 0xFF);
            array[position + 2] = (byte)(((ulong)data >> 16) & 0xFF);
            array[position + 3] = (byte)(((ulong)data >> 24) & 0xFF);
            array[position + 4] = (byte)(((ulong)data >> 32) & 0xFF);
            array[position + 5] = (byte)(((ulong)data >> 40) & 0xFF);
            array[position + 6] = (byte)(((ulong)data >> 48) & 0xFF);
            array[position + 7] = (byte)(((ulong)data >> 56) & 0xFF);
        }

        static public void To(long[] data, int position, int len, byte[] array)
        {
            for (int i = 0; i < len; i++)
            {
                To(data[i], position + i, array);
            }
        }

        static public void To(int data, int position, byte[] array)
        {
            array[position + 0] = (byte)data;
            array[position + 1] = (byte)(((uint)data >> 8) & 0xFF);
            array[position + 2] = (byte)(((uint)data >> 16) & 0xFF);
            array[position + 3] = (byte)(((uint)data >> 24) & 0xFF);
        }

        static public void To(int[] data, int position, int len, byte[] array)
        {
            for (int i = 0; i < len; i++)
            {
                To(data[i], position + i, array);
            }
        }

        static public void To(short data, int position, byte[] array)
        {
            array[position + 0] = (byte)data;
            array[position + 1] = (byte)(((ushort)data >> 8) & 0xFF);
        }

        static public void To(short[] data, int position, int len, byte[] array)
        {
            for (int i = 0; i < len; i++)
            {
                To(data[i], position + i, array);
            }
        }

        static public void To(byte data, int position, byte[] array)
        {
            array[position + 0] = (byte)data;
        }

        static public void To(byte[] data, int position, int len, byte[] array)
        {
            for (int i = 0; i < len; i++)
            {
                To(data[i], position + i, array);
            }
        }

        static public void To(char data, int position, byte[] array)
        {
            array[position + 0] = (byte)data;
        }

        static public void To(char[] data, int position, int len, byte[] array)
        {
            for (int i = 0; i < len; i++)
            {
                To(data[i], position + i, array);
            }
        }

        static public void To(bool data, int position, byte[] array)
        {
            if (data)
            {
                array[position + 0] = 1;
            }
            else
            {
                array[position + 0] = 0;
            }
        }

        static public void To(bool[] data, int position, int len, byte[] array)
        {
            for (int i = 0; i < len; i++)
            {
                To(data[i], position + i, array);
            }
        }



        static public bool BoolFrom(int position, byte[] array)
        {
            return array[position] != 0;
        }

        static public bool[] BoolArrayFrom(int position, byte[] array, int len)
        {
            bool[] ret = new bool[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = BoolFrom(position + i * 1, array);
            }

            return ret;
        }

        static public char CharFrom(int position, byte[] array)
        {
            return (char)array[position];
        }

        static public char[] CharArrayFrom(int position, byte[] array, int len)
        {
            char[] ret = new char[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = CharFrom(position + i * 1, array);
            }

            return ret;
        }

        static public short SByteFrom(int position, byte[] array)
        {
            return (short)array[position];
        }

        static public short[] SByteArrayFrom(int position, byte[] array, int len)
        {
            short[] ret = new short[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = SByteFrom(position + i * 1, array);
            }

            return ret;
        }

        static public byte ByteFrom(int position, byte[] array)
        {
            return (byte)array[position];
        }

        static public byte[] ByteArrayFrom(int position, byte[] array, int len)
        {
            byte[] ret = new byte[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = ByteFrom(position + i * 1, array);
            }

            return ret;
        }

        static public short ShortFrom(int position, byte[] array)
        {
            return (short)((ushort)array[position + 0] << 0 |
                           (ushort)array[position + 1] << 8);
        }

        static public short[] ShortArrayFrom(int position, byte[] array, int len)
        {
            short[] ret = new short[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = ShortFrom(position + i * 2, array);
            }

            return ret;
        }

        static public int UShortFrom(int position, byte[] array)
        {
            return (int)((int)array[position + 0] << 0 |
                         (int)array[position + 1] << 8);
        }

        static public int[] UShortArrayFrom(int position, byte[] array, int len)
        {
            int[] ret = new int[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = UShortFrom(position + i * 2, array);
            }

            return ret;
        }

        static public int IntFrom(int position, byte[] array)
        {
            return (int)((int)array[position + 0] << 0 |
                         (int)array[position + 1] << 8 |
                         (int)array[position + 2] << 16 |
                         (int)array[position + 3] << 24);
        }

        static public int[] IntArrayFrom(int position, byte[] array, int len)
        {
            int[] ret = new int[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = IntFrom(position + i * 4, array);
            }

            return ret;
        }

        static public long UIntFrom(int position, byte[] array)
        {
            return (long)((long)array[position + 0] << 0 |
                          (long)array[position + 1] << 8 |
                          (long)array[position + 2] << 16 |
                          (long)array[position + 3] << 24);
        }

        static public long[] UIntArrayFrom(int position, byte[] array, int len)
        {
            long[] ret = new long[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = UIntFrom(position + i * 4, array);
            }

            return ret;
        }

        static public long LongFrom(int position, byte[] array)
        {
            return (long)((long)array[position + 0] << 0 |
                          (long)array[position + 1] << 8 |
                          (long)array[position + 2] << 16 |
                          (long)array[position + 3] << 24 |
                          (long)array[position + 4] << 32 |
                          (long)array[position + 5] << 40 |
                          (long)array[position + 6] << 48 |
                          (long)array[position + 7] << 56);
        }

        static public long[] LongArrayFrom(int position, byte[] array, int len)
        {
            long[] ret = new long[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = LongFrom(position + i * 8, array);
            }

            return ret;
        }

        static public long ULongFrom(int position, byte[] array)
        {
            return (long)((long)array[position + 0] << 0 |
                          (long)array[position + 1] << 8 |
                          (long)array[position + 2] << 16 |
                          (long)array[position + 3] << 24 |
                          (long)array[position + 4] << 32 |
                          (long)array[position + 5] << 40 |
                          (long)array[position + 6] << 48 |
                          (long)array[position + 7] << 56);
        }

        static public long[] ULongArrayFrom(int position, byte[] array, int len)
        {
            long[] ret = new long[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = ULongFrom(position + i * 8, array);
            }

            return ret;
        }

        static public float FloatFrom(int position, byte[] array)
        {
            // We need Little Endian
            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToSingle(array, position);
            }
            else
            {
                byte[] array_tmp = new byte[4];
                array_tmp[3] = array[position + 0];
                array_tmp[2] = array[position + 1];
                array_tmp[1] = array[position + 2];
                array_tmp[0] = array[position + 3];
                return BitConverter.ToSingle(array_tmp, 0);
            }
        }

        static public float[] FloatArrayFrom(int position, byte[] array, int len)
        {
            float[] ret = new float[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = FloatFrom(position + i * 4, array);
            }

            return ret;
        }

        static public string StringFrom(int position, byte[] array, int len)
        {
            StringBuilder sb = new StringBuilder(len);
            for (int i = position; i < position + len; i++)
            {
                if (array[i] == 0)
                {
                    break;
                }
                sb.Append((char)array[i]);
            }

            return sb.ToString();
        }

    }
}
