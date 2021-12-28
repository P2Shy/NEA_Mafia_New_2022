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
            GameState curState = GameState.Day;
            //Call role distribution
        }

        public static void Game()
        {

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
