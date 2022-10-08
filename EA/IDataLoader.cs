using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA
{
    public interface IDataLoader<T> where T : class
    {
        T? Load(string path);
    }
}
