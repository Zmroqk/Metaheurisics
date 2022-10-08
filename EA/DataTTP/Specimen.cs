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
            Items = new Dictionary<Item, bool>();
            Nodes = new List<Node>();
        }

        public Dictionary<Item, bool> Items { get; }

        public List<Node> Nodes { get; }

        public double Evaluate()
        {
            throw new NotImplementedException();
        }

        public void Fix()
        {
            throw new NotImplementedException();
        }
    }
}
