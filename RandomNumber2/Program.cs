using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomNumber2
{
    public class Program
    {
        static void Main(string[]arg)
        {
            // Skapa en instans av UI-klassen
            UI ui = new UI();

            // Anropa DrawUI för att visa välkomsthälsning och hämta spelarens svar
            bool wantsToPlay = ui.DrawUI();
            while (wantsToPlay==true) {
                GuessNumberGame.StartGame();
            }
            //skapa instans av GuessNaumberGame, anropa spelmetoden och skicka med wantsTOPlay
            if (wantsToPlay == false)
            {
                Console.WriteLine("En annan Gång då");
                Environment.Exit(0);
            }
           

            
        }
    }
}
