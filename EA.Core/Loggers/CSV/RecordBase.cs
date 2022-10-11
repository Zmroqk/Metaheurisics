using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Loggers.CSV
{
    public class RecordBase<T> : IRecord<T> where T : ISpecimen<T>
    {
        public int CurrentEpoch { get; set; }
        public double MaxSpecimenScore { get; set; }
        public double MinSpecimenScore { get; set; }
        public double AverageSpecimenScore { get; set; }
    }
}
