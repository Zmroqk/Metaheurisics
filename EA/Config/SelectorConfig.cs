using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class SelectorConfig
    {
        /// <summary>
        /// Selector method to be used
        /// </summary>
        public SelectionType Type { get; set; }
        /// <summary>
        /// Specimen count which should be selected
        /// </summary>
        public int SpecimenCount { get; set; }
    }
}
