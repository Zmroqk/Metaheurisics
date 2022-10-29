using Loggers.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.DataTTP.Loggers
{
    public class SimulatedAnnealingRecord : IRecord
    {
        public int Iteration { get; set; }
        public double BestScore { get; set; }
        public double CurrentScore { get; set; }
        public double AverageScore { get; set; }
        public double WorstScore { get; set; }
    }
}
