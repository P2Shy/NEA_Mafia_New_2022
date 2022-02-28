using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace NEA_Mafia_New_2022
{

    public enum GameState { Day, Night}

    class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("(S)erver or (C)lient");
            string initMenuInput = Console.ReadLine();

            if (initMenuInput == "S")
            {
                Server hostServer = new Server(2);
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
                string name = Console.ReadLine();
                Client newClient = new Client(name);
                newClient.Connect("127.0.0.1", 6556);

                while (true)
                {
                    string msgString = Console.ReadLine();
                    if (msgString == "d"){
                        newClient.Disconnect();
                        break;
                    }

                    else if (msgString == "ready")
                    {
                        Ready rdy = new Ready(newClient.ID);
                        newClient.Send(rdy.Data);
                    }

                    else if (msgString == "unready")
                    {
                        Unready unrdy = new Unready(newClient.ID);
                        newClient.Send(unrdy.Data);
                    }
                    Message msg = new Message(msgString, newClient.ID);
                    newClient.Send(msg.Data);
                }
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


        }
    }
}
