using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class Node
    {
        public Node()
        {
            this.Index = 0;
            this.X = 0;
            this.Y = 0;
        }

        public Node(int index, decimal x, decimal y)
        {
            Index = index;
            X = x;
            Y = y;
        }

        public int Index { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
