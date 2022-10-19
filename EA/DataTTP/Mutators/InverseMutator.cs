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
            var newPopulation = new List<Specimen>();
            foreach(Specimen specimen in currentPopulation)
            {
                var newSpecimen = specimen.Clone();
                if (probability <= random.NextDouble())
                {
                    var startIndex = random.Next(newSpecimen.Nodes.Count);
                    var length = random.Next(newSpecimen.Nodes.Count - startIndex);
                    var swappedNodes = newSpecimen.Nodes.GetRange(startIndex, length);
                    swappedNodes.Reverse();
                    newSpecimen.Nodes.RemoveRange(startIndex, length);
                    newSpecimen.Nodes.InsertRange(startIndex, swappedNodes);
                    newSpecimen.IsMutated = true;
                }
                newPopulation.Add(newSpecimen);
            }
            return newPopulation;
        }
    }
}
