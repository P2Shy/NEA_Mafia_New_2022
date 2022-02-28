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
        private Socket _socket;
        private byte[] _buffer;
        private Guid clientGuid = Guid.NewGuid();
        private string _username;

        public Client(string username)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _username = username;
        }

        public void Connect(string ipAdress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAdress), port), ConnectCallback, null);
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void ConnectCallback(IAsyncResult result)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Connection established!");
                _buffer = new byte[1024];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecivedCallback, null);

            }
            else
            {
                Console.WriteLine("Connection could not be established");
            }
        }

        public void RecivedCallback(IAsyncResult result)
        {
            int bufLength = _socket.EndReceive(result);
            byte[] packet = new byte[bufLength];
            Array.Copy(_buffer, packet, packet.Length);

            ClientPacketHandler.Handle(packet, _socket);

            _buffer = new byte[1024];
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecivedCallback, null);
        }

        public void Send(byte[] data)
        {
            _socket.Send(data);
        }

        public string ID
        {
            get { return clientGuid.ToString(); }
        }

        public string Name
        {
            get { return _username; }
        }
    }

    public static class ClientPacketHandler
    {
        public static void Handle(byte[] hpacket, Socket clientSocket)
        {
            ushort packetLength = BitConverter.ToUInt16(hpacket, 0);
            ushort packetType = BitConverter.ToUInt16(hpacket, 2);

            switch (packetType)
            {
                case 2000:
                    Message msg = new Message(hpacket);
                    Console.WriteLine(msg.Text);
                    break;
            }
        }
    }


}