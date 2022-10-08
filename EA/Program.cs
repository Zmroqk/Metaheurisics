using EA;
using EA.DataTTP;
using EA.DataTTP.Inititializators;
using EA.DataTTP.Mutators;
using EA.EA;

var dataLoader = new DataLoader();
var data = dataLoader.Load("ai-lab1-ttp_data/easy_0.ttp");
if(data == null)
{
    Environment.Exit(-1);
}

var mutator = new SwapMutator(data, 0.1d);
var specimenInitailizator = new RandomSpecimenInitializator(data, 0.2d);
var specimenFactory = new SpecimenFactory(data, specimenInitailizator);
var learningManager = new LearningManager(data
    , mutator
    , null
    , null
    , specimenFactory
    , 100
    );
learningManager.Init();
//learningManager.NextEpoch();
