using EA.EA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class LearningManager : ILearningManager<Specimen>
    {
        public Data Config { get; set; }
        public IMutator<Specimen> Mutator { get; set; }
        public ICrossover<Specimen> Crossover { get; set; }
        public ISelector<Specimen> Selector { get; set; }
        public IList<Specimen> CurrentEpochSpecimens { get; set; }

        public LearningManager(Data config, IMutator<Specimen> mutator, ICrossover<Specimen> crossover, ISelector<Specimen> selector)
        {
            this.Config = config;
            this.Mutator = mutator;
            this.Crossover = crossover;
            this.Selector = selector;
            this.CurrentEpochSpecimens = new List<Specimen>();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void NextEpoch()
        {
            throw new NotImplementedException();
        }
    }
}
