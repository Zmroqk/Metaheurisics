using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Loggers.CSV
{
    public interface IRecord<T> where T : ISpecimen<T>
    {
        int CurrentEpoch { get; set; }
        double MaxSpecimenScore { get; set; }
        double MinSpecimenScore { get; set; }
        double AverageSpecimenScore { get; set; }
    }
}
