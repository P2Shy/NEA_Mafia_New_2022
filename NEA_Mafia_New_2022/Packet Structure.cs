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
        public int _nameLength;
        public int _idLength;
        public int displace;

        public PacketStructure(ushort length, ushort type, string id, string name)
        {
            _buffer = new byte[length];
            _nameLength = 8*(System.Text.Encoding.UTF8.GetByteCount(name));
            _idLength = 8 * (System.Text.Encoding.UTF8.GetByteCount(name));
            displace = 4 + _nameLength + _idLength;
            Console.WriteLine(_nameLength.ToString());
            WriteUShort(length, 0);
            WriteUShort(type, 2);
            WriteString(id, 4);
            WriteString(name, 132);
        }

        // Implement Marshal copy?

        public PacketStructure(byte[] packet)
        {
            _buffer = packet;
        }

        public void WriteUShort(ushort value, int offset)
        {
            byte[] tempBuf = new byte[2];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 2);
        }

        public ushort ReadUShort(int offset)
        {
            return BitConverter.ToUInt16(_buffer, offset);
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

        public string ReadString(int offset, int count)
        {
            return Encoding.UTF8.GetString(_buffer, offset, count);
        }

        public void Header(ushort length, ushort type, string id, string name)
        {
            WriteUShort(length, 0);
            WriteUShort(type, 2);
            WriteString(id, 4);
            WriteString(name, 132);
        }


        public byte[] Data
        {
            get { return _buffer; }
        }

        public string ID
        {
            get { return ReadString(4, _idLength); }
        }

        public string Name
        {
            get { return ReadString(_idLength, _nameLength); }
        }
    }

    public class Message : PacketStructure
    {

        private string _message;

        public Message(string message, string id, string name) : base((ushort)( + message.Length), 2000, id, name)
        {
            Text = message;
        }

        public Message(byte[] packet) : base(packet)
        {

        }

        public string Text
        {
            get { return ReadString(displace, Data.Length - displace); }
            set
            {
                _message = value;
                WriteString(value, displace);
            }
        }
    }
}