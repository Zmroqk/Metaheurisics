using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.EA
{
    public interface ISpecimenInitializator<T> where T : ISpecimen<T>
    {
        void Initialize(T specimen);
    }
}
