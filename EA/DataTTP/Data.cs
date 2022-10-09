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
            this.Nodes = new List<Node>();
            this.Items = new List<Item>();
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
        public List<Node> Nodes { get; set; }
        public List<Item> Items { get; set; }

        public Dictionary<(Node from, Node to), double> Distances { get; set; }

        public Dictionary<(Node from, Node to), double> GetNodeMatrix()
        {
            if(this.Distances.Count != 0)
            {
                return this.Distances;
            }
            foreach (var node in Nodes)
            {
                foreach (var otherNode in Nodes)
                {
                    var distance = Math.Sqrt(Math.Pow(node.X - otherNode.X, 2) + Math.Pow(node.Y - otherNode.Y, 2));
                    this.Distances.Add((node, otherNode), distance);
                }
            }
            return this.Distances;
        }
    }
}
