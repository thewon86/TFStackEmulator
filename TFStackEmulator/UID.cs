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
    public struct UID
    {
        private string StringRepresentation;
        private int IntRepresentation;

        public UID(int uid)
        {
            IntRepresentation = uid;
            StringRepresentation = Base58.Encode(uid);
        }

        public UID(string uid)
        {
            StringRepresentation = uid;
            long uidTmp = Base58.Decode(uid);
            if (uidTmp > 0xFFFFFFFFL)
            {
                // convert from 64bit to 32bit
                long value1 = uidTmp & 0xFFFFFFFFL;
                long value2 = (uidTmp >> 32) & 0xFFFFFFFFL;

                uidTmp = (value1 & 0x00000FFFL);
                uidTmp |= (value1 & 0x0F000000L) >> 12;
                uidTmp |= (value2 & 0x0000003FL) << 16;
                uidTmp |= (value2 & 0x000F0000L) << 6;
                uidTmp |= (value2 & 0x3F000000L) << 2;
            }

            IntRepresentation = (int)uidTmp;

            if (StringRepresentation != Base58.Encode(IntRepresentation))
            {
                throw new ArgumentException("Invalid UID, it is probably too long.");
            }
        }

        public int ToInt()
        {
            return IntRepresentation;
        }

        public static explicit operator int(UID uid)
        {
            return uid.ToInt();
        }

        public override string ToString()
        {
            return StringRepresentation;
        }
    }
}
