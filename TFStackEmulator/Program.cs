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
            var device = new RandomTemperatureBricklet(new UID("myu1d"));
            var device2 = new RandomTemperatureBricklet(new UID("myu2d"));
            device2.Position = 'b';
            emulator.AddDevice(device);
            emulator.AddDevice(device2);

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
