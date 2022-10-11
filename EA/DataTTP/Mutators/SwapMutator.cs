using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP.Mutators
{
    public class SwapMutator : IMutator<Specimen>
    {
        public Data Config { get; set; }
        public double MutateRatio { get; set; }
        public double Probability
        {
            get
            {
                return this.MutateRatio;
            }
            set
            {
                this.MutateRatio = value;
            }
        }

        public SwapMutator(Data config, double mutateRatio)
        {
            this.Config = config;
            this.MutateRatio = mutateRatio;
        }

        public IList<Specimen> Mutate(IList<Specimen> currentPopulation)
        {
            Random random = new Random();
            var probability = 1 - this.MutateRatio;
            foreach(Specimen specimen in currentPopulation)
            {
                for(int i = 0; i < specimen.Nodes.Count; i++)
                {
                    if (probability <= random.NextDouble())
                    {
                        var index2 = random.Next(specimen.Nodes.Count);
                        var swappedNode = specimen.Nodes[i];
                        specimen.Nodes[i] = specimen.Nodes[index2];
                        specimen.Nodes[index2] = swappedNode;
                    }
                }
                KnapsackHelper.GreedyKnapsack(specimen);
            }
            return currentPopulation;
        }
    }
}
