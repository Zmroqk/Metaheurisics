using Loggers;
using Loggers.CSV;
using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core
{
    public class LearningManagerBase<T, TRecord> : ILearningManager<T, TRecord> where T : ISpecimen<T> where TRecord : IRecord, new()
    {
        public IList<T> CurrentEpochSpecimens { get; set; }
        public IMutator<T> Mutator { get; set; }
        public ICrossover<T> Crossover { get; set; }
        public ISelector<T> Selector { get; set; }
        public uint PopulationSize { get; set; }
        public ISpecimenFactory<T> SpecimenFactory { get; set; }
        public ILogger<TRecord>? Logger { get; set; }

        public int CurrentEpoch { get; set; }
        public IAdditionalOperations<T>? AdditionalOperationsHandler { get; set; }
        public T Best { get; set; }

        public LearningManagerBase(IMutator<T> mutator
            , ICrossover<T> crossover
            , ISelector<T> selector
            , ISpecimenFactory<T> specimenFactory
            , uint populationSize
            , ILogger<TRecord>? logger = null
            , IAdditionalOperations<T> additionalOperations = null
            )
        {
            this.CurrentEpochSpecimens = new List<T>();
            this.Mutator = mutator;
            this.Crossover = crossover;
            this.Selector = selector;            this.PopulationSize = populationSize;
            this.SpecimenFactory = specimenFactory;
            this.Logger = logger;
            this.CurrentEpoch = 0;
            this.AdditionalOperationsHandler = additionalOperations;
        }

        public virtual void Init()
        {
            this.CurrentEpochSpecimens.Clear();
            for (int i = 0; i < this.PopulationSize; i++)
            {
                this.CurrentEpochSpecimens.Add(this.SpecimenFactory.CreateSpecimen());
            }
        }

        public virtual void NextEpoch()
        {
            var beforeSelectSpecimens = this.AdditionalOperationsHandler?.BeforeSelect(this.CurrentEpochSpecimens) ?? this.CurrentEpochSpecimens;
            var selectedSpecimens = this.Selector.Select(beforeSelectSpecimens);
            var afterSelectSpecimens = this.AdditionalOperationsHandler?.AfterSelect(selectedSpecimens) ?? selectedSpecimens;
            var beforeCrossoverSpecimens = this.AdditionalOperationsHandler?.BeforeCrossover(afterSelectSpecimens) ?? afterSelectSpecimens;
            var newSpecimens = this.Crossover.Crossover(beforeCrossoverSpecimens);
            var afterCrossoverSpecimens = this.AdditionalOperationsHandler?.AfterCrossover(newSpecimens) ?? newSpecimens;
            var beforeMutationSpecimens = this.AdditionalOperationsHandler?.BeforeMutation(afterCrossoverSpecimens) ?? afterCrossoverSpecimens;
            var mutatedSpecimens = this.Mutator.MutateAll(beforeMutationSpecimens);
            var afterMutationSpecimens = this.AdditionalOperationsHandler?.AfterMutation(mutatedSpecimens) ?? mutatedSpecimens;
            this.CurrentEpochSpecimens = afterMutationSpecimens;
            var best = this.CurrentEpochSpecimens.MaxBy(x => x.Evaluate());
            if(this.Best.Evaluate() < best.Evaluate())
            {
                this.Best = best;
            }
            this.Logger?.Log(new TRecord());
        }
    }
}
