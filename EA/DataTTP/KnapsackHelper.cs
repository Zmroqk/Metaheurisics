using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class KnapsackHelper
    {
        public static void GreedyKnapsack(Specimen specimen)
        {
            foreach (var currentCity in specimen.Nodes)
            {
                var currentItems = currentCity.AvailableItems.ToList();
                currentItems.Sort((i1, i2) =>
                {
                    var rate1 = i1.Profit / i1.Weight;
                    var rate2 = i2.Profit / i2.Weight;
                    if (rate1 > rate2)
                    {
                        return 1;
                    }
                    else if (rate1 == rate2)
                    {
                        return 0;
                    }
                    return -1;
                });
                specimen.RemoveAllItemsFromKnapsack();
                while (currentItems.Count > 0 && specimen.AddItemToKnapsack(currentItems.First()))
                {
                    currentItems.RemoveAt(0);
                }
            }
        }
    }
}
