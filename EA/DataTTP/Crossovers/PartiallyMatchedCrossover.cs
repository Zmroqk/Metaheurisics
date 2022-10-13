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

        public PartiallyMatchedCrossover(double probability)
        {
            this.Probability = probability;
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
                    specimen1.IsCrossed = true;
                    specimen2.IsCrossed = true;
                    newSpecimens.Add(specimen1);
                    newSpecimens.Add(specimen2);
                }
                else
                {
                    newSpecimens.Add(specimens[i - 1].Clone());
                    newSpecimens.Add(specimens[i].Clone());
                }
            }
            if(specimens.Count % 2 == 0)
            {
                var specimen = specimens[specimens.Count - 1].Clone();
                newSpecimens.Add(specimen);
            }
            return newSpecimens;
        }

        private (Specimen, Specimen) CrossSpecimens(Specimen specimen, Specimen otherSpecimen)
        {
            var newSpecimen = specimen.Clone();
            var newOtherSpecimen = otherSpecimen.Clone();
            var random = new Random();
            var startIndex = random.Next(specimen.Nodes.Count);
            var length = specimen.Nodes.Count - startIndex;
            var specimenRange = newSpecimen.Nodes.GetRange(startIndex, length);
            var otherSpecimenRange = newOtherSpecimen.Nodes.GetRange(startIndex, length);
            newSpecimen.Nodes.RemoveRange(startIndex, length);
            newOtherSpecimen.Nodes.RemoveRange(startIndex, length);
            newSpecimen.Nodes.InsertRange(startIndex, otherSpecimenRange);
            newSpecimen.Nodes.InsertRange(startIndex, specimenRange);

            var mapping = this.CreateMapping(specimenRange, otherSpecimenRange);

            for(int i = 0; i < newSpecimen.Nodes.Count; i++)
            {
                if(i < startIndex || i > startIndex + length)
                {
                    newSpecimen.Nodes[i] = mapping[newSpecimen.Nodes[i]][random.Next(mapping[newSpecimen.Nodes[i]].Count)];
                }
            }
            for (int i = 0; i < newOtherSpecimen.Nodes.Count; i++)
            {
                if (i < startIndex || i > startIndex + length)
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
