using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.Genetic
{
    public class Population
    {
        public List<Individual> individuals;

        /*
         * Constructor
         */
        // Create a population
        public Population(int populationSize, bool initialise,List<Individual> ind)
        {
            individuals = new List<Individual>();
            Individual[] f = new Individual[ind.Count];
            // Initialise population
            if (initialise)
            {
                ind.CopyTo(f);
                individuals = f.ToList<Individual>();
            }
        }
        public void reducePop()
        {
            individuals.Sort(); ;
            individuals.RemoveRange(individuals.Count / 2, individuals.Count/2 - 1);
        }

        /* Getters */
        public Individual getIndividual(int index)
        {
            return individuals[index];
        }

        public Individual getMoreCompetent()
        {
            Individual moreCompetent = individuals[0];
            
            // Loop through individuals to find more competent
            for (int i = 0; i < size(); i++)
            {
                double x = getIndividual(i).getCompetence();
                if (individuals[i]!=null )
                if (moreCompetent.getCompetence() >= getIndividual(i).getCompetence())
                {
                    moreCompetent = getIndividual(i);
                }
            }
            return moreCompetent;
        }

        /* Public methods */
        // Get population size
        public int size()
        {
            return individuals.Count  ;
        }

        // Save individual
        public void saveIndividual( Individual indiv)
        {
            individuals.Add( indiv);
        }

    }
}
