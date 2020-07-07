using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.Genetic
{
    public class Algorithm
    {
        private static double uniformRate = 0.5;
        private static double mutationRate =0.95;
        private static int tournamentSize = 5;
        private static bool elitism = true;

        public static Population evolvePopulation(Population pop, int[] tc, int[] sc, int g, double[] stk, double[] dep, int[] rept, int scn, int[][] dl, List<Composant> cc)
        {
            Population newPopulation = new Population(pop.size(), true ,pop.individuals );

            // Keep our best individual
            if (elitism)
            {
                elitism = elitism; // newPopulation.saveIndividual(pop.getMoreCompetent());
            }

            // Crossover population
            int elitismOffset;
            if (elitism)
            {
                elitismOffset = 1;
            }
            else {
                elitismOffset = 0;
            }
            // Loop over the population size and create new individuals with
            // crossover
            for (int i = elitismOffset; i < pop.size(); i++)
            {
                Individual indiv1 = tournamentSelection(pop);
                Individual indiv2 = tournamentSelection(pop);
                Individual newIndiv = crossover(indiv1, indiv2);
               // Individual newIndiv2 = crossover(indiv2, indiv1);
                newIndiv.generateIndividual(newIndiv.getGenes(), tc, sc, g,  stk, dep, rept, scn, dl,  cc);
              //  newIndiv2.generateIndividual(newIndiv2.getGenes(), tc, sc, g, stk, dep, rept, scn, dl, cc);
                newPopulation.saveIndividual( newIndiv);
             //   newPopulation.saveIndividual( newIndiv2);
               
            }

            // Mutate population
            for (int i = elitismOffset; i < pop.size(); i++)
            {
                newPopulation.saveIndividual( mutate(pop.getIndividual(i)));
              
            }

            return newPopulation;
        }
        // Crossover individuals
        private static Individual crossover(Individual indiv1, Individual indiv2)
        {
            Individual newSol = new Individual();
            int separation = new Random().Next(1, indiv1.size() - 2);
            // Loop through genes separation(aleatoire)
            for (int i =separation  ; i < indiv1.size(); i++)
            {
                // Crossover
                if (new Random().Next() <= uniformRate)
                {
                    newSol.setGene(i, indiv1.getGene(i));
                }
                else {
                    newSol.setGene(i, indiv2.getGene(i));
                }
            }
            return newSol;
        }

        // Mutate an individual
        private static Individual  mutate(Individual indiv)
        {
           
            Individual mutant = new Individual();
            mutant.generateIndividual(indiv.getGenes(), indiv.temps, indiv.p, indiv.gg, indiv.stock, indiv.depp, indiv.repture, indiv.s, indiv.DLTS, indiv.composants);
           
            // Loop through genes(a changer, 
            for (int i = 0; i < mutant .size(); i++)
            {
                if (new Random().NextDouble() <= mutationRate)
                {
                    // Create random gene
                    double  gene =mutant .getGene( (int)(new Random().Next(new Random().Next (i,mutant.size()-1),mutant .size()-1)));
                    mutant .setGene((int)(new Random().Next(new Random().Next(i, mutant.size() - 1), mutant.size() - 1)), mutant .getGene(i));
                    mutant .setGene(i, gene);
                }
                mutant.setCompetence(true);


            }
            
            return mutant;
        }

        // Select individuals for crossover
        private static Individual tournamentSelection(Population pop)
        {
            
                int randomId = (int)(new Random().NextDouble() * pop.size());
               return pop.getIndividual(randomId);
            
        }


    }
}
