using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace NEA_Mafia_New_2022
{

    class Program
    {

        public static int DayCycle = 10000;

        public static void Main(string[] args)
        {
            /*Console.WriteLine("(S)erver or (C)lient");
            string initMenuInput = Console.ReadLine();

            if (initMenuInput == "S")
            {
                Server hostServer = new Server();
                hostServer.Bind(6556);
                hostServer.Listen(50);
                hostServer.Accept();

                while (true){
                    Console.ReadLine();
                }
            }
            else if (initMenuInput == "C")
            {
                Client newClient = new Client();
                newClient.Connect("127.0.0.1", 6556);

                while (true)
                {
                    string msg = Console.ReadLine();
                    Message packet = new Message(msg);
                    newClient.Send(packet.Data());
                }
            }*/

            Day();
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


        public static void Day()
        {
            Timer timer = new System.Timers.Timer(DayCycle);
            timer.Elapsed += new ElapsedEventHandler(Night);
            timer.Enabled = true;
            
            while (true)
            {
                Console.ReadLine();
            }



        }

        public static void Night(object source, ElapsedEventArgs e)
        {
            Console.Write("E");
        }

    }

}
