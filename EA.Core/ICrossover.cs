using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core
{
    public interface ICrossover<T> where T : ISpecimen<T>
    {
        double Probability { get; set; }
        IList<T> Crossover(IList<T> specimens);
    }
}
