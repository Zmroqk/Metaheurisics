using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.EA
{
    public interface ICrossover<T> where T : ISpecimen
    {
        List<T> Crossover(List<T> specimens);
    }
}
