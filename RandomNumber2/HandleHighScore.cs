using System;
using System.Collections.Generic;
using System.IO;

namespace RandomNumber2
{
    // --------------------------------------------------------------------------------------------------------------------
    // <summary>
    //   Part of RandomNumber2 project.
    //   Author: [Eric]
    //   Date: [2025]
    // </summary
    // --------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Hanterar inläsning och sparning av highscores till fil.
    /// </summary>
    public class HandleHighScore
    {
        private string filePath = "highscores.txt"; // Filväg för highscore-listan

        /// <summary>
        /// Hämtar highscore-listan från filen och returnerar en sorterad lista.
        /// </summary>
        /// <returns>En lista av <see cref="Score"/> sorterad efter poäng.</returns>
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

/// <summary>
/// Sparar en eller flera highscores till filen.
/// </summary>
/// <param name="scores">En lista av <see cref="Score"/> att spara.</param>
public void CheckAndSaveHighScore(List<Score> scores)
        {
            try
            {
                // Spara highscore-listan till filen
                using (FileStream file = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(file))
                {
                    foreach (var score in scores)
                    {
                        writer.WriteLine($"{score.PlayerName},{score.Points}");
                    }
                }

                Console.WriteLine("Highscore sparad!");
            }
            catch (IOException)
            {
                Console.WriteLine($"Ett fel inträffade när highscore skulle sparas");
            }
            catch (Exception)
            {
                Console.WriteLine($"Ett oväntat fel inträffade");
            }
        }
    }
}