using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.Config
{
    public class FullSearchCrossover
    {
        public CrossoverType[] Types { get; set; }
        public double MinProbability { get; set; }
        public double MaxProbability { get; set; }
        public double ProbabilityChange { get; set; } = 0.001d;
    }
}
