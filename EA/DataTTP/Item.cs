using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class Item
    {
        public Item()
        {
            this.Index = 0;
            this.Profit = 0;
            this.Weight = 0;
            this.NodeIndex = 0;
        }
        public Item(int index, int profit, int weight, int nodeIndex)
        {
            this.Index = index;
            this.Profit = profit;
            this.Weight = weight;
            this.NodeIndex = nodeIndex;
        }

        int Index { get; set; }
        int Profit { get; set; }
        int Weight { get; set; }
        int NodeIndex { get; set; }
    }
}
