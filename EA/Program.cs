using EA;
using EA.DataTTP;
using EA.DataTTP.Inititializators;
using EA.DataTTP.Mutators;
using EA.Core;
using EA.Core.Selectors;
using EA.Core.Loggers.CSV;
using EA.DataTTP.Crossovers;
using EA.Config;


Console.WriteLine("Path to Learning config: ");
string learningConfig = Console.ReadLine();
if (string.IsNullOrWhiteSpace(learningConfig))
{
    Console.WriteLine("Path to TTP files: ");
    DirectoryInfo directoryInfo = new DirectoryInfo(Console.ReadLine());
    if (!directoryInfo.Exists)
    {
        Environment.Exit(-2);
    }
    foreach (var file in directoryInfo.EnumerateFiles())
    {
        if (file.Extension == ".ttp")
        {
            var dataLoader = new DataLoader();
            var data = dataLoader.Load(file.FullName);
            if (data == null)
            {
                Environment.Exit(-1);
            }

            var mutator = new SwapMutator(data, 0.1d);
            var specimenInitailizator = new RandomSpecimenInitializator(data, 0.2d);
            var specimenFactory = new SpecimenFactory(data, specimenInitailizator);
            var selector = new RouletteSelection<Specimen>(false);
            var csvLogger = new CSVLogger<Specimen, RecordBase<Specimen>>("Logs.csv");
            csvLogger.RunLogger();
            var crossover = new OrderCrossover(0.3d);
            var learningManager = new LearningManager(data
                , mutator
                , crossover
                , selector
                , specimenFactory
                , 100
                , csvLogger
                );
            learningManager.Init();
            for (int i = 0; i < 100; i++)
            {
                learningManager.NextEpoch();
            }
            csvLogger.Wait();
            csvLogger.Dispose();
        }
    }
}
else
{
    var configLoader = new LearningConfigLoader();
    foreach(var config in configLoader.Load(learningConfig))
    {
        var dataLoader = new DataLoader();
        var data = dataLoader.Load(config.InputFileName);
        if (data == null)
        {
            Environment.Exit(-1);
        }

        Console.WriteLine($"Test for: {config.OutputFileName}");

        IMutator<Specimen> mutator;
        ISpecimenInitializator<Specimen> specimenInitializator;
        ISelector<Specimen> selector;
        ICrossover<Specimen> crossover;

        if(config.Mutator.Type == MutatorType.Swap)
        {
            mutator = new SwapMutator(data, config.Mutator.MutateRatio);
        }
        else
        {
            mutator = new InverseMutator(data, config.Mutator.MutateRatio);
        }

        if(config.SpecimenInitializator.Type == SpecimenInitializatorType.Random)
        {
            specimenInitializator = new RandomSpecimenInitializator(data, config.SpecimenInitializator.ItemAddPropability);
        }
        else
        {
            specimenInitializator = new GreedySpecimenInitializator(data);
        }

        if(config.Selector.Type == SelectionType.Roulette)
        {
            selector = new RouletteSelection<Specimen>(config.Selector.IsMinimalizing);
        }
        else
        {
            selector = new TournamentSelection<Specimen>(config.Selector.SpecimenCount, config.Selector.IsMinimalizing);
        }

        if(config.Crossover.Type == CrossoverType.Order)
        {
            crossover = new OrderCrossover(config.Crossover.Probability);
        }
        else
        {
            crossover = new PartiallyMatchedCrossover(config.Crossover.Probability);
        }

        var specimenFactory = new SpecimenFactory(data, specimenInitializator);
        var csvLogger = new CSVLogger<Specimen, RecordBase<Specimen>>(config.OutputFileName);
        csvLogger.RunLogger();

        var learningManager = new LearningManager(data
            , mutator
            , crossover
            , selector
            , specimenFactory
            , (uint)config.PopulationSize
            , csvLogger
            );
        learningManager.Init();
        for (int i = 0; i < config.Epochs; i++)
        {
            learningManager.NextEpoch();
        }
        csvLogger.Wait();
        csvLogger.Dispose();
    }
}



