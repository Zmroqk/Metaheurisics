using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class MutatorConfig
    {
        public MutatorType Type { get; set; }
        public double MutateRatio { get; set; }
        public int NodeSwappingCount { get; set; }
        public int ItemSwappingCount { get; set; }
        public int? NodeSwappingLength { get; set; }
    }
}
