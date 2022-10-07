using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class Data
    {
        public Data()
        {
            this.Nodes = new HashSet<Node>();
            this.Items = new HashSet<Item>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public int CityCount { get; set; }
        public int NumberOfItems { get; set; }
        public int KnapsackCapacity { get; set; }
        public decimal MinSpeed { get; set; }
        public decimal MaxSpeed { get; set; }
        public decimal RentingRatio { get; set; }
        public string EdgeWeightType { get; set; }
        public HashSet<Node> Nodes { get; set; }
        public HashSet<Item> Items { get; set; }
    }
}
