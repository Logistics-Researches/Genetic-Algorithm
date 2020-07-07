using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.Genetic
{
    public class Skill
    {
        static int[] solution = new int[9];

        /* Public methods */
        // Set a candidate solution as a int array
        public static void setSolution(int[] newSolution)
        {
            solution = newSolution;
        }

        // To make it easier we can use this method to set our candidate solution
        // with string of 0s and 1s
        public static void setSolution(String newSolution)
        {
            solution = new int[newSolution.Length];
            // Loop through each character of our string and save it in our int
            // array
            for (int i = 0; i < newSolution.Length ; i++)
            {
                String character = newSolution.Substring(i, 1);
                if (character.Contains("0") || character.Contains("1"))
                {
                     int.TryParse(character,out solution[i]);
                }
                else {
                    solution[i] = 0;
                }
            }
        }

        // Compute skill by comparing it to our candidate solution
        public static int getSkill(Individual individual)
        {
            int skill = 0;
            // Loop through our individuals genes and compare them to our candidates
            for (int i = 0; i < individual.size() && i < solution.Length; i++)
            {
                if (individual.getGene(i) == solution[i])
                {
                    skill++;
                }
            }
            return skill;
        }

        // Get optimum skill
        public static int getMaxSkill()
        {
            int maxSkill = solution.Length;
            return maxSkill;
        }
    }
}
