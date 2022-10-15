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
                    var startIndex = random.Next(specimen.Nodes.Count);
                    var length = random.Next(specimen.Nodes.Count - startIndex);
                    var swappedNodes = specimen.Nodes.GetRange(startIndex, length);
                    swappedNodes.Reverse();
                    specimen.Nodes.RemoveRange(startIndex, length);
                    specimen.Nodes.InsertRange(startIndex, swappedNodes);
                    specimen.IsMutated = true;
                }
            }
            return currentPopulation;
        }
    }
}
