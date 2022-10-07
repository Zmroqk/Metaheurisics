using EA.EA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class Specimen : ISpecimen
    {
        public Specimen()
        {
            Items = new HashSet<Item>();
            Nodes = new HashSet<Node>();
        }

        public HashSet<Item> Items { get; }

        public HashSet<Node> Nodes { get; }

        public void Fix()
        {
            throw new NotImplementedException();
        }
    }
}
