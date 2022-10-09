﻿using EA;
using EA.DataTTP;
using EA.DataTTP.Inititializators;
using EA.DataTTP.Mutators;
using EA.Core;
using EA.Core.Selectors;
using EA.Core.Loggers.CSV;
using EA.DataTTP.Crossovers;
using EA.Config;

Console.WriteLine("Path to TTP files: ");
DirectoryInfo directoryInfo = new DirectoryInfo(Console.ReadLine());
if (!directoryInfo.Exists)
{
    Environment.Exit(-2);
}
Console.WriteLine("Path to Learning config: ");
string learningConfig = Console.ReadLine();
if (string.IsNullOrWhiteSpace(learningConfig))
{
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

            var mutator = new SwapMutator(data, 0.1d, 5, 5);
            var specimenInitailizator = new RandomSpecimenInitializator(data, 0.2d);
            var specimenFactory = new SpecimenFactory(data, specimenInitailizator);
            var selector = new RouletteSelection<Specimen>(10);
            var csvLogger = new CSVLogger<Specimen, RecordBase<Specimen>>("Logs.csv");
            csvLogger.RunLogger();
            var crossover = new OrderCrossover(0.3d, 5);
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

                IMutator<Specimen> mutator;
                ISpecimenInitializator<Specimen> specimenInitializator;
                ISelector<Specimen> selector;
                ICrossover<Specimen> crossover;

                if(config.Mutator.Type == MutatorType.Swap)
                {
                    mutator = new SwapMutator(data, config.Mutator.MutateRatio, config.Mutator.NodeSwappingCount, config.Mutator.ItemSwappingCount);
                }
                else
                {
                    mutator = new InverseMutator(data, config.Mutator.MutateRatio, config.Mutator.NodeSwappingCount, config.Mutator.ItemSwappingCount, config.Mutator.NodeSwappingLength.Value);
                }

                if(config.SpecimenInitializator.Type == SpecimenInitializatorType.Random)
                {
                    specimenInitializator = new RandomSpecimenInitializator(data, config.SpecimenInitializator.ItemAddPropability);
                }
                else
                {
                    specimenInitializator = new GreedySpecimenInitializator(data, config.SpecimenInitializator.ItemAddPropability);
                }

                if(config.Selector.Type == SelectionType.Roulette)
                {
                    selector = new RouletteSelection<Specimen>(config.Selector.SpecimenCount);
                }
                else
                {
                    selector = new TournamentSelection<Specimen>(config.Selector.SpecimenCount, config.Selector.TournamentCount.Value);
                }

                if(config.Crossover.Type == CrossoverType.Order)
                {
                    crossover = new OrderCrossover(config.Crossover.Probability, config.Crossover.CrossoverLength);
                }
                else
                {
                    crossover = new PartiallyMatchedCrossover(config.Crossover.Probability, config.Crossover.CrossoverLength);
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
    }
}



