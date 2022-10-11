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

        public TournamentSelection(int specimenCount)
        {
            this.SpecimenCount = specimenCount;
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
                selectedSpecimens.Add(tournamentSelectedSpecimens.MaxBy(s => s.Evaluate()).Clone());
            }
            return selectedSpecimens;
        }
    }
}
