using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Config
{
    public class LearningConfigLoader
    {
        public List<LearningConfig> Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<List<LearningConfig>>(File.ReadAllText(path));
            }
            else
            {
                Environment.Exit(-3);
                return null;
            }
        }
    }
}
