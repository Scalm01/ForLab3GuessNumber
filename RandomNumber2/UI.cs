using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomNumber2
{
    public class UI
    {
        private HandleHighScore scoreList; // Instans för att hantera highscore
        private List<Score> scores;        // Lista för poäng

        // Konstruktor
        public UI()
        {
            scoreList = new HandleHighScore();
            scores = scoreList.GetHighScores();
        }

        // Metod för att visa välkomstmeddelande och highscore
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
            Console.Write("Vill du spela? (ja/nej): "); //spela ja eller nej
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
