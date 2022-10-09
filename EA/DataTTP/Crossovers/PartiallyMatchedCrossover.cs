using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP.Crossovers
{
    public class PartiallyMatchedCrossover : ICrossover<Specimen>
    {
        public double Probability { get; set; }
        public int CrossLength { get; set; }

        public PartiallyMatchedCrossover(double probability, int crossLength)
        {
            this.Probability = probability;
            this.CrossLength = crossLength;
        }

        public IList<Specimen> Crossover(IList<Specimen> specimens)
        {
            var random = new Random();
            var prop = 1 - this.Probability;
            var newSpecimens = new List<Specimen>();
            for (int i = 1; i < specimens.Count; i += 2)
            {
                if (prop <= random.NextDouble())
                {
                    var (specimen1, specimen2) = this.CrossSpecimens(specimens[i - 1], specimens[i]);
                    newSpecimens.Add(specimen1);
                    newSpecimens.Add(specimen2);
                }
                else
                {
                    newSpecimens.Add(specimens[i-1].Clone());
                    newSpecimens.Add(specimens[i].Clone());
                }
            }
            if(specimens.Count % 2 == 0)
            {
                newSpecimens.Add(specimens[specimens.Count - 1].Clone());
            }
            return newSpecimens;
        }

        private (Specimen, Specimen) CrossSpecimens(Specimen specimen, Specimen otherSpecimen)
        {
            var newSpecimen = specimen.Clone();
            var newOtherSpecimen = otherSpecimen.Clone();
            var random = new Random();
            var startIndex = random.Next(specimen.Nodes.Count - this.CrossLength);
            var specimenRange = newSpecimen.Nodes.GetRange(startIndex, this.CrossLength);
            var otherSpecimenRange = newOtherSpecimen.Nodes.GetRange(startIndex, this.CrossLength);
            newSpecimen.Nodes.RemoveRange(startIndex, this.CrossLength);
            newOtherSpecimen.Nodes.RemoveRange(startIndex, this.CrossLength);
            newSpecimen.Nodes.InsertRange(startIndex, otherSpecimenRange);
            newSpecimen.Nodes.InsertRange(startIndex, specimenRange);

            var mapping = this.CreateMapping(specimenRange, otherSpecimenRange);

            for(int i = 0; i < newSpecimen.Nodes.Count; i++)
            {
                if(i < startIndex || i > startIndex + this.CrossLength)
                {
                    newSpecimen.Nodes[i] = mapping[newSpecimen.Nodes[i]][random.Next(mapping[newSpecimen.Nodes[i]].Count)];
                }
            }
            for (int i = 0; i < newOtherSpecimen.Nodes.Count; i++)
            {
                if (i < startIndex || i > startIndex + this.CrossLength)
                {
                    newSpecimen.Nodes[i] = mapping[newSpecimen.Nodes[i]][random.Next(mapping[newSpecimen.Nodes[i]].Count)];
                }
            }

            newSpecimen.Fix();
            newOtherSpecimen.Fix();
            return (newSpecimen, newOtherSpecimen);
        }

        private Dictionary<Node, List<Node>> CreateMapping(List<Node> map1, List<Node> map2)
        {
            var mapping = new Dictionary<Node, List<Node>>();
            for(int i = 0; i < map1.Count; i++)
            {
                var node = map1[i];
                if (!mapping.ContainsKey(node))
                {
                    mapping.Add(node, new List<Node>());                   
                }
                mapping[node].Add(map2[i]);
            }
            for (int i = 0; i < map2.Count; i++)
            {
                var node = map2[i];
                if (!mapping.ContainsKey(node))
                {
                    mapping.Add(node, new List<Node>());
                }
                mapping[node].Add(map1[i]);
            }
            return mapping;
        }
    }
}
