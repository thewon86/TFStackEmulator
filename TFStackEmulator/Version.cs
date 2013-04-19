using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator
{
    public struct Version
    {
        public byte Major;
        public byte Minor;
        public byte Revision;

        public Version(byte major, byte minor, byte revision)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
        }
    }
}
