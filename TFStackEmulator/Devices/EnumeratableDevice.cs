using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public abstract class EnumeratableDevice : Device
    {
        private const int ENUMERATE_FUNCTIONID = 254;
        private const int ENUMERATE_CALLBACK_FUNCTIONID = 253;

        public UID UID { get; private set; }

        protected EnumeratableDevice(UID uid)
        {
            UID = uid;
        }

        protected abstract Packet OnUnhandledRequest(Packet packet);

        public Packet HandleRequest(Packet packet)
        {
            if (packet.FunctionID == ENUMERATE_FUNCTIONID)
            {
                return CreateEnumerateCallback();
            }
            else
            {
                return OnUnhandledRequest(packet);
            }
        }

        private Packet CreateEnumerateCallback()
        {
            //TODO: properties for callback-values
            var callbackPacket = new Packet(UID, 26, ENUMERATE_CALLBACK_FUNCTIONID, 0, true);
            using (MemoryStream stream = new MemoryStream(callbackPacket.Payload))
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
            {
                char[] myUID = new char[8];
                char[] myShortUID = UID.ToString().ToCharArray();
                Array.Copy(myShortUID, myUID, myShortUID.Length);
                char[] connectedUID = new char[8];
                writer.Write(myUID);
                writer.Write(connectedUID);
                writer.Write('a');
                writer.Write((byte)1);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)1);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((UInt16)216);
                writer.Write((byte)0);
            }
            return callbackPacket;
        }
    }
}
