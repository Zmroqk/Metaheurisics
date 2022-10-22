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
            var newPopulation = new List<Specimen>();
            foreach (Specimen specimen in currentPopulation)
            {
                var newSpecimen = specimen.Clone();
                for(int i = 0; i < newSpecimen.Nodes.Count; i++)
                {
                    if (probability <= random.NextDouble())
                    {
                        var index2 = random.Next(newSpecimen.Nodes.Count);
                        var swappedNode = newSpecimen.Nodes[i];
                        newSpecimen.Nodes[i] = newSpecimen.Nodes[index2];
                        newSpecimen.Nodes[index2] = swappedNode;
                        newSpecimen.IsMutated = true;
                    }
                }
                newPopulation.Add(newSpecimen);
            }
            return newPopulation;
        }
    }
}
