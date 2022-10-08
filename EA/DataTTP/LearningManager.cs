using EA.EA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class LearningManager : LearningManagerBase<Specimen>
    {
        public Data Config { get; set; }

        public LearningManager(Data config, IMutator<Specimen> mutator, ICrossover<Specimen> crossover, ISelector<Specimen> selector)
            : base(mutator, crossover, selector)
        {
            this.Config = config;
        }
    }
}
