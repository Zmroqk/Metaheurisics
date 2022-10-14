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
            this.Distances = new DistanceInfo[0][];
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

        public DistanceInfo[][] Distances { get; set; }

        public DistanceInfo[][] GetNodeMatrix()
        {
            if(this.Distances.Length != 0)
            {
                return this.Distances;
            }
            this.Distances = new DistanceInfo[this.CityCount][];
            foreach (var node in Nodes)
            {
                this.Distances[node.Index - 1] = new DistanceInfo[this.CityCount];
                foreach (var otherNode in Nodes)
                {
                    var distance = Math.Sqrt(Math.Pow(node.X - otherNode.X, 2) + Math.Pow(node.Y - otherNode.Y, 2));
                    this.Distances[node.Index - 1][otherNode.Index - 1] = new DistanceInfo()
                    {
                        Distance = distance,
                        From = node,
                        To = otherNode
                    };
                }
            }
            return this.Distances;
        }

        public double GetDistance(Node first, Node second)
        {
            return this.GetNodeMatrix()[first.Index - 1][second.Index - 1].Distance;
        }
    }
}
