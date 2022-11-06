using EA.Core;
using EA.Core.Selectors;
using Loggers;
using Loggers.CSV;
using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearch.Core;
using TTP.Config;
using TTP.DataTTP;
using TTP.DataTTP.AdditionalOperations;
using TTP.DataTTP.Crossovers;
using TTP.DataTTP.Inititializators;
using TTP.DataTTP.Loggers;
using TTP.DataTTP.Mutators;
using TTP.DataTTP.Neighborhoods;

namespace TTP.Managers
{
    public class FullSearchParamsManager : IDisposable
    {
        FullSearchConfig FullSearchConfig { get; set; }
        public int MaxThreads { get; set; }

        private Task managerTask;
        private CancellationTokenSource cancellationToken;
        private List<(Task<List<Specimen>> task, IConfig config)> currentTasks;
        private ILogger<FullSearchRecord> logger;
        private IEnumerator<IConfig> Configs;
        private int rowsLogged;
        private int fileIndex;

        public FullSearchParamsManager(FullSearchConfig config, int maxThreads)
        {
            this.MaxThreads = maxThreads;
            this.FullSearchConfig = config;
            this.currentTasks = new List<(Task<List<Specimen>>, IConfig)>();
            this.rowsLogged = 0;
            this.fileIndex = 0;
        }

        public void Run()
        {
            this.Configs = this.GetConfigsEnumerator();
            this.cancellationToken = new CancellationTokenSource();
            this.managerTask = new Task(() =>
            {
                this.ManagerLoop();
            }, this.cancellationToken.Token);
            this.managerTask.Start();
        }

