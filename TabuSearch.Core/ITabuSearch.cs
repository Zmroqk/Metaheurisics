using Loggers;
using Loggers.CSV;
using Meta.Core;

namespace TabuSearch.Core
{
    public interface ITabuSearch<TSpecimen, TRecord> where TSpecimen : ISpecimen<TSpecimen> where TRecord : IRecord
    {
        INeighborhood<TSpecimen> Neighborhood { get; }
        ISpecimenFactory<TSpecimen> SpecimenFactory { get; }
        ILogger<TRecord>? Logger { get; }
        TSpecimen RunTabuSearch();
        IEnumerable<TSpecimen> TabuList();
    }
}
