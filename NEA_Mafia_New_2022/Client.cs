using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NEA_Mafia_New_2022
{
    class Client
    {
        public static void StartClient()
        {

            try
            {

                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(remoteEP);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public class Handle
    {
        Socket sender;

        public void Start(Socket iSender)
        {
            this.sender = iSender;

            Thread ctThread = new Thread(DoChat);
            ctThread.Start();
        }

        private void DoChat()
        {

            try
            {

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                Handle newChat = new Handle();
                newChat.Start(sender);

                while (true)
                {

                    Console.Write("Message:");
                    string userMessage = "<MSG>" + Console.ReadLine();
                    byte[] msg = Encoding.ASCII.GetBytes(userMessage);

                    int bytesSent = sender.Send(msg);

                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echo = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    if (Encoding.ASCII.GetString(bytes).IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                }

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
    }
}