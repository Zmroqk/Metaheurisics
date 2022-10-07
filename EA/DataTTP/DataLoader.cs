using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP
{
    public class DataLoader : IDataLoader
    {
        public void Load(string path)
        {
            string[] lines = File.ReadAllLines(path);
            try
            {
                Data data = new Data();
                data.Name = lines[0].Split('\t')[1];
                data.Type = lines[1].Split(':')[1].Trim();
                data.CityCount = int.Parse(lines[2].Split('\t')[1]);
                data.NumberOfItems = int.Parse(lines[3].Split('\t')[1]);
                data.KnapsackCapacity = int.Parse(lines[4].Split('\t')[1]);
                data.MinSpeed = decimal.Parse(lines[5].Split('\t')[1]);
                data.MaxSpeed = decimal.Parse(lines[6].Split('\t')[1]);
                data.RentingRatio = decimal.Parse(lines[7].Split('\t')[1]);
                data.EdgeWeightType = lines[8].Split('\t')[1];
                for(int i = 10; i < 10 + data.CityCount; i++)
                {
                    var lineData = lines[i].Split('\t');
                    data.Nodes.Add(new Node(int.Parse(lineData[0])
                        , decimal.Parse(lineData[1])
                        , decimal.Parse(lineData[2])
                        )
                    );
                }
                for (int i = 11 + data.CityCount; i < 11 + data.NumberOfItems + data.CityCount; i++)
                {
                    var lineData = lines[i].Split('\t');
                    data.Items.Add(new Item(int.Parse(lineData[0])
                        , int.Parse(lineData[1])
                        , int.Parse(lineData[2])
                        , int.Parse(lineData[3])
                        )
                    );
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Incorrect format");
            }
        }
    }
}
