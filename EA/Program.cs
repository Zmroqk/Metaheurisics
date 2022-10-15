using EA.DataTTP;
using EA.DataTTP.Inititializators;
using EA.DataTTP.Mutators;
using EA.Core;
using EA.Core.Selectors;
using EA.Core.Loggers.CSV;
using EA.DataTTP.Crossovers;
using EA.Config;
using EA.DataTTP.Loggers;

Console.WriteLine("Path to Learning config: ");
string learningConfig = Console.ReadLine();
if (string.IsNullOrWhiteSpace(learningConfig))
{
    learningConfig = "LearningManager.json";
}

var configLoader = new LearningConfigLoader();
var tasks = new List<Task>();
var configs = configLoader.Load(learningConfig);
var groups = configs.GroupBy(x => x.RunAsTask).ToList();
var itemIndex = 0;
var itemsCount = groups.Sum(g => g.Count());

Task.Run(PrintProgress);

foreach (var group in groups)
{
    foreach(var config in group.ToList())
    {
        if (group.Key)
        {
            var configCopy = config;
            var task = new Task(() => { LearningTask(configCopy); itemIndex++; });
            tasks.Add(task);
            task.Start();
        }
        else
        {
            LearningTask(config);
            itemIndex++;
        }
    }  
}

Task.WaitAll(tasks.ToArray());

void PrintProgress()
{
    while (true)
    {
        Thread.Sleep(5000);
        var progress = (itemIndex / (double)itemsCount) * 100;
        Console.WriteLine($"Progress: {progress}%");
    }
}

void LearningTask(LearningConfig config)
{
    for (int runCount = 1; runCount <= config.RunCount; runCount++)
    {
        var dataLoader = new DataLoader();
        var data = dataLoader.Load(config.InputFileName);
        if (data == null)
        {
            Environment.Exit(-1);
        }

        var name = config.TestName == null ? string.Format(config.OutputFileName, runCount) : config.TestName;

        Console.WriteLine($"Test number {runCount} for: {name}");

        IMutator<Specimen> mutator;
        ISpecimenInitializator<Specimen> specimenInitializator;
        ISelector<Specimen> selector;
        ICrossover<Specimen> crossover;

        if (config.Mutator.Type == MutatorType.Swap)
        {
            mutator = new SwapMutator(data, config.Mutator.MutateRatio);
        }
        else
        {
            mutator = new InverseMutator(data, config.Mutator.MutateRatio);
        }

        if (config.SpecimenInitializator.Type == SpecimenInitializatorType.Random)
        {
            specimenInitializator = new RandomSpecimenInitializator(data, config.SpecimenInitializator.ItemAddPropability);
        }
        else
        {
            specimenInitializator = new GreedySpecimenInitializator(data);
        }

        if (config.Selector.Type == SelectionType.Roulette)
        {
            selector = new RouletteSelection<Specimen>(config.Selector.IsMinimalizing);
        }
        else
        {
            selector = new TournamentSelection<Specimen>(config.Selector.SpecimenCount, config.Selector.IsMinimalizing);
        }

        if (config.Crossover.Type == CrossoverType.Order)
        {
            crossover = new OrderCrossover(config.Crossover.Probability);
        }
        else
        {
            crossover = new PartiallyMatchedCrossover(config.Crossover.Probability);
        }

        var specimenFactory = new SpecimenFactory(data, specimenInitializator);
        var recordFactory = new RecordFactory((r) => { r.CurrentRun = runCount; });
        var csvLogger = new CSVLogger<Specimen, Record>(string.Format(config.OutputFileName, runCount), recordFactory);
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

        Console.WriteLine($"End for test number {runCount} for: {name}");
    }
}



