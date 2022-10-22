using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabuSearch.Core
{
    public interface INeighborhood<TSpecimen> where TSpecimen : ISpecimen<TSpecimen>
    {
        IEnumerable<TSpecimen> FindNeighborhood(TSpecimen specimen, int size);
    }
}
