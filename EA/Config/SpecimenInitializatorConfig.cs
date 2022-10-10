using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class SpecimenInitializatorConfig
    {
        /// <summary>
        /// Initialization method to be used
        /// </summary>
        public SpecimenInitializatorType Type { get; set; }

        /// <summary>
        /// Item add probability at node
        /// </summary>
        public double ItemAddPropability { get; set; }
    }
}
