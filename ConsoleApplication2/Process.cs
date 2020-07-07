using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using ConsoleApplication2.Genetic;

namespace ConsoleApplication2
{
    public class Process
    {
        string instance;
        int periode;
        int nbr_comp;
        public static int nbr_scenarios;
        int[][] capacités;
        int[][] dlt;
        double[] stockCost;
        int[] setupCost;
        int[] reptCost;
        int[] timeCap;
        int tempsG;
        double[] depassCost;
        List<Composant> composants;
        Iteration[] iterations;
        int[][] XtS;
        double[][][] itp;
        double[][][] itm;
        int[] zt;
        List<int> K;
        int xx;
        List<List<int>> combinaisonsK;
        static int comb = 0;


        public Process()
        {

        }

        [STAThread]
        public string lecture()
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            
            return File.ReadAllText(ofd.FileName).Trim();
        }
        public void initialisation(string fichier)
        {
            instance = fichier;//Récuperer le contenu du fichier de l'instance
            string[] rr = instance.Replace('\r', '\0').Split('\n');
            periode = Convert.ToInt16(rr[0]);//la première ligne contient "période"
            nbr_comp = Convert.ToInt16(rr[1]);// la deuxième
            nbr_scenarios = Convert.ToInt16(rr[2]);//la troisième
            composants = new List<Composant>();//ici on créé une liste(tableau) qui va contenir la liste des composants
            Composant c;//c est un objet de type Composant, il contiendra les attributs de chaque composant--->
            dlt = new int[periode  ][];//dlt, ici juste la déclaration, l'inistialisation se fait après
                                       //cout de stockage,  rr[4+nbr_comp+periode] signifie:la lecture de 4 premières valeurs se fait en incrémentant la position par 1, mais à partir la 5ème position, on doit calculer la position des prochaines valeurs dans le fichier (selon nbr_comp et periode)ok?
            stockCost = new double [nbr_comp];
            setupCost = new int[periode];
            reptCost=new int[nbr_comp];
            timeCap=new int[periode];
            setupCost = new int[periode];
            tempsG= Convert.ToInt16(rr[4 + nbr_comp + periode+5]);
            depassCost =new  double [periode];
            for (int i=0;i<nbr_comp;i++)
            {
                stockCost[i] = Convert.ToInt16(rr[4 + nbr_comp + periode+1].Split(' ').ElementAt(i));

                c = new Composant();
                c.setNuméro(i);
                c.setRendement(Convert.ToInt16(rr[3].ToString().Split(' ').ElementAt(i)));                
                c.setSI(Convert.ToInt16(rr[4].ToString().Trim().Split(' ').ElementAt(i)));
                int[] d = new int[periode];
                
                c.setDemandes(d);
                reptCost[i] = Convert.ToInt16(rr[4 + nbr_comp + periode + 3].Split(' ').ElementAt(i));
                for (int j=0;j< periode;j++)
                {
                   
                    d[j] = Convert.ToInt16(rr[i+5].ToString().Trim().Split(' ').ElementAt(j));
                    setupCost[j]= Convert.ToInt16(rr[4 + nbr_comp + periode+2].Split(' ').ElementAt(j));
                    timeCap[j]= Convert.ToInt16(rr[4 + nbr_comp + periode + 4].Split(' ').ElementAt(j));
                    depassCost[j]= Convert.ToInt16(rr[4 + nbr_comp + periode + 6].Split(' ').ElementAt(j));

                    try {
                        dlt[j][i] = Convert.ToInt16(rr[j + 5 + nbr_comp].ToString().Trim().Split(' ').ElementAt(i));

                    }
                    catch(Exception e)
                    {
                        dlt[j] = new int[nbr_scenarios];
                        dlt[j][i] = Convert.ToInt16(rr[j + 5 + nbr_comp].ToString().Trim().Split(' ').ElementAt(i));
                        
                    }
                    

                }
               
                c.setDemandes(d);
                composants.Add(c);
             

            }

             


        }
        [STAThread]
        public static void Main(string [] args)
        {

            List<List<int>> fesdin = new List<List<int>>();
            Stopwatch stopwatch = new Stopwatch();           
            CIteration c;
            Process p = new Process();
            p.initialisation(p.lecture());
            p.combinaisonsK = new List<List<int>>();
            int[] arr=new int[p.periode ] ;
            for (int i = 0; i < arr.Length; i++)
                arr[i] = i;
            int r = 0;
            int n = arr.Length;
            for (int i = 2; i < p.periode - 2; i++)
                p.printCombination(arr, n, i);
            p.combinaisonsK = p.combinaisonsK.Distinct<List<int>>(new EqualityParity()).ToList();

            Individual individu = new Individual();
            Individual.setDefaultGeneLength(p.periode);
            List<Individual> lstInd = new List<Individual>();
           
            stopwatch.Start();
            double sm = 0;
            int nb = 0;
            double[] zzt = new double[p.periode];
            for (int i = 0; i < 80; i++)
            {
                c = new CIteration(p.periode, i + 1, p.composants, p.dlt, nbr_scenarios, p.setupCost, p.tempsG, p.timeCap, p.stockCost, p.reptCost, p.depassCost);
                c.K = p.combinaisonsK[i];
                zzt = c.generationZt();
                sm = zzt.Sum();
                
                individu = new Individual();
                individu.generateIndividual(zzt, p.timeCap, p.setupCost, p.tempsG, p.stockCost, p.depassCost, p.reptCost, nbr_scenarios, p.dlt,p.composants );
                
                individu.setK(c.K);
                individu.setCompetence(Convert.ToDouble(c.coutSetup + c.coutDep + c.couSTK + c.coutRepture));
                lstInd.Add(individu);
            }
            MessageBox.Show(nb.ToString());

            Population myPop = new Population(80, true, lstInd);
            Individual best = myPop.getMoreCompetent();
            Console.WriteLine(best.kstring());
            for (int i = 0; i < best.getGenes().Length; i++)
                Console.Write(best.getGenes()[i] + " | ");
            Console.WriteLine("Cout best pop initiale" + best.getCompetence());
            int generationCount = 0;
            while (generationCount<10)
            {
                
                Console.WriteLine("Generation: " + generationCount +"zt "+ myPop.getMoreCompetent().toString()+ " competence: " + myPop.getMoreCompetent().getCompetence()+" taille= "+myPop.size());
                myPop = Algorithm.evolvePopulation(myPop, p.timeCap, p.setupCost, p.tempsG, p.stockCost, p.depassCost, p.reptCost, nbr_scenarios, p.dlt, p.composants);
                myPop.reducePop();
                generationCount++;
            }
            Console.WriteLine("Solution found!");
            Console.WriteLine("Generation: " + generationCount);
            Console.WriteLine("Genes:");
            Console.WriteLine(myPop.getMoreCompetent());
















            stopwatch.Stop();

            // IHM.
            Console.WriteLine("Durée d'exécution: {0}", stopwatch.Elapsed.Milliseconds);
            Console.ReadKey();
        }

