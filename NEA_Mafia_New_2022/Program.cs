using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

class Program
{
    public enum GameState { Day, Night, Accuse, GameOver };
    public GameState curState;

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

    void Start()
    {
        curState = GameState.Day;
        Game();
    }


    void Game()
    {
        Console.WriteLine("input:");
        string input = Console.ReadLine();

        switch (curState)
        {
            case GameState.Day:
                return;
        }
    }

    public static void ParsePlayerInput(string playerInput)
    {
        string ChatString, PlayerAccused;
        if ((playerInput[0] == 'S') | (playerInput[0] == 's'))
        {
            playerInput.TrimStart('S', 's');
            ChatString = playerInput;
        }

        else if ((playerInput[0] == 'V') | (playerInput[0] == 'v'))
        {
            //playerInput.TrimStart('V','v');
            //PlayerAccused = playerInput;
            //Vote(PlayerAccused, 0.5);
            //TODO Vote method
        }
        return;
    }

}

class Player
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

class Server
{
    public static void StartServer()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        try
        {

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Waiting for a connection...");
            Socket handler = listener.Accept();
            Console.WriteLine("Connected");

            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);

                if (data.IndexOf("<EOF>") > -1)
                {

                    Console.WriteLine("Connection with",
                    listener.RemoteEndPoint.ToString(), "terminated");
                    break;
                }

                else
                {
                    Console.WriteLine("Text recived : {0}", data);
                }
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\n press any key to continue...");
        Console.ReadKey();
    }
}

class Client
{
    public static void StartClient()
    {
        byte[] bytes = new byte[1024];

        try
        {

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                while (true)
                {

                    Console.Write("Message:");
                    byte[] msg = Encoding.ASCII.GetBytes(Console.ReadLine());

                    int bytesSent = sender.Send(msg);

                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echo = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    if (Encoding.ASCII.GetString(bytes).IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
