using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public abstract class EnumeratableDevice : Device
    {
        private const int GET_IDENTITY_FUNCTIONID = 255;
        private const int ENUMERATE_FUNCTIONID = 254;
        private const int ENUMERATE_CALLBACK_FUNCTIONID = 253;

        public UID UID { get; private set; }

        public UID ConnectedUID { get; set; }

        public char Position { get; set; }

        public Version HardwareVersion { get; set; }

        public Version FirmwareVersion { get; set; }

        public DeviceIdentifier DeviceIdentifier { get; private set; }

        protected EnumeratableDevice(UID uid, DeviceIdentifier deviceIdentifier)
        {
            UID = uid;
            DeviceIdentifier = deviceIdentifier;
            Position = 'a'; //TODO: better default depending on brick/bricklet?
            ConnectedUID = new UID(0);
            HardwareVersion = new Version(1, 0, 0);
            FirmwareVersion = new Version(1, 0, 0);
        }

        protected abstract Packet DoHandleRequest(Packet packet);

        public Packet HandleRequest(Packet packet)
        {
            if (packet.FunctionID == ENUMERATE_FUNCTIONID)
            {
                return CreateEnumerateCallback();
            }
            else if (packet.FunctionID == GET_IDENTITY_FUNCTIONID)
            {
                packet.PayloadSize = 25;
                CreateIdentificationPayload(packet);
                return packet;
            }
            else
            {
                return DoHandleRequest(packet);
            }
        }

        private Packet CreateEnumerateCallback()
        {
            var callbackPacket = new Packet(UID, 26, ENUMERATE_CALLBACK_FUNCTIONID, 0, true);
            CreateIdentificationPayload(callbackPacket);
            return callbackPacket;
        }

        /// <summary>
        /// Can be useed for GetIdentity-calls and enumerate-callback
        /// Payload-Length must be set (enumerate-callback is 1 Byte longer)
        /// </summary>
        private void CreateIdentificationPayload(Packet packet)
        {
            using (MemoryStream stream = new MemoryStream(packet.Payload))
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
            {
                char[] myUID = CreateUIDCharArray(UID);
                char[] connectedUID = CreateUIDCharArray(ConnectedUID);
                writer.Write(myUID);
                writer.Write(connectedUID);
                writer.Write(Position);
                writer.Write(HardwareVersion.Major);
                writer.Write(HardwareVersion.Minor);
                writer.Write(HardwareVersion.Revision);
                writer.Write(FirmwareVersion.Major);
                writer.Write(FirmwareVersion.Minor);
                writer.Write(FirmwareVersion.Revision);
                writer.Write((UInt16)DeviceIdentifier);
            }
        }

        private char[] CreateUIDCharArray(UID uid)
        {
            //TODO: there has to be a better way...
            char[] fullArray = new char[8];
            char[] smallArray = uid.ToString().ToCharArray();
            Array.Copy(smallArray, fullArray, smallArray.Length);
            return fullArray;
        }

        public abstract void OnTick(PacketSink sink);
    }
}
