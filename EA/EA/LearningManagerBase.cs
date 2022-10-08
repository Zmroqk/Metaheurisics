using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.EA
{
    public class LearningManagerBase<T> : ILearningManager<T> where T : ISpecimen
    {
        public IList<T> CurrentEpochSpecimens { get; set; }
        public IMutator<T> Mutator { get; set; }
        public ICrossover<T> Crossover { get; set; }
        public ISelector<T> Selector { get; set; }

        public LearningManagerBase(IMutator<T> mutator, ICrossover<T> crossover, ISelector<T> selector)
        {
            CurrentEpochSpecimens = new List<T>();
            Mutator = mutator;
            Crossover = crossover;
            Selector = selector;
        }

        public virtual void Init()
        {
        }

        public virtual void NextEpoch()
        {
            var selectedSpecimens = this.Selector.Select(this.CurrentEpochSpecimens);
            var newSpecimens = this.Crossover.Crossover(selectedSpecimens);
            var mutatedSpecimens = this.Mutator.Mutate(newSpecimens);
            this.CurrentEpochSpecimens = mutatedSpecimens;
        }
    }
}
