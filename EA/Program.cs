using Loggers.CSV;
using TabuSearch.Core;
using TTP.DataTTP;
using TTP.DataTTP.Inititializators;
using TTP.DataTTP.Loggers;
using TTP.DataTTP.Mutators;
using TTP.DataTTP.Neighborhoods;

var dataLoader = new DataLoader();
var data = dataLoader.Load("ai-lab1-ttp_data/medium_1.ttp");
var initializator = new RandomSpecimenInitializator(data, 0.3d);
var factory = new SpecimenFactory(data, initializator);
var mutator = new TabuSwapMutator(data);
var neighbourhood = new Neighbourhood(mutator);
var logger = new CSVLogger<Specimen, TabuRecord>("example.csv");
logger.RunLogger();

var tabuSearch = new TabuSearchManager(data, factory, neighbourhood, logger, 1000, 10, 100);
tabuSearch.RunTabuSearch();

logger.Wait();
logger.Dispose();