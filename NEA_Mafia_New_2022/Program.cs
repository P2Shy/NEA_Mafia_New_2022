using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace NEA_Mafia_New_2022
{

    public enum GameState { Day, Night }

    class Program
    {

        public static void Main(string[] args)
        {

            Guid clientGuid = Guid.NewGuid();
            int _idLength = (System.Text.Encoding.UTF8.GetByteCount(clientGuid.ToString()));
            Console.WriteLine("(S)erver or (C)lient" + _idLength);
            string initMenuInput = Console.ReadLine();

            if (initMenuInput == "S")
            {
                Server hostServer = new Server(3);
                hostServer.Bind(6556);
                hostServer.Listen();
                hostServer.Accept();

                while (true)
                {
                    Console.ReadLine();
                }
            }
            else if (initMenuInput == "C")
            {
                Console.Write("Input name: ");
                string name = Console.ReadLine();
                Client newClient = new Client(name);
                newClient.Connect("127.0.0.1", 6556);

                while (true)
                {
                    string msgString = Console.ReadLine();
                    if (msgString == "d")
                    {
                        newClient.Disconnect();
                        break;
                    }
                    Message msg = new Message(msgString, newClient.ID, newClient.Name);
                    newClient.Send(msg.Data);
                }
            }

        }

        void StartGame()
        {
            //distribute roles
            //setup everything

            GameState curState = GameState.Day;



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


        }
    }
}