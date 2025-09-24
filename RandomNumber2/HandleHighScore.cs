using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomNumber2
{
    public class HandleHighScore
    {
        private string filePath = "highscores.txt"; // Filväg för highscore-listan

        // Metod för att hämta highscore-listan från filen
        public List<Score> GetHighScores()
        {
            List<Score> scores = new List<Score>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int points))
                    {
                        scores.Add(new Score { PlayerName = parts[0], Points = points });
                    }
                }
            }
            scores.Sort();
            return scores;
        }

        // Metod för att kontrollera och spara highscore
        public void CheckAndSaveHighScore(List<Score> scores)
        {
            try
            {
             

                
                // Spara highscore-listan till filen
                using(FileStream file = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(file))
                {
                    foreach (var score in scores)
                    {
                        writer.WriteLine($"{score.PlayerName},{score.Points}");
                    }
                }

                Console.WriteLine("Highscore sparad!");
            }
            catch (IOException )
            {
                Console.WriteLine($"Ett fel inträffade när highscore skulle sparas");
            }
            catch (Exception )
            {
                Console.WriteLine($"Ett oväntat fel inträffade");
            }
        }
    }
}
        

