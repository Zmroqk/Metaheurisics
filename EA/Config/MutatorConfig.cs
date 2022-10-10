using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class MutatorConfig
    {
        /// <summary>
        /// Mutator method to be used
        /// </summary>
        public MutatorType Type { get; set; }

        /// <summary>
        /// Mutate probability
        /// </summary>
        public double MutateRatio { get; set; }

        /// <summary>
        /// Node swapping count
        /// </summary>
        public int NodeSwappingCount { get; set; }

        /// <summary>
        /// Item swapping count
        /// </summary>
        public int ItemSwappingCount { get; set; }

        /// <summary>
        /// Node swapping length only used by Inverse Mutator
        /// </summary>
        public int? NodeSwappingLength { get; set; }
    }
}
