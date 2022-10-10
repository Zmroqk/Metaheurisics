using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class CrossoverConfig
    {
        /// <summary>
        /// Crossover method to use
        /// </summary>
        public CrossoverType Type { get; set; }
        /// <summary>
        /// Crossover probability
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// Crossover method length parameter
        /// </summary>
        public int CrossoverLength { get; set; }
    }
}
