using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class CrossoverConfig
    {
        public CrossoverType Type { get; set; }
        public double Probability { get; set; }
        public int CrossoverLength { get; set; }
    }
}