        private void ManagerLoop()
        {
            this.Configs.MoveNext();
            while (!this.cancellationToken.IsCancellationRequested || this.currentTasks.Count != 0)
            {
                for(int i = 0; i < this.currentTasks.Count; i++)
                {
                    if (this.currentTasks[i].task.IsCompleted)
                    {
                        this.LogData(this.currentTasks[i].task.Result, this.currentTasks[i].config);
                        this.currentTasks.RemoveAt(i);
                        i--;
                    }
                }
                for(int i = this.MaxThreads - this.currentTasks.Count; i > 0; i--)
                {                    
                    var config = this.Configs.Current;
                    var canMoveNext = this.Configs.MoveNext();
                    if (!canMoveNext)
                    {
                        this.cancellationToken.Cancel();
                    }
                    else {
                        switch (config)
                        {
                            case LearningConfig:
                                this.currentTasks.Add((Task.Run(() => this.RunEvolutionaryAlgorithm((LearningConfig)config)), config));
                                break;
                            case TabuConfig:
                                this.currentTasks.Add((Task.Run(() => this.RunTabuSearch((TabuConfig)config)), config));
                                break;
                            case SimulatedAnnealingConfig:
                                this.currentTasks.Add((Task.Run(() => this.RunSimulatedAnnealing((SimulatedAnnealingConfig)config)), config));
                                break;
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }

        private double CalculateStandardError(List<Specimen> population)
        {
            var avg = population.Average(x => x.Evaluate());
            return Math.Sqrt(population.Sum(x => Math.Pow(x.Evaluate() - avg, 2))/population.Count)/Math.Sqrt(population.Count);
        }

        private void LogData(List<Specimen> results, IConfig config)
        {
            FullSearchRecord record;
            switch (config)
            {
                case LearningConfig:
                    var learningConfig = (LearningConfig)config;
                    if (this.logger == null || this.rowsLogged == 1000000)
                    {
                        this.RecreateLogger(learningConfig.OutputFileName);
                    }
                    record = new FullSearchRecord()
                    {
                        BestScore = results.Max(x => x.Evaluate()),
                        WorstScore = results.Min(x => x.Evaluate()),
                        AverageScore = results.Average(x => x.Evaluate()),
                        StandardError = this.CalculateStandardError(results),
                        PopulationSize = learningConfig.PopulationSize,
                        CrossoverProbability = learningConfig.Crossover.Probability,
                        MutationProbability = learningConfig.Mutator.MutateRatio,
                        SpecimenCount = learningConfig.Selector.SpecimenCount,
                        CrossoverType = learningConfig.Crossover.Type.ToString(),
                        MutatorType = learningConfig.Mutator.Type.ToString(),
                        SelectorType = learningConfig.Selector.Type.ToString(),
                        Epochs = learningConfig.Epochs,
                        Metaheuristic = "EvolutionaryAlgorithm",
                        FileName = learningConfig.InputFileName,
                        GreeedyKnapsack = true
                    };
                    break;
                case TabuConfig:
                    var tabuConfig = (TabuConfig)config;
                    if (this.logger == null || this.rowsLogged == 1000000)
                    {
                        this.RecreateLogger(tabuConfig.OutputFileName);
                    }
                    record = new FullSearchRecord()
                    {
                        BestScore = results.Max(x => x.Evaluate()),
                        WorstScore = results.Min(x => x.Evaluate()),
                        AverageScore = results.Average(x => x.Evaluate()),
                        StandardError = this.CalculateStandardError(results),
                        Iterations = tabuConfig.Iterations,
                        GreeedyKnapsack = tabuConfig.GreedyKnapsackMutator,
                        TabuSize = tabuConfig.TabuSize,
                        MutatorType = tabuConfig.Mutator.ToString(),
                        NeighborSize = tabuConfig.NeighborhoodSize,   
                        Metaheuristic = "Tabu",
                        FileName = tabuConfig.InputFileName,
                    };
                    break;
                case SimulatedAnnealingConfig:
                    var simulatedAnnealingConfig = (SimulatedAnnealingConfig)config;
                    if (this.logger == null || this.rowsLogged == 1000000)
                    {
                        this.RecreateLogger(simulatedAnnealingConfig.OutputFileName);
                    }
                    record = new FullSearchRecord()
                    {
                        BestScore = results.Max(x => x.Evaluate()),
                        WorstScore = results.Min(x => x.Evaluate()),
                        AverageScore = results.Average(x => x.Evaluate()),
                        StandardError = this.CalculateStandardError(results),
                        Iterations = simulatedAnnealingConfig.Iterations,
                        GreeedyKnapsack = simulatedAnnealingConfig.GreedyKnapsackMutator,
                        MutatorType = simulatedAnnealingConfig.Mutator.ToString(),
                        NeighborSize = simulatedAnnealingConfig.NeighborhoodSize,
                        StartingTemperature = simulatedAnnealingConfig.StartingTemperature,
                        TargetTemperature = simulatedAnnealingConfig.TargetTemperature,
                        AnnealingRate = simulatedAnnealingConfig.AnnealingRate,
                        Metaheuristic = "SimulatedAnnealing",
                        FileName = simulatedAnnealingConfig.InputFileName,
                    };
                    break;
                default:
                    return;
            }
            this.rowsLogged++;
            this.logger.Log(record);
        }

        private void RecreateLogger(string path)
        {
            if(this.logger != null)
            {
                var csvLogger = (CSVLogger<Specimen, FullSearchRecord>)this.logger;
                csvLogger.Wait();
                csvLogger.Dispose();
            }
            this.rowsLogged = 0;
            this.logger = new CSVLogger<Specimen, FullSearchRecord>(path.Insert(path.LastIndexOf('.'), $"_{this.fileIndex++}"));
            ((CSVLogger<Specimen, FullSearchRecord>)this.logger).RunLogger();
        }

        private IEnumerator<IConfig> GetConfigsEnumerator()
        {
            foreach (var filePath in this.FullSearchConfig.FilePaths)
            {
                IEnumerator<IConfig> enumerator;
                enumerator = this.GenerateTabuConfigs(filePath, this.FullSearchConfig);
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
                enumerator = this.GenerateEvolutionaryAlgorithmConfigs(filePath, this.FullSearchConfig);
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
                enumerator = this.GenerateSimulatedAnnealingConfigs(filePath, this.FullSearchConfig);
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        #region Generate
        private IEnumerator<IConfig> GenerateTabuConfigs(string filePath, FullSearchConfig fullSearchConfig)
        {
            var tabuConfig = fullSearchConfig.TabuConfig;
            if(tabuConfig == null)
            {
                yield break;
            }
            foreach (var useGreedyKnapsack in tabuConfig.UseGreedyKnapsack)
            {
                foreach(var mutator in tabuConfig.Mutators)
                {
                    foreach(var specimenInitializator in tabuConfig.SpecimenInitializators)
                    {
                        for(int neigbourhoodSize = tabuConfig.MinNeighborhoodSize; neigbourhoodSize < tabuConfig.MaxNeighborhoodSize; neigbourhoodSize += tabuConfig.NeighborhoodSizeChange)
                        {
                            for (int tabuSize = tabuConfig.MinTabuSize; tabuSize < tabuConfig.MaxTabuSize; tabuSize += tabuConfig.TabuSizeChange)
                            {
                                for (int iterations = tabuConfig.MinIterations; iterations < tabuConfig.MaxIterations; iterations += tabuConfig.IterationsChange)
                                {
                                    var config = new TabuConfig()
                                    {
                                        GreedyKnapsackMutator = useGreedyKnapsack,
                                        Mutator = mutator,
                                        Iterations = iterations,
                                        InputFileName = filePath,
                                        SpecimenInitializator = specimenInitializator,
                                        NeighborhoodSize = neigbourhoodSize,
                                        RunCount = fullSearchConfig.RunCount,
                                        TabuSize = tabuSize,
                                        OutputFileName = fullSearchConfig.OutputPath
                                    };
                                    yield return config;
                                }
                            }
                        }
                    }
                }
            }
        }
        private IEnumerator<IConfig> GenerateEvolutionaryAlgorithmConfigs(string filePath, FullSearchConfig fullSearchConfig)
        {
            FullSearchEvolutionaryAlgorithm? algorithm = fullSearchConfig.EvolutionaryAlgorithm;
            if (algorithm == null)
            {
                yield break;
            }
            foreach (var mutator in algorithm.Mutator.Types)
            {
                foreach (var crossover in algorithm.Crossover.Types)
                {
                    foreach (var selector in algorithm.Selector.Types)
                    {
                        foreach (var specimenInitializator in algorithm.SpecimenInitializators)
                        {
                            for (double mutatorProb = algorithm.Mutator.MinMutateRatio; mutatorProb < algorithm.Mutator.MaxMutateRatio; mutatorProb += algorithm.Mutator.MutateRatioChange)
                            {
                                for (double crossoverProb = algorithm.Crossover.MinProbability; crossoverProb < algorithm.Crossover.MaxProbability; crossoverProb += algorithm.Crossover.ProbabilityChange)
                                {
                                    for (int specimenCount = algorithm.Selector.MinSpecimenCount; specimenCount < algorithm.Selector.MaxSpecimenCount; specimenCount += algorithm.Selector.SpecimenCountChange)
                                    {
                                        for (int populationSize = algorithm.MinPopulationSize; populationSize < algorithm.MaxPopulationSize; populationSize += algorithm.PopulationSizeChange)
                                        {
                                            for (int epochs = algorithm.MinEpochs; epochs < algorithm.MaxEpochs; epochs += algorithm.EpochsChange)
                                            {
                                                var config = new LearningConfig()
                                                {
                                                    Mutator = new MutatorConfig()
                                                    {
                                                        Type = mutator,
                                                        MutateRatio = mutatorProb
                                                    },
                                                    Selector = new SelectorConfig()
                                                    {
                                                        Type = selector,
                                                        SpecimenCount = specimenCount,
                                                    },
                                                    Crossover = new CrossoverConfig()
                                                    {
                                                        Type = crossover,
                                                        Probability = crossoverProb,
                                                    },
                                                    InputFileName = filePath,
                                                    Epochs = epochs,
                                                    PopulationSize = populationSize,
                                                    RunCount = fullSearchConfig.RunCount,
                                                    SpecimenInitializator = specimenInitializator,
                                                    OutputFileName = fullSearchConfig.OutputPath
                                                };
                                                yield return config;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }                  
                }
            }
        }
        private IEnumerator<IConfig> GenerateSimulatedAnnealingConfigs(string filePath, FullSearchConfig fullSearchConfig)
        {
            FullSearchSimulatedAnnealing? simulatedAnnealing = fullSearchConfig.SimulatedAnnealing;
            if (simulatedAnnealing == null)
            {
                yield break;
            }
            foreach (var useGreedyKnapsack in simulatedAnnealing.UseGreedyKnapsack)
            {
                foreach (var mutator in simulatedAnnealing.Mutators)
                {
                    foreach (var specimenInitializator in simulatedAnnealing.SpecimenInitializators)
                    {
                        for (int neigbourhoodSize = simulatedAnnealing.MinNeighborhoodSize; neigbourhoodSize < simulatedAnnealing.MaxNeighborhoodSize; neigbourhoodSize += simulatedAnnealing.NeighborhoodSizeChange)
                        {
                            for (double startingTemperature = simulatedAnnealing.MinStartingTemperature; startingTemperature < simulatedAnnealing.MaxStartingTemperature; startingTemperature += simulatedAnnealing.StartingTemperatureChange)
                            {
                                for (double targetTemperature = simulatedAnnealing.MinTargetTemperature; targetTemperature < simulatedAnnealing.MaxTargetTemperature; targetTemperature += simulatedAnnealing.TargetTemperatureChange)
                                {
                                    for (double annealingRate = simulatedAnnealing.MinAnnealingRate; annealingRate < simulatedAnnealing.MaxAnnealingRate; annealingRate += simulatedAnnealing.AnnealingRateChange)
                                    {
                                        for (int iterations = simulatedAnnealing.MinIterations; iterations < simulatedAnnealing.MaxIterations; iterations += simulatedAnnealing.IterationsChange)
                                        {
                                            var config = new SimulatedAnnealingConfig()
                                            {
                                                GreedyKnapsackMutator = useGreedyKnapsack,
                                                Mutator = mutator,
                                                Iterations = iterations,
                                                InputFileName = filePath,
                                                SpecimenInitializator = specimenInitializator,
                                                NeighborhoodSize = neigbourhoodSize,
                                                RunCount = fullSearchConfig.RunCount,
                                                StartingTemperature = startingTemperature,
                                                TargetTemperature = targetTemperature,
                                                AnnealingRate = annealingRate,
                                                OutputFileName = fullSearchConfig.OutputPath
                                            };
                                            yield return config;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private List<Specimen> RunTabuSearch(TabuConfig tabuConfig)
        {
            var results = new List<Specimen>();
            for(int i = 0; i < tabuConfig.RunCount; i++)
            {
                var dataLoader = new DataLoader();
                var data = dataLoader.Load(tabuConfig.InputFileName);
                ISpecimenInitializator<Specimen> initializator;
                if (tabuConfig.SpecimenInitializator.Type == SpecimenInitializatorType.Greedy)
                {
                    initializator = new GreedySpecimenInitializator(data, new KnapsackMutator(data, true));
                }
                else
                {
                    initializator = new RandomSpecimenInitializator(data, tabuConfig.SpecimenInitializator.ItemAddPropability);
                }
                var factory = new SpecimenFactory(data, initializator);
                IMutator<Specimen> mutator;
                if (tabuConfig.Mutator == MutatorType.Swap)
                {
                    mutator = new TabuSwapMutator(data);
                }
                else
                {
                    mutator = new InverseMutator(data, 1);
                }
                var knapsackMutator = new KnapsackMutator(data, tabuConfig.GreedyKnapsackMutator);
                var neighbourhood = new Neighbourhood(mutator, knapsackMutator);

                var tabuSearch = new TabuSearchManager(data, factory, neighbourhood, null, tabuConfig.Iterations, tabuConfig.NeighborhoodSize, tabuConfig.TabuSize);
                results.Add(tabuSearch.RunTabuSearch());
            }
            return results;
        }

        private List<Specimen> RunSimulatedAnnealing(SimulatedAnnealingConfig simulatedAnnealingConfig)
        {
            var results = new List<Specimen>();
            for (int i = 0; i < simulatedAnnealingConfig.RunCount; i++)
            {
                var dataLoader = new DataLoader();
                var data = dataLoader.Load(simulatedAnnealingConfig.InputFileName);
                ISpecimenInitializator<Specimen> initializator;
                if (simulatedAnnealingConfig.SpecimenInitializator.Type == SpecimenInitializatorType.Greedy)
                {
                    initializator = new GreedySpecimenInitializator(data, new KnapsackMutator(data, true));
                }
                else
                {
                    initializator = new RandomSpecimenInitializator(data, simulatedAnnealingConfig.SpecimenInitializator.ItemAddPropability);
                }
                var factory = new SpecimenFactory(data, initializator);
                IMutator<Specimen> mutator;
                if (simulatedAnnealingConfig.Mutator == MutatorType.Swap)
                {
                    mutator = new TabuSwapMutator(data);
                }
                else
                {
                    mutator = new InverseMutator(data, 1);
                }
                var knapsackMutator = new KnapsackMutator(data, simulatedAnnealingConfig.GreedyKnapsackMutator);
                var neighbourhood = new Neighbourhood(mutator, knapsackMutator);

                var simulatedAnnealing = new SimulatedAnnealingManager(neighbourhood
                    , factory
                    , null
                    , simulatedAnnealingConfig.AnnealingRate
                    , simulatedAnnealingConfig.Iterations
                    , simulatedAnnealingConfig.NeighborhoodSize
                    , simulatedAnnealingConfig.StartingTemperature
                    , simulatedAnnealingConfig.TargetTemperature
                    );
                results.Add(simulatedAnnealing.RunSimulatedAnnealing());
            }
            return results;
        }

        private List<Specimen> RunEvolutionaryAlgorithm(LearningConfig learningConfig)
        {
            var results = new List<Specimen>();
            for (int i = 0; i < learningConfig.RunCount; i++)
            {
                var dataLoader = new DataLoader();
                var data = dataLoader.Load(learningConfig.InputFileName);
                if (data == null)
                {
                    Environment.Exit(-1);
                }

                IMutator<Specimen> mutator;
                ISpecimenInitializator<Specimen> specimenInitializator;
                ISelector<Specimen> selector;
                ICrossover<Specimen> crossover;

                if (learningConfig.Mutator.Type == MutatorType.Swap)
                {
                    mutator = new SwapMutator(data, learningConfig.Mutator.MutateRatio);
                }
                else
                {
                    mutator = new InverseMutator(data, learningConfig.Mutator.MutateRatio);
                }

                if (learningConfig.SpecimenInitializator.Type == SpecimenInitializatorType.Random)
                {
                    specimenInitializator = new RandomSpecimenInitializator(data, learningConfig.SpecimenInitializator.ItemAddPropability);
                }
                else
                {
                    specimenInitializator = new GreedySpecimenInitializator(data, new KnapsackMutator(data, true));
                }

                if (learningConfig.Selector.Type == SelectionType.Roulette)
                {
                    selector = new RouletteSelection<Specimen>(learningConfig.Selector.IsMinimalizing);
                }
                else
                {
                    selector = new TournamentSelection<Specimen>(learningConfig.Selector.SpecimenCount, learningConfig.Selector.IsMinimalizing);
                }

                if (learningConfig.Crossover.Type == CrossoverType.Order)
                {
                    crossover = new OrderCrossover(learningConfig.Crossover.Probability);
                }
                else
                {
                    crossover = new PartiallyMatchedCrossover(learningConfig.Crossover.Probability);
                }

                var additionalOperations = new AdditionalOperationsHandler(new KnapsackMutator(data, true));
                var specimenFactory = new SpecimenFactory(data, specimenInitializator);

                var learningManager = new LearningManager(data
                    , mutator
                    , crossover
                    , selector
                    , specimenFactory
                    , (uint)learningConfig.PopulationSize
                    , null
                    , additionalOperations
                    );
                learningManager.Init();
                for (int j = 0; j < learningConfig.Epochs; j++)
                {
                    learningManager.NextEpoch();
                }
                results.Add(learningManager.Best);
            }
            return results;
        }

        public void Wait()
        {
            this.managerTask.Wait();
        }

        public void Dispose()
        {
            this.cancellationToken.Cancel();
            this.managerTask = null;
        }
    }
}
