using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.EA
{
    public class LearningManagerBase<T> : ILearningManager<T> where T : ISpecimen<T>
    {
        public IList<T> CurrentEpochSpecimens { get; set; }
        public IMutator<T> Mutator { get; set; }
        public ICrossover<T> Crossover { get; set; }
        public ISelector<T> Selector { get; set; }
        public uint PopulationSize { get; set; }
        public ISpecimenFactory<T> SpecimenFactory { get; set; }
        public ILogger<T>? Logger { get; set; }

        public LearningManagerBase(IMutator<T> mutator
            , ICrossover<T> crossover
            , ISelector<T> selector
            , ISpecimenFactory<T> specimenFactory
            , uint populationSize
            , ILogger<T>? logger = null
            )
        {
            this.CurrentEpochSpecimens = new List<T>();
            this.Mutator = mutator;
            this.Crossover = crossover;
            this.Selector = selector;            this.PopulationSize = populationSize;
            this.SpecimenFactory = specimenFactory;
            this.Logger = logger;
        }

        public virtual void Init()
        {
            this.CurrentEpochSpecimens.Clear();
            for(int i = 0; i < PopulationSize; i++)
            {
                this.CurrentEpochSpecimens.Add(this.SpecimenFactory.CreateSpecimen());
            }
        }

        public virtual void NextEpoch()
        {
            var selectedSpecimens = this.Selector.Select(this.CurrentEpochSpecimens);
            var newSpecimens = this.Crossover.Crossover(selectedSpecimens);
            var mutatedSpecimens = this.Mutator.Mutate(newSpecimens);
            this.CurrentEpochSpecimens = mutatedSpecimens;
            this.Logger?.Log(this.CurrentEpochSpecimens);
        }
    }
}
