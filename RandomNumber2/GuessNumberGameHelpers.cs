namespace RandomNumber2
{
    /// <summary>
    /// Hjälp klass för statisk anrop i spelet.
    /// </summary>
    internal static class GuessNumberGameHelpers
    {
        /// <summary>
        /// Startar spelet från ett statiskt anrop.
        /// </summary>
        public static void StartGame() // Metod för att starta spelet från ett externt anrop
        {
            GuessNumberGame game = new GuessNumberGame();
            game.StartaSpel();
        }
    }
}