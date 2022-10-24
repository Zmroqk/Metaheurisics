using EA.Core;
using Loggers.CSV;
using Meta.Core;
using TabuSearch.Core;
using TTP.Config;
using TTP.DataTTP;
using TTP.DataTTP.Inititializators;
using TTP.DataTTP.Loggers;
using TTP.DataTTP.Mutators;
using TTP.DataTTP.Neighborhoods;

var loader = new LearningConfigLoader<TabuConfig>();
Console.WriteLine("Path to config: ");
var path = Console.ReadLine();
var configs = loader.Load(string.IsNullOrWhiteSpace(path) ? "TabuConfig.json" : path);

foreach(var config in configs)
{
    var dataLoader = new DataLoader();
    var data = dataLoader.Load(config.InputFileName);
    ISpecimenInitializator<Specimen> initializator;
    if(config.SpecimenInitializator.Type == SpecimenInitializatorType.Greedy)
    {
        initializator = new GreedySpecimenInitializator(data);
    }
    else
    {
        initializator = new RandomSpecimenInitializator(data, config.SpecimenInitializator.ItemAddPropability);
    }
    var factory = new SpecimenFactory(data, initializator);
    IMutator<Specimen> mutator;
    if (config.Mutator == MutatorType.Swap)
    {
        mutator = new TabuSwapMutator(data);
    }
    else
    {
        mutator = new InverseMutator(data, 1);
    }  
    var knapsackMutator = new KnapsackMutator(data, config.GreedyKnapsackMutator);
    var neighbourhood = new Neighbourhood(mutator, knapsackMutator);
    var logger = new CSVLogger<Specimen, TabuRecord>(config.OutputFileName);
    logger.RunLogger();

    var tabuSearch = new TabuSearchManager(data, factory, neighbourhood, logger, config.Iterations, config.NeighborhoodSize, config.TabuSize);
    Console.WriteLine(config.TestName);
    tabuSearch.RunTabuSearch();

    logger.Wait();
    logger.Dispose();
}

