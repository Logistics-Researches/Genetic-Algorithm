using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.Genetic
{
    public class Ga
    {
        public static void Main(String[] args)
        {

            
            
            
            
            
            
            // Set a candidate solution
            Skill.setSolution("111100000");

            // Create an initial population
            Population myPop = new Population(50, true,new List<Individual>());

            // Evolve our population until we reach an optimum solution
            int generationCount = 0;
            while (myPop.getMoreCompetent().getCompetence() < Skill.getMaxSkill())
            {
                generationCount++;
                Console.WriteLine ("Generation: " + generationCount + " competence: " + myPop.getMoreCompetent().getCompetence());
               // myPop = Algorithm.evolvePopulation(myPop);
            }
            Console.WriteLine("Solution found!");
            Console.WriteLine("Generation: " + generationCount);
            Console.WriteLine("Genes:");
            Console.WriteLine(myPop.getMoreCompetent());

        }
    }
}
