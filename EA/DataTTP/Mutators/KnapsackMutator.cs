using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.DataTTP.Mutators
{
    public class KnapsackMutator : IMutator<Specimen>
    {
        public double Probability { get => 1; set => _ = value; }
        public bool UseGreedy { get; set; }
        public Data Config { get; set; }

        public KnapsackMutator(Data config, bool useGreedy = true)
        {
            this.Config = config;
            this.UseGreedy = useGreedy;
        }


        public Specimen Mutate(Specimen specimen)
        {
            if (this.UseGreedy)
            {
                this.GreedyMutate(specimen);
            }
            else
            {
                this.RandomMutate(specimen);
            }
            return specimen;
        }

        private void GreedyMutate(Specimen specimen)
        {
            specimen.RemoveAllItemsFromKnapsack();
            var revertedNodes = specimen.Nodes;
            revertedNodes.Reverse();
            foreach (var currentCity in revertedNodes)
            {
                var currentItems = currentCity.AvailableItems.ToList();
                currentItems.Sort((i1, i2) =>
                {
                    var rate1 = (double)i1.Profit / i1.Weight;
                    var rate2 = (double)i2.Profit / i2.Weight;
                    if (rate1 < rate2)
                    {
                        return 1;
                    }
                    else if (rate1 == rate2)
                    {
                        return 0;
                    }
                    return -1;
                });
                while (currentItems.Count > 0 && specimen.AddItemToKnapsack(currentItems.First()) && GetRate(currentItems.First()) >= this.Config.SortedItemMedian())
                {
                    currentItems.RemoveAt(0);
                }
            }
        }

        private void RandomMutate(Specimen specimen)
        {
            var random = new Random();
            specimen.RemoveAllItemsFromKnapsack();
            while (specimen.AddItemToKnapsack(this.Config.Items[random.Next(this.Config.Items.Count)]))
            {
            }
        }

        private double GetRate(Item item)
        {
            return (double)item.Profit / item.Weight;
        }

        public IList<Specimen> MutateAll(IList<Specimen> currentPopulation)
        {
            throw new NotImplementedException();
        }
    }
}
