using EA;
using EA.DataTTP;
using EA.DataTTP.Inititializators;
using EA.DataTTP.Mutators;
using EA.Core;
using EA.Core.Selectors;
using EA.Core.Loggers.CSV;

Console.WriteLine("Path to TTP files: ");
DirectoryInfo directoryInfo = new DirectoryInfo(Console.ReadLine());
if (!directoryInfo.Exists)
{
    Environment.Exit(-2);
}
foreach(var file in directoryInfo.EnumerateFiles())
{
    if(file.Extension == "tpp")
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
        var csvLogger = new CSVLogger<Specimen, RecordBase<Specimen>>(file.Name);
        var learningManager = new LearningManager(data
            , mutator
            , null
            , selector
            , specimenFactory
            , 100
            , csvLogger
            );
        learningManager.Init();
        for (int i = 0; i < 100; i++)
        {
            //learningManager.NextEpoch();
        }
    }
}


