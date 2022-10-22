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
    public interface ILearningManager<T, TRecord> where T : ISpecimen<T> where TRecord : IRecord
    {
        IList<T> CurrentEpochSpecimens { get; set; }
        IMutator<T> Mutator { get; set; }
        ICrossover<T> Crossover { get; set; }
        ISelector<T> Selector { get; set; }
        ISpecimenFactory<T> SpecimenFactory { get; set; }
        ILogger<TRecord>? Logger { get; set; }
        IAdditionalOperations<T>? AdditionalOperationsHandler { get; set; }
        void Init();
        void NextEpoch();

        uint PopulationSize { get; set; }
    }
}
