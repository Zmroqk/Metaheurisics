using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.Config
{
    public class LazyConfigLoader<TConfig> : IConfigLoader<IEnumerator<TConfig>> where TConfig : IConfig
    {
        public bool ExitWhenFileNotExists { get; set; }
        public LazyConfigLoader(bool exitWhenFileNotExists = true)
        {
            ExitWhenFileNotExists = exitWhenFileNotExists;
        }

        public IEnumerator<TConfig> Load(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using(JsonTextReader jsonReader = new JsonTextReader(new StreamReader(fileStream)))
                    {
                        jsonReader.Read();
                    }
                }
            }
            else
            {
                if (this.ExitWhenFileNotExists)
                {
                    Environment.Exit(-3);
                }
                yield break;
            }
        }
    }
}
