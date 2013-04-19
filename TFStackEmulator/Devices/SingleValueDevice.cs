using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFStackEmulator.Devices
{
    public abstract class SingleValueDevice<T> : EnumeratableDevice where T : struct
    {
        private const int BINDINGS_PING = 128;

        public int ValueCallbackPeriod { get; private set; }

        public T CurrentValue { get; protected set; }

        protected abstract byte FunctionGetValue { get; }

        protected abstract byte FunctionSetValueCallbackPeriod { get; }

        protected abstract byte FunctionValueCallback { get; }

        private T LastCallbackValue;
        private long LastCallbackTime;

        protected SingleValueDevice(UID uid, UInt16 deviceIdentifier)
            : base(uid, deviceIdentifier)
        {
        }

        protected override Packet OnUnhandledRequest(Packet packet)
        {
            if (packet.FunctionID == FunctionGetValue)
            {
                packet.Payload = GetBytesForValue(CurrentValue);
                return packet;
            }
            if (packet.FunctionID == FunctionSetValueCallbackPeriod)
            {
                ValueCallbackPeriod = BitConverter.ToInt32(packet.Payload, 0);
                ResetValueCallback();
                if (packet.ResponseExpected)
                {
                    packet.PayloadSize = 0;
                    return packet;
                }
            }

            return null;
        }

        protected abstract byte[] GetBytesForValue(T value);

        public override void OnTick(PacketSink sink)
        {
            ProcessTemperatureCallback(sink);
        }

        private void ResetValueCallback()
        {
            LastCallbackValue = CurrentValue;
            LastCallbackTime = Environment.TickCount;
        }

        private void ProcessTemperatureCallback(PacketSink sink)
        {
            if (ValueCallbackPeriod == 0)
            {
                return;
            }

            long elapsedTime = Environment.TickCount - LastCallbackTime;
            if (elapsedTime > ValueCallbackPeriod && !CurrentValue.Equals(LastCallbackValue))
            {
                var packet = new Packet(UID, 0, FunctionValueCallback, 0, false);
                packet.Payload = GetBytesForValue(CurrentValue);
                sink.SendPacket(packet);

                ResetValueCallback();
            }
        }
    }
}
