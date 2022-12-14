using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.Config
{
    public class FullSearchSelector
    {
        public SelectionType[] Types { get; set; }
        public int MinSpecimenCount { get; set; }
        public int MaxSpecimenCount { get; set; }
        public int SpecimenCountChange { get; set; } = 1;
    }
}
