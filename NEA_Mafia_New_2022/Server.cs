
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace NEA_Mafia_New_2022
{
    class Server
    {
        private Socket __socket;
        private Guid serverID = Guid.NewGuid();
        private int __maxPlayers;
        ArrayList arrSocket = new ArrayList();

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

        public void Broadcast(string message)
        {
            Message broadcast = new Message(message, serverID.ToString(), "Server");
            foreach (Socket socket in arrSocket){
                socket.Send(broadcast.Data);
            }
        }

        public void Handle(byte[] hpacket, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(hpacket, 0);
            ushort packetType = BitConverter.ToUInt16(hpacket, 2);

            switch (packetType)
            {
                case 2000:
                    Message msg = new Message(hpacket);
                    string name = msg.Name;
                    string message = msg.Text;
                    Broadcast(name + ":" + message);
                    break;
                case 2020:
                    break;

            }
        }

        public void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSocket = __socket.EndAccept(result);
            arrSocket.Add(clientSocket);
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

                        Handle(packet, clientSocket);

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
}