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
        public double ItemAddProbability { get; set; }

        public GreedySpecimenInitializator(Data config, double itemAddProbability)
        {
            this.Config = config;
            this.ItemAddProbability = itemAddProbability;
        }

        public void Initialize(Specimen specimen)
        {
            var cities = this.Config.Nodes.ToList();
            var distanceMatrix = new Dictionary<(Node from, Node to), double>(this.Config.GetNodeMatrix());
            var random = new Random();
            var currentCity = cities[random.Next(0, cities.Count)];
            cities.Remove(currentCity);
            specimen.Nodes.Add(currentCity);
            var probability = 1 - this.ItemAddProbability;
            while (cities.Count > 0)
            {
                var selected = distanceMatrix.Where(x => x.Key.from == currentCity && !cities.Contains(x.Key.to)).MaxBy(x => x.Value);
                currentCity = selected.Key.to;
                cities.Remove(currentCity);
                specimen.Nodes.Add(currentCity);
                if (probability <= random.NextDouble() && currentCity.AvailableItems.Count > 0)
                {
                    specimen.AddItemToKnapsack(currentCity.AvailableItems[random.Next(currentCity.AvailableItems.Count)]);
                }
            }
        }
    }
}
