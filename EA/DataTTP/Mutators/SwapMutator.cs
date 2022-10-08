using EA.EA;
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

        public SwapMutator(Data config, double mutateRatio)
        {
            Config = config;
            MutateRatio = mutateRatio;
        }

        public IList<Specimen> Mutate(IList<Specimen> currentPopulation)
        {
            Random random = new Random();
            foreach(Specimen specimen in currentPopulation)
            {

            }
            return null;
        }
    }
}