        private static int[] Shuffle(int n)
        {
            var random = new Random();
            var result = new int[n];
            for (var i = 0; i < n; i++)
            {
                var j = random.Next(0, i + 1);
                if (i != j)
                {
                    result[i] = result[j];
                }
                result[j] = i;
            }
            return result;
        }






        void combinationUtil(int[] arr, int[] data,
                                int start, int end,
                                int index, int r)
        {
            List<int> kint = new List<int>();
            // Current combination is  
            // ready to be printed,  
            // print it 
            if (index == r)
            {
                for (int j = 0; j < r; j++)
                    kint.Add(data[j]);


                if (kint.Count != 0)
                {
                    /*if (kint[0] != 0)
                        kint.Insert(0, 0);*/
                    if (kint[kint.Count - 1] != periode - 1)
                        kint.Add(periode-1);

                    combinaisonsK.Add(kint);
                    comb++;

                    return;
                }
            }

            // replace index with all 
            // possible elements. The  
            // condition "end-i+1 >=  
            // r-index" makes sure that  
            // including one element 
            // at index will make a  
            // combination with remaining  
            // elements at remaining positions 
            for (int i = start; i <= end &&
                      end - i + 1 >= r - index; i++)
            {
                data[index] = arr[i];
                combinationUtil(arr, data, i + 1,
                                end, index + 1, r);
            }
        }

        // The main function that prints 
        // all combinations of size r 
        // in arr[] of size n. This  
        // function mainly uses combinationUtil() 
        void printCombination(int[] arr,
                                          int n, int r)
        {
            // A temporary array to store  
            // all combination one by one 
            int[] data = new int[r];

            // Print all combination  
            // using temprary array 'data[]' 
            combinationUtil(arr, data, 0,
                           n-1, 0, r);
        }



    }
}
