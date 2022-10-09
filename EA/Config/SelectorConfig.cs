using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class SelectorConfig
    {
        public SelectionType Type { get; set; }
        public int SpecimenCount { get; set; }
        public int? TournamentCount { get; set; }
    }
}
