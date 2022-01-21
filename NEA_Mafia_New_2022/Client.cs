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

        public void Connect (string ipAdress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAdress), port) ConnectCallback, null)
        }

        public void ConnectCallback (IAsyncResult result)
        {
            _socket.BeginReceive(_buffer)
        }
    }    
}