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
            this.Distances = new Dictionary<(Node, Node), double>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public int CityCount { get; set; }
        public int NumberOfItems { get; set; }
        public int KnapsackCapacity { get; set; }
        public double MinSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double RentingRatio { get; set; }
        public string EdgeWeightType { get; set; }
        public HashSet<Node> Nodes { get; set; }
        public HashSet<Item> Items { get; set; }

        public Dictionary<(Node, Node), double> Distances { get; set; }

        public Dictionary<(Node, Node), double> GetNodeMatrix()
        {
            if(this.Distances.Count != 0)
            {
                return this.Distances;
            }
            foreach (var node in Nodes)
            {
                foreach (var otherNode in Nodes)
                {
                    var distance = Math.Sqrt(Math.Pow(node.X, 2) + Math.Pow(node.Y, 2));
                    this.Distances.Add((node, otherNode), distance);
                }
            }
            return this.Distances;
        }
    }
}
