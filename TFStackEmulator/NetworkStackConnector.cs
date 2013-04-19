using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TFStackEmulator
{
    public class NetworkStackConnector
    {
        private NetworkStream Stream;
        private Socket ClientSocket;
        private StackEmulator Emulator;

        private Thread RequestHandler;

        public NetworkStackConnector(Socket clientSocket, StackEmulator emulator)
        {
            ClientSocket = clientSocket;
            Emulator = emulator;
            Stream = new NetworkStream(clientSocket);
            RequestHandler = new Thread(RequestLoop);

            ClientSocket.NoDelay = true;
            RequestHandler.Name = "NetworkStackConnector";
            RequestHandler.IsBackground = true;
            Emulator.Response += Emulator_Response;
        }

        private void Emulator_Response(object sender, ResponseEventArgs args)
        {
            args.Response.WriteTo(Stream);
            Stream.Flush();
        }

        public void Start()
        {
            if (RequestHandler.IsAlive)
            {
                throw new InvalidOperationException("Connector already started");
            }

            RequestHandler.Start();
        }

        private void RequestLoop()
        {
            Console.WriteLine("Serving client...");
            try
            {
                HandleRequests();
            }
            catch
            {
                Console.WriteLine("Connection aborted!");
            }
            finally
            {
                Stream.Dispose();
                TryDisconnect();
            }
        }

        private void HandleRequests()
        {
            while (true)
            {
                var packet = Packet.ReadFrom(Stream);

                Emulator.HandleRequest(packet);
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
