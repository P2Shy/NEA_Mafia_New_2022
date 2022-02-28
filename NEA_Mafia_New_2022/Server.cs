
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NEA_Mafia_New_2022
{
    class Server
    {
        private Socket __socket;
        private Guid clientID = Guid.NewGuid();
        private int __maxPlayers;

        public Server(int players)
        {
            __socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            __maxPlayers = players;
        }

        public void Bind(int port)
        {
            __socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen()
        {
            __socket.Listen(__maxPlayers);
        }

        public void Accept()
        {
            __socket.BeginAccept(AcceptedCallback, null);
        }

        public void Close(Socket sock)
        {
            Console.WriteLine("Closing socket for IP:" + sock.RemoteEndPoint.ToString() + " and releasing resources.");
            sock.Dispose();
            sock.Close();
        }

        public void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSocket = __socket.EndAccept(result);
            Accept();
            var buffer = new byte[1024];
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);



            void RecivedCallback(IAsyncResult result)
            {
                Socket clientSocket = result.AsyncState as Socket;
                SocketError ER;


                try
                {
                    int bufferSize = clientSocket.EndReceive(result, out ER);
                    if (ER == SocketError.Success)
                    {
                        byte[] packet = new byte[bufferSize];
                        Array.Copy(buffer, packet, packet.Length);

                        ServerPacketHandler.Handle(packet, clientSocket);

                        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);
                    }

                    else
                    {
                        Close(clientSocket);
                    }

                }

                catch
                {
                    Close(clientSocket);
                }
            }

        }
    }

    public static class ServerPacketHandler
    {
        public static void Handle(byte[] hpacket, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(hpacket, 0);
            ushort packetType = BitConverter.ToUInt16(hpacket, 2);

            switch (packetType)
            {
                case 2000:
                    Message msg = new Message(hpacket);
                    Console.WriteLine(msg.ID + ":" + msg.Text);
                    Console.WriteLine("message recived");
                    break;
                case 2020:
                    break;

            }
        }
    }

}