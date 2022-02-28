
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
<<<<<<< HEAD
using System.Text;
using System.Collections;
=======
>>>>>>> parent of 4291c0f (FIXED SEND TWICE PROBLEM!!!)
using System.Threading;

namespace NEA_Mafia_New_2022
{
    class Server
    {
        private Socket __socket;
        private byte[] __buffer = new byte[1024];
        private Guid clientID = Guid.NewGuid();
<<<<<<< HEAD
        private int __maxPlayers;
        ArrayList arrSocket = new ArrayList();
        int readyCount = 0;
=======
>>>>>>> parent of 4291c0f (FIXED SEND TWICE PROBLEM!!!)

        public Server()
        {
            __socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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

        public void Listen(int backlog)
        {
<<<<<<< HEAD
            __socket.Listen(__maxPlayers+1);
=======
            __socket.Listen(backlog);
>>>>>>> parent of 4291c0f (FIXED SEND TWICE PROBLEM!!!)
        }

        public void Accept()
        {
            __socket.BeginAccept(AcceptedCallback, null);
        }

<<<<<<< HEAD
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

=======
>>>>>>> parent of 4291c0f (FIXED SEND TWICE PROBLEM!!!)
        public void AcceptedCallback(IAsyncResult result)
        {
            var clientSocket = __socket.EndAccept(result);
            arrSocket.Add(clientSocket);
            byte[] buffer = new byte[1024];
            Accept();
<<<<<<< HEAD
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
=======
            clientSocket.BeginReceive(__buffer, 0, __buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);
            
        }
>>>>>>> parent of 4291c0f (FIXED SEND TWICE PROBLEM!!!)

        public void RecivedCallback(IAsyncResult result)
        {
            Socket clientSocket = result.AsyncState as Socket;
            int bufferSize = clientSocket.EndReceive(result);
            byte[] packet = new byte[bufferSize];
            Array.Copy(__buffer, packet, packet.Length);

            ServerPacketHandler.Handle(packet, clientSocket);

<<<<<<< HEAD
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Close(clientSocket);
                }
            }
            
=======
            __buffer = new byte[1024];
            clientSocket.BeginReceive(__buffer, 0, __buffer.Length, SocketFlags.None, RecivedCallback, clientSocket);
>>>>>>> parent of 4291c0f (FIXED SEND TWICE PROBLEM!!!)
        }

        public void GameLogic()
        {
<<<<<<< HEAD
            
=======
            ushort packetLength = BitConverter.ToUInt16(hpacket,0);
            ushort packetType = BitConverter.ToUInt16(hpacket, 2);

            switch (packetType)
            {
                case 2000:
                    Message msg = new Message(hpacket);
                    Console.WriteLine(msg.ID +":"+ msg.Text);
                    break;
            }
>>>>>>> parent of 4291c0f (FIXED SEND TWICE PROBLEM!!!)
        }
    }       
}