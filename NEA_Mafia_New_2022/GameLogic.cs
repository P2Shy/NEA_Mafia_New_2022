using System;
using System.Collections.Generic;
using System.Text;

namespace NEA_Mafia_New_2022
{
    public enum GameState { Day, Night, Vote, GameOver}
    class GameLogic
    {
        public static void StartGame()
        {
            //Call role distribution
            Game();
        }

        public static void Game()
        {
            Console.WriteLine("Game Started");

            GameState curState = GameState.Day;

            switch (curState)
            {
                case GameState.Day:
                    {
                        //Send to clients
                        break;
                    }

            }
        }

        public static void RoleDistribution(string[] Deck, string[] PlayerList)
        {
            Random rnd = new Random();
            
            IDictionary<string, string> roleClientDict = new Dictionary<string, string>();

            foreach (string ip in PlayerList)
            {
                int randomNumber = rnd.Next(0, Deck.Length);
                roleClientDict.Add(ip, Deck[randomNumber]);
            }
        }
    }
}
