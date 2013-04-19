using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using TFStackEmulator.Devices;

namespace TFStackEmulator
{
    //TODO: split networking from device-simulation
    //TODO: device-simulation (for regular callbacks)
    public class StackEmulator
    {
        public static readonly UID BroadcastUID = new UID(0);

        private NetworkStream Stream;
        private Socket ClientSocket;
        private Dictionary<UID, Device> Devices = new Dictionary<UID,Device>();

        public StackEmulator(Socket clientSocket)
        {
            ClientSocket = clientSocket;
            ClientSocket.NoDelay = true;

            Stream = new NetworkStream(clientSocket);

            //TODO: extract test-setup to the outside
            var device = new RandomTemperatureBricklet(new UID("myu1d"));
            var device2 = new RandomTemperatureBricklet(new UID("myu2d"));
            Devices.Add(device.UID, device);
            Devices.Add(device2.UID, device2);
        }

        public void ServeClient()
        {
            Console.WriteLine("Serving client...");
            try
            {
                while (true)
                {
                    var packet = Packet.ReadFrom(Stream);
                    Device device;
                    if (Devices.TryGetValue(packet.UID, out device))
                    {
                        DispatchPacket(packet, device);
                    }
                    else if (packet.UID.Equals(BroadcastUID))
                    {
                        foreach (Device currentDevice in Devices.Values)
                        {
                            DispatchPacket(packet, currentDevice);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Device for UID {0}", packet.UID);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Stream.Dispose();
                TryDisconnect();
            }
        }

        private void DispatchPacket(Packet packet, Device device)
        {
            Packet response = device.HandleRequest(packet.Copy());
            if (response != null)
            {
                response.WriteTo(Stream);
                Stream.Flush();
            }
        }

        private void TryDisconnect()
        {
            try
            {
                ClientSocket.Disconnect(false);
            }
            catch
            {
            }
        }
    }
}
