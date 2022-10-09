using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core
{
    public interface ISpecimenFactory<T> where T : ISpecimen<T>
    {
        T CreateSpecimen();
    }
}
