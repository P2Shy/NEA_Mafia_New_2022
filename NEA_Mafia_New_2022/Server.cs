﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

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

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();
                Console.WriteLine("Connected");

                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);

                    if (data.IndexOf("<EOF>") > -1)
                    {

                        Console.WriteLine("Connection with",
                        listener.RemoteEndPoint.ToString(), "terminated");
                        break;
                    }

                    else
                    {
                        Console.WriteLine("Text recived : {0}", data);
                    }
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
}
