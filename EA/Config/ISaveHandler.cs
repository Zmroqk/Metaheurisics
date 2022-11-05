using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.Config
{
    public interface ISaveHandler<TConfig> where TConfig : IConfig
    {
        void Save(TConfig configs);
    }
}
