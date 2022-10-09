using EA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Selectors
{
    public class RouletteSelection<T> : ISelector<T> where T : ISpecimen<T>
    {
        public int SpecimenCount { get; set; }

        public RouletteSelection(int specimenCount)
        {
            this.SpecimenCount = specimenCount;
        }

        public virtual IList<T> Select(IList<T> currentPopulation)
        {
            Dictionary<T, (double from, double to)> weightedSpecimens = new Dictionary<T, (double from, double to)>();
            var sum = 0d;
            foreach(var specimen in currentPopulation)
            {
                var score = specimen.Evaluate();
                weightedSpecimens.Add(specimen, (sum, sum + score));
                sum += score;
            }
            Random random = new Random();
            List<T> selectedSpecimens = new List<T>();
            for (int i = 0; i < this.SpecimenCount; i++)
            {
                var value = random.NextDouble() * sum;
                weightedSpecimens.First(ws => ws.Value.from >= value && ws.Value.to < value);
            }
            List<T> newSpecimens = new List<T>(currentPopulation.Count);
            int selectedSpecimenIndex = 0;
            for(int i = 0; i < currentPopulation.Count; i++)
            {
                newSpecimens.Add(selectedSpecimens[selectedSpecimenIndex++].Clone());
                selectedSpecimenIndex %= selectedSpecimens.Count;
            }
            return newSpecimens;
        }
    }
}
