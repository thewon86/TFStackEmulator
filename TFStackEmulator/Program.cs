using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TFStackEmulator.Devices;

namespace TFStackEmulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var emulator = new StackEmulator();
            var device = new RandomBarometerBricklet(new UID("myu1d"));
            var device2 = new RandomAmbientLightBricklet(new UID("myu2d"));
            device2.Position = 'b';
            var device3 = new RandomTemperatureBricklet(new UID("myu3d"));
            device3.Position = 'c';
            emulator.AddDevice(device);
            emulator.AddDevice(device2);
            emulator.AddDevice(device3);

            emulator.Start();

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(IPAddress.Any, 4224));
            sock.Listen(3);
            while (true)
	        {
                var client = sock.Accept();
                var connector = new NetworkStackConnector(client, emulator);
                connector.Start();
	        }
        }
    }
}
