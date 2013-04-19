using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TFStackEmulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(IPAddress.Any, 4224));
            sock.Listen(3);
            while (true)
	        {
                var client = sock.Accept();
                var emulator = new StackEmulator(client);
                Thread t = new Thread(emulator.ServeClient);
                t.Start();
	        }
        }
    }
}
