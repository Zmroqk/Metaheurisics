using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP.Inititializators
{
    public class GreedySpecimenInitializator : ISpecimenInitializator<Specimen>
    {
        public Data Config { get; set; }

        public GreedySpecimenInitializator(Data config)
        {
            this.Config = config;
        }

        public void Initialize(Specimen specimen)
        {
            var cities = this.Config.Nodes.ToList();
            var distanceMatrix = this.Config.GetNodeMatrix();
            var random = new Random();
            var currentCity = cities[random.Next(0, cities.Count)];
            cities.Remove(currentCity);
            specimen.Nodes.Add(currentCity);
            while (cities.Count > 0)
            {
                var selected = distanceMatrix.Where(x => x.Key.From == currentCity 
                    && cities.Contains(x.Key.To)
                    && x.Key.From != x.Key.To
                    ).MaxBy(x => x.Value);
                currentCity = selected.Key.To;
                cities.Remove(currentCity);
                specimen.Nodes.Add(currentCity);
            }
            KnapsackHelper.GreedyKnapsack(specimen);
        }
    }
}
