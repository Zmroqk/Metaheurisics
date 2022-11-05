using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.DataTTP.Mutators
{
    public class TabuSwapMutator : SwapMutator
    {
        public TabuSwapMutator(Data config) : base(config, 1)
        {
        }

        public override Specimen Mutate(Specimen specimen)
        {
            Random random = new Random();
            var index = random.Next(specimen.Nodes.Count);
            var index2 = random.Next(specimen.Nodes.Count);
            var swappedNode = specimen.Nodes[index];
            specimen.Nodes[index] = specimen.Nodes[index2];
            specimen.Nodes[index2] = swappedNode;
            specimen.IsMutated = true;
            return specimen;
        }
    }
}
