
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

            Console.WriteLine(Encoding.UTF8.GetString(packet));

            __buffer = new byte[1024];
            clientSocket.BeginReceive(__buffer, 0, __buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);
        }
    }

    public static class PacketHandler
    {
        public static void Handle(byte[] packet, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet,0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);
        }
    }
        
}