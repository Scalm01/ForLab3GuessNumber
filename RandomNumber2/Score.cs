using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomNumber2
{

    public class Score : IComparable<Score>
    {
        // Properties för spelarens namn och poäng
        public string PlayerName { get; set; }
        public int Points { get; set; }

        // Implementerar CompareTo method att jämföra scores by Points
        public int CompareTo(Score other)
        {
            // Sort by Points (ascending)
            return this.Points.CompareTo(other.Points);
        }

    }
    
}
