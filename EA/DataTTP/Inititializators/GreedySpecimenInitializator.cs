using EA.Core;
using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTP.DataTTP.Mutators;

namespace TTP.DataTTP.Inititializators
{
    public class GreedySpecimenInitializator : ISpecimenInitializator<Specimen>
    {
        public Data Config { get; set; }
        public IMutator<Specimen> KnapsackMutator { get; set; }

        public GreedySpecimenInitializator(Data config, IMutator<Specimen> knapsackMutator)
        {
            this.Config = config;
            this.KnapsackMutator = knapsackMutator;
        }

        public void Initialize(Specimen specimen)
        {
            var cities = this.Config.Nodes.ToList();
            var indexes = Enumerable.Range(0, this.Config.Nodes.Count).ToList();
            var distanceMatrix = this.Config.GetNodeMatrix();
            var random = new Random();
            var currentCity = cities[random.Next(0, cities.Count)];
            cities.Remove(currentCity);
            specimen.Nodes.Add(currentCity);
            indexes.Remove(currentCity.Index);
            while (cities.Count > 0)
            {
                var infos = distanceMatrix[currentCity.Index-1];
                var maxDistance = 0d;
                var selectedInfo = infos[0];
                foreach(var info in infos)
                {
                    if(info.From != info.To && cities.Contains(info.To) && maxDistance < info.Distance)
                    {
                        selectedInfo = info;
                        maxDistance = info.Distance;
                    }
                }
                currentCity = selectedInfo.To;
                cities.Remove(currentCity);
                specimen.Nodes.Add(currentCity);
                indexes.Remove(currentCity.Index);
            }
            this.KnapsackMutator.Mutate(specimen);
        }
    }
}
