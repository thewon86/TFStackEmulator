using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TFStackEmulator
{
    public class Packet
    {
        public const byte HeaderSize = 8;

        public UID UID { get; private set; }

        public byte PayloadSize
        {
            get
            {
                return (byte)Payload.Length;
            }
            set
            {
                Payload = new byte[(byte)value];
            }
        }

        public byte[] Payload { get; set; }

        public byte FunctionID { get; set; }

        public int SequenceNumber { get; set; }

        public bool ResponseExpected { get; set; }

        public byte ErrorCode { get; set; }

        public Packet(UID uid, byte payloadLength, byte functionId, int sequenceNumber, bool responseExpected, byte errorCode = 0)
        {
            UID = uid;
            PayloadSize = payloadLength;
            FunctionID = functionId;
            SequenceNumber = sequenceNumber;
            ResponseExpected = responseExpected;
            ErrorCode = errorCode;
        }

        private Packet(byte[] data)
        {
            UID = new UID(LEConverter.IntFrom(0, data));
            PayloadSize = (byte)(data[4] - HeaderSize);
            FunctionID = data[5];
            SequenceNumber = (byte)((((int)data[6]) >> 4) & 0x0F);
            ErrorCode = (byte)(((int)(data[7] >> 6)) & 0x03);
            ResponseExpected = (((data[6]) >> 3) & 0x01) == 1;
        }

        public void WriteTo(Stream stream)
        {
            byte[] uidBuffer = new byte[4];
            LEConverter.To((int)UID, 0, uidBuffer);
            stream.Write(uidBuffer, 0, uidBuffer.Length);

            stream.WriteByte((byte)(HeaderSize + PayloadSize));
            stream.WriteByte((byte)(FunctionID));

            byte sequenceResponseByte = (byte)(((byte)(ResponseExpected ? 1 : 0) << 3) | ((SequenceNumber << 4) & 0xF7));
            stream.WriteByte(sequenceResponseByte);

            stream.WriteByte((byte)(ErrorCode << 6));

            stream.Write(Payload, 0, PayloadSize);
        }

        public Packet Copy()
        {
            Packet copy = new Packet(UID, PayloadSize, FunctionID, SequenceNumber, ResponseExpected, ErrorCode);
            Array.Copy(Payload, copy.Payload, PayloadSize);
            return copy;
        }

        public override string ToString()
        {
            return string.Format("Packet(\"{0}\" fn: {1} seq: {2} resp: {3} err: {4})[{5}]", UID, FunctionID, SequenceNumber, ResponseExpected, ErrorCode, PayloadSize);
        }

        public static Packet ReadFrom(Stream stream)
        {
            byte[] header = new byte[HeaderSize];
            FillBuffer(stream, header);

            var packet = new Packet(header);
            byte[] payload = FillBuffer(stream, packet.Payload);

            return packet;
        }

        private static byte[] FillBuffer(Stream stream, byte[] buffer)
        {
            int read = 0;

            while (read < buffer.Length)
            {
                int newBytes = stream.Read(buffer, read, buffer.Length - read);
                if (newBytes == 0)
                {
                    throw new IOException("Stream was closed before enough bytes were read");
                }
                read += newBytes;
            }

            return buffer;
        }
    }
}
