using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.Genetic
{
    public class Individual:IComparable<Individual>
    {
        static int defaultGeneLength = 9;
        private double[] genes = new double[defaultGeneLength];
        List<int> K;
        public int s;
        double[][][] itp;
        double[][][] itm;
        double[][] XtS;
        public List<Composant> composants;
        int[][] xtp;
        public double couSTK;
        public double coutRepture;
        public int coutSetup;
        public double coutDep;
        public int[] p;
        public double[] stock;
        public int[] temps;
        public int gg;
        public int[] repture;
        public double[] depp;
        // Cache
        public double competence = 0;
        public int[][] DLTS;

        // Create a random individual
        public void generateIndividual()
        {
            for (int i = 0; i < size(); i++)
            {
                int gene = (int)Math.Round(new Random().NextDouble());
                genes[i] = gene;
            }
        }
        public void setK(List<int>m)
        {
            K = new List<int>();
            K = m;
        }
        public string kstring()
        {
            string s = "";
            for (int i = 0; i < K.Count; i++)
                s += K.ElementAt(i) + " | ";
            return s;
        }
        public void generateIndividual(double [] zt,int[]tc,int[]sc,int g,double[] stk,double[]dep,int[]rept,int scn,int[][]dl,List<Composant> cc)
        {
            genes = zt;
            coutRepture = 0;
            couSTK = 0;
            coutSetup = 0;
            coutSetup = 0;
            temps = tc;
            p = sc;
            gg = g;
            stock = stk;
            depp = dep;
            repture = rept;
            s = scn;
       
            DLTS = dl;
            composants = cc;
            prep_XtS();
            prep_ipmt();
            CxTs();

            for (int c = 0; c < composants.Count; c++)
                for (int ss = 0; ss < s; ss++)
                    for (int x = 0; x < genes.Length; x++)
                    {
                        citp(x, ss, c);
                        imt(x, ss, c);
                    }
            costSetup();
            depassCost();
            coutStock();
            coutReptt();
            setCompetence(true);

        }
        public void prep_XtS()
        {


            XtS = new double[s][];
            xtp = new int[s][];
            for (int i = 0; i < s; i++)
            {
                XtS[i] = new double[genes.Length ];
                xtp[i] = new int[genes.Length ];
            }
        }
        public void prep_ipmt()
        {
            itp = new double[composants.Count ][][];
            itm = new double[composants.Count ][][];
            foreach (Composant c in composants)
            {
                itp[c.getNum()] = new double[s][];
                for (int i = 0; i < s; i++)
                    itp[c.getNum()][i] = new double[genes.Length ];
            }
            foreach (Composant c in composants)
            {
                itm[c.getNum()] = new double[s][];
                for (int i = 0; i < s; i++)
                    itm[c.getNum()][i] = new double[genes.Length ];
            }
        }
        public double imt(int x, int s, int c)
        {
            int xp = 0;
            double res = 0.0;
            if (x - 1 <= 0)
                return 0;
            xp = x - 1;

            double ccc = itm[c][s][x - 1] + composants.ElementAt<Composant>(c).getDemandeAt(x);
            double ccc1 = GxTs(x, s) * composants.ElementAt<Composant>(c).getRendement();
            double ccc2 = citp(x - 1, s, c);
            res = itm[c][s][x - 1] + composants.ElementAt<Composant>(c).getDemandeAt(x) - GxTs(x, s) * composants.ElementAt<Composant>(c).getRendement() - citp(x - 1, s, c);
            if (res > 0)
            {
                itm[c][s][x] = res;
                return res;
            }
            itm[c][s][x] = 0;
            return 0;
        }
        public double GxTs(int t, int ss)
        {
            return XtS[ss][t];
        }
        public void CxTs()
        {

            for (int i = 0; i < s; i++)
                XtS[i][0] = 0;

            for(int ss=0;ss< s;ss++)
                for(int t=0;t<genes.Length;t++)
            XtS[ss][t + DLTS[t][ss]] += genes[t];
        }
        public double citp(int x, int s, int c)
        {
            int xp = 0;
            double res = 0.0;
            if (x - 1 <= 0)
                return 0;

            //res = citp(xp,s, c) + XtS[s][x] * composants.ElementAt<Composant>(c).getRendement() - composants.ElementAt<Composant>(c).getDemandeAt(x)-imt(xp,s, c);


            double ccc = itp[c][s][x - 1] + GxTs(x, s) * composants.ElementAt<Composant>(c).getRendement();
            double ccc1 = composants.ElementAt<Composant>(c).getDemandeAt(x);
            double ccc2 = imt(x - 1, s, c);
            //res = itp[c][s][x-1] + XtS[s][x] * composants.ElementAt<Composant>(c).getRendement() - composants.ElementAt<Composant>(c).getDemandeAt(x) - imt(x-1, s, c);
            res = ccc - ccc1 - ccc2;
            if (res > 0)
            {
                itp[c][s][x] = res;
                return res;
            }
            itp[c][s][x] = 0;
            return 0;
        }
        public double[] getGenes()
        {
            return genes;
        }

        /* Getters and setters */
        // Use this if you want to create individuals with different gene lengths
        public static void setDefaultGeneLength(int length)
        {
            defaultGeneLength = length;
        }

        public double  getGene(int index)
        {
            return genes[index];
        }

        public void setGene(int index, double value)
        {
            genes[index] = value;
            competence = 0;
        }

        /* Public methods */
        public int size()
        {
            return genes.Length;
        }

        public double getCompetence()
        {
           return competence;
        }
        public void setCompetence(double x)
        {
            competence = x;
        }
        public void setCompetence(bool b)
        {
          competence=  Convert.ToDouble(coutSetup + coutDep + couSTK + coutRepture);
        }
        public void costSetup()
        {
            for (int j = 0; j < genes.Length ; j++)
            {
                if (genes[j] > 0)
                    coutSetup += p[j];
            }
        }
        
        public void depassCost()
        {
            for (int j = 0; j <genes.Length; j++)
                if (gg * genes[j] > temps[j])
                {
                    coutDep += (gg * genes[j] - temps[j]) * depp[j];
                }
        }
        public void coutStock()
        {
            for (int i = 0; i < composants.Count; i++)
                for (int j = 0; j < s; j++)
                    for (int k = 0; k < genes.Length ; k++)
                        couSTK += stock[i] * itp[i][j][k];
            couSTK /= s;

        }
        public void coutReptt()
        {
            for (int i = 0; i < composants.Count; i++)
                for (int j = 0; j < s; j++)
                    for (int k = 0; k <genes.Length ; k++)
                        coutRepture += repture[i] * itm[i][j][k];
            coutRepture /= s;

        }


        public String toString()
        {
            String geneString = "";
            for (int i = 0; i < size(); i++)
            {
                geneString += getGene(i)+" | ";
            }
            return geneString;
        }

        int IComparable<Individual>.CompareTo(Individual other)
        {
            
            return other.getCompetence().CompareTo(this.getCompetence());
        }
    }
}
