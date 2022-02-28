
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;

namespace NEA_Mafia_New_2022
{
    class Server
    {
        private Socket __socket;
        private Guid clientID = Guid.NewGuid();
        private int __maxPlayers;
        ArrayList arrSocket = new ArrayList();
        int readyCount = 0;

        public Server(int players)
        {
            __socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            __maxPlayers = players;
        }

        public void WaitReady()
        {
            while (true)
            {
                if (arrSocket.Count == __maxPlayers)
                {
                    break;
                }
            }

            Console.WriteLine("Fart noise");
        }

        public void Bind(int port)
        {
            __socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen()
        {
            __socket.Listen(__maxPlayers+1);
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

        public void HandlePacket(byte[] hpacket, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(hpacket, 0);
            ushort packetType = BitConverter.ToUInt16(hpacket, 2);

            switch (packetType)
            {
                case 2000:
                    Message message = new Message(hpacket);
                    Console.WriteLine(message.ID + ":" + message.Text);
                    break;
                case 2019:
                    readyCount--;
                    break;
                case 2020:
                    readyCount++;
                    break;

            }
        }

        public void AcceptedCallback(IAsyncResult result)
        {
            var clientSocket = __socket.EndAccept(result);
            arrSocket.Add(clientSocket);
            byte[] buffer = new byte[1024];
            Accept();
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

                        HandlePacket(packet, clientSocket);

                        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);
                    }

                    else
                    {
                        Close(clientSocket);
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Close(clientSocket);
                }
            }
            
        }

        public void GameLogic()
        {
            
        }
    }       
}