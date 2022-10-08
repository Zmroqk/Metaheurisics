using EA.EA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP.Inititializators
{
    public class RandomSpecimenInitializator : ISpecimenInitializator<Specimen>
    {
        public Data Config { get; set; }
        public double ItemAddProbability { get; set; }

        public RandomSpecimenInitializator(Data config, double itemAddProbability)
        {
            this.Config = config;
            this.ItemAddProbability = itemAddProbability;
        }

        public void Initialize(Specimen specimen)
        {
            List<Node> cities = this.Config.Nodes.ToList();
            List<Item> items = this.Config.Items.ToList();
            Random random = new Random();
            while(cities.Count > 0)
            {
                var city = cities[random.Next(0, cities.Count)];
                specimen.Nodes.Add(city);
                if(random.Next() >= ItemAddProbability || ItemAddProbability == 1)
                {
                    specimen.AddItemToKnapsack(city.AvailableItems[random.Next(city.AvailableItems.Count)]);
                }
            }
        }
    }
}
