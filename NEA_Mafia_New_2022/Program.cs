﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NEA_Mafia_New_2022
{

    class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("(S)erver or (C)lient");
            string initMenuInput = Console.ReadLine();

            if (initMenuInput == "S")
            {
                Server.StartServer();
            }
            else if (initMenuInput == "C")
            {
                Client.StartClient();
            }
        }

        public class Player
        {
            public bool state;
            public int protectionDate;
            public string alignment, role, name;

            public Player(string username)
            {
                name = username;
                state = true;
            }

            public void Protect(int date)
            {
                protectionDate = date;
            }

            class Innocent : Player
            {

                public Innocent(string username) : base(username)
                {
                    role = "Innocent";
                    alignment = "Town";
                }
            }

            class Mafioso : Player
            {

                public Mafioso(string username) : base(username)
                {
                    role = "Mafioso";
                    alignment = "Mafia";
                }
            }

            class Detective : Player
            {

                public Detective(string username) : base(username)
                {
                    role = "Detective";
                    alignment = "Town";
                }
            }

            public static void Kill(Player player, int currentDate)
            {
                if ((player.state == false) | (player.protectionDate != currentDate))
                {
                    return;
                }
                else
                {
                    player.state = false;
                }
            }
        }
    }

}
