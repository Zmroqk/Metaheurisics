using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.DataTTP
{
    public class KnapsackHelper
    {
        public static void GreedyKnapsack(Specimen specimen)
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
                int maxCountInCity = 3;
                while (currentItems.Count > 0 && specimen.AddItemToKnapsack(currentItems.First()) && maxCountInCity > 0)
                {
                    currentItems.RemoveAt(0);
                    maxCountInCity--;
                }
            }
        }

        //public static void GreedyKnapsack(Specimen specimen)
        //{
        //    specimen.RemoveAllItemsFromKnapsack();
        //    var currentItems = specimen.Nodes.SelectMany(n => n.AvailableItems).ToList();
        //    currentItems.Sort((i1, i2) =>
        //    {
        //        var rate1 = (double)i1.Profit / i1.Weight;
        //        var rate2 = (double)i2.Profit / i2.Weight;
        //        if (rate1 < rate2)
        //        {
        //            return 1;
        //        }
        //        else if (rate1 == rate2)
        //        {
        //            return 0;
        //        }
        //        return -1;
        //    });
        //    while (currentItems.Count > 0 && specimen.AddItemToKnapsack(currentItems.First()))
        //    {
        //        currentItems.RemoveAt(0);
        //    }
        //}
    }
}
