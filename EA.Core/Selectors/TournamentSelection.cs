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
        public int TournamentCount { get; set; }

        public TournamentSelection(int specimenCount, int tournamentCount)
        {
            this.SpecimenCount = specimenCount;
            this.TournamentCount = tournamentCount;
        }

        public virtual IList<T> Select(IList<T> currentPopulation)
        {
            Random random = new Random();
            List<T> selectedSpecimens = new List<T>();
            for (int i = 0; i < this.TournamentCount; i++)
            {
                List<T> tournamentSelectedSpecimens = new List<T>();
                for(int j = 0; j < this.SpecimenCount; j++)
                {
                    var index = random.Next(currentPopulation.Count);
                    tournamentSelectedSpecimens.Add(currentPopulation[index]);
                }
                selectedSpecimens.Add(tournamentSelectedSpecimens.MaxBy(s => s.Evaluate()));
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
