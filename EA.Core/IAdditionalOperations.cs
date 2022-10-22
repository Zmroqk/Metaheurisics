using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core
{
    public interface IAdditionalOperations<T> where T : ISpecimen<T>
    {
        IList<T> BeforeSelect(IList<T> currentPopulation);
        IList<T> AfterSelect(IList<T> currentPopulation);
        IList<T> BeforeMutation(IList<T> currentPopulation);
        IList<T> AfterMutation(IList<T> currentPopulation);
        IList<T> BeforeCrossover(IList<T> currentPopulation);
        IList<T> AfterCrossover(IList<T> currentPopulation);
    }
}
