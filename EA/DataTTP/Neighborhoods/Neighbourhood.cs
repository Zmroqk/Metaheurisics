using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearch.Core;

namespace TTP.DataTTP.Neighborhoods
{
    public class Neighbourhood : INeighborhood<Specimen>
    {
        public IMutator<Specimen> Mutator { get; set; }

        public Neighbourhood(IMutator<Specimen> mutator)
        {
            this.Mutator = mutator;
        }

        public IEnumerable<Specimen> FindNeighborhood(Specimen specimen, int size)
        {
            var neighborhoods = new List<Specimen>();
            for(int i = 0; i < size; i++)
            {
                var newSpecimen = this.Mutator.Mutate(specimen.Clone());
                KnapsackHelper.GreedyKnapsack(newSpecimen);
                neighborhoods.Add(newSpecimen);
            }
            return neighborhoods;
        }
    }
}
