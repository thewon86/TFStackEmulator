using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace TFStackEmulator
{
    public class NetworkStackConnector
    {
        private NetworkStream Stream;
        private Socket ClientSocket;

        private StackEmulator Emulator;

        public NetworkStackConnector(Socket clientSocket, StackEmulator emulator)
        {
            ClientSocket = clientSocket;
            Emulator = emulator;
            Stream = new NetworkStream(clientSocket);

            ClientSocket.NoDelay = true;
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
