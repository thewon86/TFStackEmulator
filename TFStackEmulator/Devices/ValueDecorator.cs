using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public class ValueDecorator<T> : Device
    {
        public UID UID { get; private set; }

        public uint ValueCallbackPeriod { get; private set; }

        public T CurrentValue { get; set; }

        private Device DecoratedDevice;

        private byte FunctionGetValue;

        private byte FunctionSetValueCallbackPeriod;

        private byte FunctionGetValueCallbackPeriod;

        private byte FunctionValueCallback;

        private T LastCallbackValue;
        private long LastCallbackTime;

        public ValueDecorator(UID uid, byte getValue, Device decoratedDevice = null)
            : this(uid, getValue, 0, 0, 0, decoratedDevice)
        {
        }

        public ValueDecorator(UID uid, byte getValue, byte setCBPeriod, byte getCBPeriod, byte valueCB, Device decoratedDevice = null)
        {
            UID = uid;
            FunctionGetValue = getValue;
            FunctionSetValueCallbackPeriod = setCBPeriod;
            FunctionGetValueCallbackPeriod = getCBPeriod;
            FunctionValueCallback = valueCB;
            DecoratedDevice = decoratedDevice;
        }

        public Packet HandleRequest(Packet packet)
        {
            if (packet.FunctionID == FunctionGetValue)
            {
                packet.Payload = LEConverter.GetConverter<T>().GetBytes(CurrentValue);
                return packet;
            }
            if (packet.FunctionID == FunctionSetValueCallbackPeriod)
            {
                ValueCallbackPeriod = BitConverter.ToUInt32(packet.Payload, 0);
                ResetValueCallback();
                if (packet.ResponseExpected)
                {
                    packet.PayloadSize = 0;
                    return packet;
                }
            }
            if (packet.FunctionID == FunctionGetValueCallbackPeriod)
            {
                packet.Payload = BitConverter.GetBytes(ValueCallbackPeriod);
                return packet;
            }

            if (DecoratedDevice != null)
            {
                return DecoratedDevice.HandleRequest(packet);
            }

            Console.WriteLine("Unhandled Function: {0}", packet.FunctionID);
            return null;
        }

        public void OnTick(PacketSink sink)
        {
            ProcessValueCallback(sink);
            if (DecoratedDevice != null)
            {
                DecoratedDevice.OnTick(sink);
            }
        }

        private void ResetValueCallback()
        {
            LastCallbackValue = CurrentValue;
            LastCallbackTime = Environment.TickCount;
        }

        private void ProcessValueCallback(PacketSink sink)
        {
            if (ValueCallbackPeriod == 0)
            {
                return;
            }

            long elapsedTime = Environment.TickCount - LastCallbackTime;
            if (elapsedTime > ValueCallbackPeriod && !CurrentValue.Equals(LastCallbackValue))
            {
                var packet = new Packet(UID, 0, FunctionValueCallback, 0, false);
                packet.Payload = LEConverter.GetConverter<T>().GetBytes(CurrentValue);
                sink.SendPacket(packet);

                ResetValueCallback();
            }
        }
    }
}
