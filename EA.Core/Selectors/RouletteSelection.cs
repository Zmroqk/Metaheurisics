using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Selectors
{
    public class RouletteSelection<T> : ISelector<T> where T : ISpecimen<T>
    {
        public bool IsMinimalizing { get; set; }
        Random random;
        public RouletteSelection(bool isMinimalizing)
        {
            this.IsMinimalizing = isMinimalizing;
            this.random = new Random();
        }

        public virtual IList<T> Select(IList<T> currentPopulation)
        {
            List<(T specimen, double from, double to)> weightedSpecimens = new List<(T specimen, double from, double to)>();
            var sum = 0d;
            var min = double.MaxValue;
            var max = double.MinValue;
            foreach(var specimen in currentPopulation)
            {
                var score = specimen.Evaluate();
                if(score > max)
                {
                    max = score;
                }
                if(score < min)
                {
                    min = score;
                }
            }
            foreach (var specimen in currentPopulation)
            {
                var score = specimen.Evaluate();
                var normalizedScore = this.Normalize(specimen.Evaluate(), max, min);
                weightedSpecimens.Add((specimen, sum, sum + normalizedScore));
                sum += normalizedScore;
            }
            List<T> selectedSpecimens = new List<T>();
            for (int i = 0; i < currentPopulation.Count; i++)
            {
                var value = random.NextDouble() * sum;
                var specimen = weightedSpecimens.First(ws => ws.from <= value && ws.to > value);
                selectedSpecimens.Add(specimen.specimen.Clone());
            }
            return selectedSpecimens;
        }

        private double Normalize(double value, double max, double min)
        {
            if(min == max)
            {
                return 1;
            }
            if (this.IsMinimalizing)
            {
                return (max - value) / (max - min) * 1.001;
            }
            return ((value - min) / (max - min)) * 1.001;
        }
    }
}
