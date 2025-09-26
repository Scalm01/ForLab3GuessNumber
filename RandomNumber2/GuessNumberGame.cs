namespace RandomNumber2
{
    using System;
    using System.Collections.Generic;

// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Part of RandomNumber2 project.
//   Author: [Eric]
//   Date: [2025]
// </summary
// --------------------------------------------------------------------------------------------------------------------

 /// <summary>
/// Klass som hanterar spelet "Gissa numret".
/// </summary>
    public class GuessNumberGame
    {
        private int nummerAttGissa; // Det nummer spelaren ska gissa
        private int svar; // Spelarens nuvarande gissning
        private int antGissningar; // Räknar antal gissningar
        private Score gameScore; // Instans av Score som håller spelarens resultat
        private HandleHighScore saveScoreList = new HandleHighScore(); // Instans av HandleHighScore för att spara high scores

/// <summary>
/// Startar spelet och hanterar spelloopen.
/// </summary>
public void StartaSpel()
        {
            bool spela = true;
            while (spela) // Loop för att starta om spelet med en ny spelare varje gång
            {
                Console.Write("Ange ditt namn: ");
                string playerName = Console.ReadLine(); // Fråga efter spelarens namn varje gång

                Random random = new Random();
                nummerAttGissa = random.Next(1, 101); // Genererar ett slumpmässigt nummer mellan 1 och 100
                antGissningar = 0; // Nollställer antalet gissningar

                Console.WriteLine("Låt oss starta, lycka till! Du har 20 gissningar.");
                Console.WriteLine("Gissa ett nummer mellan 1 och 100");

                List<Score> scores = new List<Score>();

                // Loopar tills spelaren gissar rätt eller når 20 försök
                while (antGissningar < 20)
                {
                    Console.Write("Ange din gissning: ");
                    bool isValidNumber = int.TryParse(Console.ReadLine(), out svar);

                    if (!isValidNumber)
                    {
                        Console.WriteLine("Ogiltig inmatning, försök igen.");
                        continue;
                    }

                    if (svar < 1 || svar > 100)
                    {
                        Console.WriteLine("Gissningar måste vara mellan 1 och 100.");
                        continue;
                    }

                    antGissningar++; // Ökar gissningsräknaren

                    if (svar == nummerAttGissa) // Om spelaren gissar rätt
                    {
                        Console.WriteLine($"Grattis {playerName}, du gissade rätt på {antGissningar} försök!");

                        // Skapa en ny Score för spelaren, med spelarens namn och antal gissningar
                        gameScore = new Score
                        {
                            PlayerName = playerName,
                            Points = antGissningar,
                        };

                        // Lägg till den nya poängen med spelarens namn
                        scores.Add(gameScore);

                        // Frågar om spelaren vill spela igen
                        Console.WriteLine("Vill du spela igen? (y/n)");
                        string svarToPlay = Console.ReadLine().ToLower();

                        if (svarToPlay == "n")
                        {
                            // Visa high score-listan och avsluta spelet
                            // Spara spelarens resultat till high score-listan
                            saveScoreList.CheckAndSaveHighScore(scores);

                            List<Score> highScores = saveScoreList.GetHighScores();
                            Console.WriteLine("\nHighscores:");
                            foreach (var score in highScores)
                            {
                                Console.WriteLine($"{score.PlayerName}: {score.Points}");
                            }

                            Console.WriteLine("Tack för att du spelade!");
                            spela = false;
                        }

                        break; // Avsluta omgången om spelaren vill fortsätta
                    }
                    else if (svar < nummerAttGissa)
                    {
                        Console.WriteLine("För lågt, gissa igen.");
                    }
                    else if (svar > nummerAttGissa)
                    {
                        Console.WriteLine("För högt, gissa igen.");
                    }
                }

                if (antGissningar == 20) // Om spelaren når 20 gissningar utan att gissa rätt
                {
                    Console.WriteLine("Du har slut på gissningar. Lycka till nästa gång!");

                    // Frågar om spelaren vill spela igen
                    Console.WriteLine("Vill du spela igen? (y/n)");
                    string svarToPlay = Console.ReadLine().ToLower();

                    if (svarToPlay == "n")
                    {
                        // Visa high score-listan och avsluta spelet
                        List<Score> highScores = saveScoreList.GetHighScores();
                        Console.WriteLine("\nHighscores:");
                        foreach (var score in highScores)
                        {
                            Console.WriteLine($"{score.PlayerName}: {score.Points}");
                        }

                        Console.WriteLine("Tack för att du spelade!");
                        return; // Avslutar spelet, men inte hela programmet
                    }
                }
            }

            Environment.Exit(0);
        }

// test metod som genererar slumpnummer för test av slumping

/// <summary>
/// Genererar ett slumpmässigt nummer mellan 1 och 100.
/// </summary>
/// <returns>
/// Slumpmässigt tal mellan 1 och 100.
/// </returns>
public int GenerateRandomNumber()
        {
            Random rnd = new Random();
            return rnd.Next(1, 101);
        }

/// <summary>
/// Kontrollerar om gissningen är korrekt.
/// </summary>
/// <param name="guess">Spelarens gissning.</param>
/// <param name="target">Numret som ska gissas.</param>
/// <returns>true om korrekt, annars false.</returns>
public bool CheckGuess(int guess, int target)
        {
            return guess == target;
        }
    }
}
