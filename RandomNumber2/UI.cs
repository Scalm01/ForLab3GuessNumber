// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Part of RandomNumber2 project.
//   Author: [Eric]
//   Date: [2025]
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace RandomNumber2
{
    /// <summary>
    /// Hanterar användargränssnittet för spelet, inklusive välkomstmeddelande och highscore-lista.
    /// </summary>
    public class UI
    {
        private HandleHighScore scoreList; // Instans för att hantera highscore
        private List<Score> scores;        // Lista för poäng

        /// <summary>
        /// Initierar en ny instans av <see cref="UI"/>-klassen.
        /// Hämtar highscore-listan vid start.
        /// </summary>
        public UI()
        {
            scoreList = new HandleHighScore();
            scores = scoreList.GetHighScores();
        }

        /// <summary>
        /// Visar välkomstmeddelande och highscore-listan.
        /// Frågar användaren om de vill spela.
        /// </summary>
        /// <returns><c>true</c> om spelaren vill spela, annars <c>false</c>.</returns>
        public bool DrawUI()
        {
            Console.WriteLine("Välkommen till Gissa Nummret 2!");

            // Visa highscore-listan
            if (scores == null || scores.Count == 0)
            {
                Console.WriteLine("Ingen har spelat tidigare. Du kan bli den första på listan!");
            }
            else
            {
                Console.WriteLine("Highscore-lista:");
                foreach (var score in scores)
                {
                    Console.WriteLine($"{score.PlayerName}: {score.Points} gissningar");
                }
            }

            Console.Write("Vill du spela? (ja/nej): "); // spela ja eller nej
            string input = Console.ReadLine();

            if (input.ToLower() == "ja")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
