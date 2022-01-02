﻿
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NEA_Mafia_New_2022
{
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
                listener.Listen(30);

                int counter = 0;
                int MaxPlayers = 2;

                while (true)
                {
                    counter += 1;
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    Console.WriteLine("Connected");
                    HandleClient client = new HandleClient();
                    client.StartClient(handler, listener, counter.ToString());

                    if (counter == MaxPlayers)
                    {
                        GameLogic.StartGame();
                        handler.Send(Encoding.ASCII.GetBytes("<CMD> Start Game"));
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n press any key to continue...");
            Console.ReadKey();
        }
    }

    public class HandleClient
    {
        public enum GameState { Day, Night, Vote, GameOver }
        GameState curState;

        string[] ipList = new string[10];

        Socket handler, listener;
        string clNo;

        public void StartClient(Socket inClientHandler, Socket inClientListener ,string clientNo)
        {
            this.handler = inClientHandler;
            this.listener = inClientListener;
            this.clNo = clientNo;

            ipList[Convert.ToInt32(clientNo)] = handler.RemoteEndPoint.ToString();
            foreach (string ip in ipList){
                Console.WriteLine(ip);
            }

            Thread ctThread = new Thread(DoChat);
            ctThread.Start();
        }

        private void DoChat()
        {

            //Thread Game = new Thread(StartGame);

            try
            {
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);

                   if (data.Contains("<GameState>"))
                    {
                        string GameStateRec = data.Split("<GameState>")[1];

                        if (GameStateRec == "D")
                        {
                            curState = GameState.Day;
                        }
                        else if (GameStateRec == "N")
                        {
                            curState = GameState.Night;
                        }
                        else if (GameStateRec == "V")
                        {
                            curState = GameState.Vote;
                        }
                        else if (GameStateRec == "E")
                        {
                            curState = GameState.GameOver;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    else if (data.IndexOf("<EOF>") > -1)
                    {

                        Console.WriteLine("Connection with client",clNo,"terminated");
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

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}