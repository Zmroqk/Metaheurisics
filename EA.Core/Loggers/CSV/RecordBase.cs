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
        public int SpecimenIndex { get; set; }
        public double SpecimenScore { get; set; }

        public void ApplyData(int currentEpoch, int specimenIndex, T specimen)
        {
            this.CurrentEpoch = currentEpoch;
            this.SpecimenIndex = specimenIndex;
            this.SpecimenScore = specimen.Evaluate();
        }
    }
}
