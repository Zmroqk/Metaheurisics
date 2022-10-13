using EA.Core;
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

        public TournamentSelection(int specimenCount, bool isMinimalizing)
        {
            this.SpecimenCount = specimenCount;
            this.IsMinimalizing = isMinimalizing;
        }

        public virtual IList<T> Select(IList<T> currentPopulation)
        {
            Random random = new Random();
            List<T> selectedSpecimens = new List<T>();
            while(selectedSpecimens.Count != currentPopulation.Count)
            {
                List<T> tournamentSelectedSpecimens = new List<T>();
                for(int j = 0; j < this.SpecimenCount; j++)
                {
                    var index = random.Next(currentPopulation.Count);
                    tournamentSelectedSpecimens.Add(currentPopulation[index]);
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
