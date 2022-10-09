using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP.Mutators
{
    public class InverseMutator : IMutator<Specimen>
    {
        public Data Config { get; set; }
        public double MutateRatio { get; set; }

        public int NodeSwappingCount { get; set; }
        public int ItemSwappingCount { get; set; }
        public int NodeSwappingLength { get; set; }
        public double Probability { 
            get
            {
                return this.MutateRatio;
            } 
            set
            {
                this.MutateRatio = value;
            }
        }

        public InverseMutator(Data config, double mutateRatio, int nodeSwappingCount, int itemSwappingCount, int nodeSwappingLength)
        {
            this.Config = config;
            this.MutateRatio = mutateRatio;
            this.NodeSwappingCount = nodeSwappingCount;
            this.ItemSwappingCount = itemSwappingCount;
            this.NodeSwappingLength = nodeSwappingLength;
        }

        public IList<Specimen> Mutate(IList<Specimen> currentPopulation)
        {
            Random random = new Random();
            foreach(Specimen specimen in currentPopulation)
            {
                if(random.Next() <= this.MutateRatio)
                {
                    for(int i = 0; i < this.NodeSwappingCount; i++)
                    {
                        var index = random.Next(specimen.Nodes.Count-NodeSwappingLength);
                        var swappedNodes = specimen.Nodes.GetRange(index, this.NodeSwappingLength);
                        swappedNodes.Reverse();
                        specimen.Nodes.RemoveRange(index, NodeSwappingCount);
                        specimen.Nodes.InsertRange(index, swappedNodes);
                    }
                    //TODO Should be moved to own MutateRatio if?
                    //TODO How inversion should work for items
                    for (int i = 0; i < this.ItemSwappingCount; i++)
                    {
                        //var items = specimen.GetKnapsackItems();
                        //var index1 = random.Next(items.Length);
                        //var index2 = random.Next(this.Config.Items.Count);
                        //specimen.RemoveItemFromKnapsack(items[index1]);
                        //specimen.AddItemToKnapsack(this.Config.Items[index2]);
                    }
                }
            }
            return currentPopulation;
        }
    }
}
