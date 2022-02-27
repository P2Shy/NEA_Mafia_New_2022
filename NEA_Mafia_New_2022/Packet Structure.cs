﻿using System;
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

        public PacketStructure(ushort length, ushort type, string id)
        {
            _buffer = new byte[length];
            WriteUShort(length, 0);
            WriteUShort(type, 2);
            WriteString(id, 4);
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

        public void Header(ushort length, ushort type, string id)
        {
            WriteUShort(length, 0);
            WriteUShort(type, 2);
            WriteString(id, 4);
        }


        public byte[] Data
        {
            get { return _buffer; }
        }

        public string ID
        {
            get { return ReadString(4, 128); }
        }
    }

    public class Message : PacketStructure
    {

        private string _message;

        public Message(string message, string id) : base((ushort)(132 + message.Length), 2000, id)
        {
            Text = message;
        }

        public Message(byte[] packet) : base(packet)
        {

        }

        public string Text
        {
            get { return ReadString(132, Data.Length - 132); }
            set
            {
                _message = value;
                WriteString(value, 132);
            }
        }
    }

    public class GiveName : PacketStructure
    {

        private string _userName;

        public GiveName(string userName, string id) : base((ushort)(132 + userName.Length), 2020, id)
        {
            Name = userName;
        }

        public GiveName(byte[] packet) : base(packet)
        {

        }

        public string Name
        {
            get { return ReadString(132, Data.Length - 132); }
            set
            {
                _userName = value;
                WriteString(value, 132);
            }
        }


    }
}