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
    /// Representerar en spelares resultat i gissningsspelet.
    /// </summary>
    public class Score : IComparable<Score>
    {
        /// <summary>
        /// Gets or Sets sätter spelarens namn.
        /// </summary>
        // Properties för spelarens namn och poäng
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or Sets spelarens poäng (antal gissningar).
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Jämför detta resultat med ett annat resultat baserat på poäng.
        /// </summary>
        /// <param name="other">Det andra resultatet att jämföra med.</param>
        /// <returns>
        /// Ett värde som anger om detta objekt är mindre än, lika med,
        /// eller större än det andra objektet.
        /// </returns>
        public int CompareTo(Score other)
        {
            // Sort by Points (ascending)
            return this.Points.CompareTo(other.Points);
        }
    }
}
