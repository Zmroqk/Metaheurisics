using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class LearningConfig
    {
        public MutatorConfig Mutator { get; set; }
        public SelectorConfig Selector { get; set; }
        public CrossoverConfig Crossover { get; set; }
        public SpecimenInitializatorConfig SpecimenInitializator { get; set; }
        public int PopulationSize { get; set; }
        public string OutputFileName { get; set; }
        public int Epochs { get; set; }
    }
}
