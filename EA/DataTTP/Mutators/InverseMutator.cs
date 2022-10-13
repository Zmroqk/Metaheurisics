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

        public InverseMutator(Data config, double mutateRatio)
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
                if (probability <= random.NextDouble())
                {
                    for (int i = 0; i < specimen.Nodes.Count; i++)
                    {
                        var length = random.Next(specimen.Nodes.Count - i);
                        var swappedNodes = specimen.Nodes.GetRange(i, length);
                        swappedNodes.Reverse();
                        specimen.Nodes.RemoveRange(i, length);
                        specimen.Nodes.InsertRange(i, swappedNodes);
                        specimen.IsMutated = true;
                    }
                }
            }
            return currentPopulation;
        }
    }
}
