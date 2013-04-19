using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using TFStackEmulator.Devices;

namespace TFStackEmulator
{
    public class NetworkStackConnector
    {
        private NetworkStream Stream;
        private Socket ClientSocket;

        //TODO: extract emulator to outside (possibility to have multiple connectors per emulator)
        private StackEmulator Emulator = new StackEmulator();

        public NetworkStackConnector(Socket clientSocket)
        {
            ClientSocket = clientSocket;
            ClientSocket.NoDelay = true;

            Stream = new NetworkStream(clientSocket);

            Emulator.Response += Emulator_Response;
        }

        private void Emulator_Response(object sender, ResponseEventArgs args)
        {
            args.Response.WriteTo(Stream);
            Stream.Flush();
        }

        public void ServeClient()
        {
            Console.WriteLine("Serving client...");
            try
            {
                while (true)
                {
                    var packet = Packet.ReadFrom(Stream);

                    Emulator.HandleRequest(packet);
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
