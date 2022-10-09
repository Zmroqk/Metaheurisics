using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core
{
    public interface ILogger<T> where T : ISpecimen<T>
    {
        Task Log(int currentEpoch, IList<T> currentEpochSpecimens);
    }
}
