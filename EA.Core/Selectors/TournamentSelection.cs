using Meta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Selectors
{
    public class TournamentSelection<T> : ISelector<T> where T : ISpecimen<T>
    {
        public int SpecimenCount { get; set; }
        public bool IsMinimalizing { get; set; }
        Random random;
        public TournamentSelection(int specimenCount, bool isMinimalizing)
        {
            this.SpecimenCount = specimenCount;
            this.IsMinimalizing = isMinimalizing;
            this.random = new Random();
        }

        public virtual IList<T> Select(IList<T> currentPopulation)
        {
            List<T> selectedSpecimens = new List<T>(currentPopulation.Count);
            var tournamentCount = (int)Math.Round((this.SpecimenCount / 100d) * currentPopulation.Count);
            tournamentCount = tournamentCount == 0 ? 1 : tournamentCount;
            while (selectedSpecimens.Count != currentPopulation.Count)
            {
                List<T> currentTempPopulation = currentPopulation.ToList();
                List<T> tournamentSelectedSpecimens = new List<T>(tournamentCount);
                for(int j = 0; j < tournamentCount; j++)
                {
                    var index = random.Next(currentTempPopulation.Count);
                    tournamentSelectedSpecimens.Add(currentTempPopulation[index]);
                    currentTempPopulation.RemoveAt(index);
                }
                if (this.IsMinimalizing)
                {
                    selectedSpecimens.Add(tournamentSelectedSpecimens.MinBy(this.Evaluate).Clone());
                }
                else
                {
                    selectedSpecimens.Add(tournamentSelectedSpecimens.MaxBy(this.Evaluate).Clone());
                }
            }
            return selectedSpecimens;
        }

        private double Evaluate(T specimen)
        {
            return specimen.Evaluate();
        }
    }
}
