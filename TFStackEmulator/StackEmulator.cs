using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFStackEmulator.Devices;

namespace TFStackEmulator
{
    //TODO: device-simulation (for regular callbacks)
    public class StackEmulator
    {
        public static readonly UID BroadcastUID = new UID(0);

        private Dictionary<UID, Device> Devices = new Dictionary<UID, Device>();

        public event ResponseEventHandler Response;
        public delegate void ResponseEventHandler(object sender, ResponseEventArgs args);

        public StackEmulator()
        {
            //TODO: extract test-setup to the outside
            var device = new RandomTemperatureBricklet(new UID("myu1d"));
            var device2 = new RandomTemperatureBricklet(new UID("myu2d"));
            AddDevice(device);
            AddDevice(device2);
        }

        public void AddDevice(Device device)
        {
            Devices.Add(device.UID, device);
        }

        public void HandleRequest(Packet packet)
        {
            RouteAndDispatchRequest(packet);
        }

        private void RouteAndDispatchRequest(Packet packet)
        {
            if (packet.UID.Equals(BroadcastUID))
            {
                foreach (Device device in Devices.Values)
                {
                    DispatchRequest(packet, device);
                }
            }
            else
            {
                Device device;
                if (Devices.TryGetValue(packet.UID, out device))
                {
                    DispatchRequest(packet, device);
                }
                else
                {
                    Console.WriteLine("No Device for UID {0}", packet.UID);
                }
            }
        }

        private void DispatchRequest(Packet packet, Device device)
        {
            Packet response = device.HandleRequest(packet.Copy());
            if (response != null)
            {
                OnResponse(response);
            }
        }

        protected void OnResponse(Packet response)
        {
            var handler = Response;
            if (handler != null)
            {
                handler(this, new ResponseEventArgs(response));
            }
        }
    }

    public class ResponseEventArgs : EventArgs
    {
        public Packet Response { get; private set; }

        public ResponseEventArgs(Packet response)
        {
            Response = response;
        }
    }
}
