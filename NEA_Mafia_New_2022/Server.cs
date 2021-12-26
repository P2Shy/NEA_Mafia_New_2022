using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NEA_Mafia_New_2022
{
    class Server
    {
        public static void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            try
            {
                string data = null;
                byte[] bytes = null;

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(10);



                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    Console.WriteLine("Connected");

                    handleClient client = new handleClient();
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n press any key to continue...");
            Console.ReadKey();
        }

    }

    public class handleClient
    {
        TcpClient clientSocket;
        string clientNumber;

        public void startClient(TcpClient inClientSocket, string clientNo)
        {
            this.clientSocket = inClientSocket;
            this.clientNumber = clientNo;
            Thread ctThread = new Thread(doChat);
        }

        private void doChat()
        {

        }
    }
}
