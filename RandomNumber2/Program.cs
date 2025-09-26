// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Part of RandomNumber2 project.
//   Author: [Eric]
//   Date: [2025]
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace RandomNumber2
{
    /// <summary>
    /// Startpunkt för applikationen.
    /// </summary>
    /// <param name="args">Argument från kommandoraden (används ej).</param>
    public class Program
    {
        private static void Main(string[] args)
        {
            // Skapa en instans av UI-klassen
            UI ui = new UI();

            // Anropa DrawUI för att visa välkomsthälsning och hämta spelarens svar
            bool wantsToPlay = ui.DrawUI();
            while (wantsToPlay == true)
            {
                GuessNumberGameHelpers.StartGame();
            }

            // skapa instans av GuessNaumberGame, anropa spelmetoden och skicka med wantsTOPlay
            if (wantsToPlay == false)
            {
                Console.WriteLine("En annan Gång då");
                Environment.Exit(0);
            }
        }
    }
}
