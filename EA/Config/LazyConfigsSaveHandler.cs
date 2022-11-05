using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTP.Config
{
    public class LazyConfigsSaveHandler : ISaveHandler<IEnumerator<IConfig>>
    {
        public string SavePath { get; set; }
        public LazyConfigsSaveHandler(string savePath)
        {
            this.SavePath = savePath;
        }

        public void Save(IEnumerator<IConfig> configs)
        {
            using(FileStream fileStream = new FileStream(this.SavePath, FileMode.Create, FileAccess.Write))
            {
                using(JsonTextWriter jsonTextWriter = new JsonTextWriter(new StreamWriter(fileStream)))
                {
                    jsonTextWriter.WriteStartObject();
                    jsonTextWriter.WriteToken()
                }
            }
            File.WriteAllText(this.SavePath, configsString);
        }
    }
}
