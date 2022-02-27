﻿
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NEA_Mafia_New_2022
{
    class Server
    {
        private Socket __socket;
        private byte[] __buffer = new byte[1024];
        private Guid clientID = Guid.NewGuid();

        public Server()
        {
            __socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Bind(int port)
        {
            __socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen(int backlog)
        {
            __socket.Listen(backlog);
        }

        public void Accept()
        {
            __socket.BeginAccept(AcceptedCallback, null);
        }

        public void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSocket = __socket.EndAccept(result);
            Accept();
            clientSocket.BeginReceive(__buffer, 0, __buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);
            
        }

        public void RecivedCallback(IAsyncResult result)
        {
            Socket clientSocket = result.AsyncState as Socket;
            int bufferSize = clientSocket.EndReceive(result);
            byte[] packet = new byte[bufferSize];
            Array.Copy(__buffer, packet, packet.Length);

            ServerPacketHandler.Handle(packet, clientSocket);

            __buffer = new byte[1024];
            clientSocket.BeginReceive(__buffer, 0, __buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);
        }
    }

    public static class ServerPacketHandler
    {
        public static void Handle(byte[] hpacket, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(hpacket,0);
            ushort packetType = BitConverter.ToUInt16(hpacket, 2);

            switch (packetType)
            {
                case 2000:
                    Message msg = new Message(hpacket);
                    Console.WriteLine(msg.ID +":"+ msg.Text);
                    break;
            }
        }
    }
        
}