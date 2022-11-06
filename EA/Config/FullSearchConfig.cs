using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.Config
{
    public class FullSearchConfig : IConfig
    {
        public int RunCount { get; set; }
        public int Threads { get; set; }
        public string[] FilePaths { get; set; }
        public string OutputPath { get; set; }
        public string[] Include { get ; set; }
        public FullSearchTabu? TabuConfig { get; set; }
        public FullSearchSimulatedAnnealing? SimulatedAnnealing { get; set; }
        public FullSearchEvolutionaryAlgorithm? EvolutionaryAlgorithm { get; set; }
    }
}
