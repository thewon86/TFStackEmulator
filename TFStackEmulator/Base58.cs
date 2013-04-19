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
    public static class Base58
    {
        private const string BASE58 = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";

        public static int IndexOf(char c, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == c)
                {
                    return i;
                }
            }

            return 0;
        }

        public static string Encode(long value)
        {
            string encoded = "";
            while (value >= 58)
            {
                long div = value / 58;
                int mod = (int)(value % 58);
                encoded = BASE58[mod] + encoded;
                value = div;
            }

            encoded = BASE58[(int)value] + encoded;
            return encoded;
        }

        public static long Decode(string encoded)
        {
            long value = 0;
            long columnMultiplier = 1;
            for (int i = encoded.Length - 1; i >= 0; i--)
            {
                int column = IndexOf(encoded[i], BASE58);
                value += column * columnMultiplier;
                columnMultiplier *= 58;
            }

            return value;
        }
    }
}
