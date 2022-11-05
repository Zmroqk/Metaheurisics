using Loggers.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.DataTTP.Loggers
{
    public class FullSearchRecord : IRecord
    {
        public string Metaheuristic { get; set; }
        public string FileName { get; set; }
        public int TabuSize { get; set; }
        public int NeighborSize { get; set; }
        public double StartingTemperature { get; set; }
        public double TargetTemperature { get; set; }
        public double AnnealingRate { get; set; }
        public double BestScore { get; set; }
        public double AverageScore { get; set; }
        public double WorstScore { get; set; }
        public double StandardError { get; set; }
        public string MutatorType { get; set; }
        public string SelectorType { get; set; }
        public string CrossoverType { get; set; }
        public bool GreeedyKnapsack { get; set; }
        public string InitializatorType { get; set; }
        public double MutationProbability { get; set; }
        public double CrossoverProbability { get; set; }
        public int SpecimenCount { get; set; }
        public int PopulationSize { get; set; }
        public int Epochs { get; set; }
        public int Iterations { get; set; }
    }
}
