using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NEA_Mafia_New_2022
{
    public abstract class PacketStructure
    {
        private byte[] _buffer;

        public PacketStructure(ushort length, ushort type)
        {
            _buffer = new byte[length];
            WriteUShort(length, 0);
            WriteUShort(type, 2);
        }

        // Implement Marshal copy?

        public void WriteUShort(ushort value, int offset)
        {
            byte[] tempBuf = new byte[2];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 2);
        }

        public void WriteUInt(ushort value, int offset)
        {
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 4);
        }

        public void WriteString(string value, int offset)
        {
            byte[] tempBuf = new byte[value.Length];
            tempBuf = Encoding.UTF8.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, value.Length);
        }

        public void Header(ushort length, ushort type)
        {
            WriteUShort(length, 0);
            WriteUShort(type, 2);
        }


        public byte[] Data()
        {
            return _buffer;
        }
    }

    public class Message : PacketStructure
    {

        private string _message;

        public Message(string message) : base((ushort)(4 + message.Length), 2000)
        {
            _message = message;
        }

        public string Text
        {
            get { return _message; }
            set
            {
                _message = value;
                WriteString(value, 4);
            }
        }
    }
}
