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

        public Client()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAdress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAdress), port), ConnectCallback, null);
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


            _buffer = new byte[1024];
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecivedCallback, null);
        }

        public void Send(byte[] data)
        {
            _socket.Send(data);
        }
    } 
    
    
}