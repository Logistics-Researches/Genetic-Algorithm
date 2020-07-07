using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication2
{
    public class CIteration : Iteration
    {
        //this object
        int numero;
       public double [] zt;
        double[][] XtS;
        public List<int> K;
        double[][][] itp;
        double[][][] itm;
        int[][] xtp;
        int x;
        // imported
        int[][] DLTS;
        List<Composant> composants;
        int periode;
        int nbr_comp;
        int s;
        public double couSTK;
        public double coutRepture;
        public int coutSetup;
        public double coutDep;
        int[] p;
        double[] stock;
        int[] temps;
        int gg;
        int[] repture;
        double[] depp;
        public CIteration(int T, int num, List<Composant> cc, int[][] dl, int scn, int[] sc, int g, int[] tc, double[] stk, int[] rept, double[] dep)
        {
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
            periode = T;
            DLTS = dl;
            composants = cc;
             nbr_comp = cc.Count;
            K = new List<int>();
            prep_XtS();
            prep_ipmt();

            zt = new double[T];
           

        }
        public void generationK()
        {            
            x = 1;
            K.Add(x);
            while (x < periode)
            {
            deb: x = new Random().Next(x, periode);
                if (x == periode)
                    break;
                if (x == 1)
                    goto deb;

                K.Add(x);
                x++;
            }
            K.Add(periode);
         }
        public double[] generationZt()
        {
            //generationK();
           /* K = new List<int>();
            K.Add(2);
            K.Add(4);*/
         
            double[] somme;
            int[] kp = new int[periode];
            for(int i=0;i<periode; i++)
                kp[i] = i;
           int ii = 0; int jj = 0;
            int deb = 0;
            int kkk = 0;
            int aux = 0;
            somme = new double[periode ];

            while (ii < kp.Length && jj<K.Count )
            {
                
                deb = ii;

                
                somme[ii]= zji2(deb,K[jj]);
                
                zt[deb] = somme[ii];
                for (int i = deb; i <K[jj]; i++)
                    for (int j = 0; j < s; j++)
                        CxTs(i, j,deb);
                for(int i=0;i<composants.Count;i++)
                    for(int j=0;j< s;j++)
                        for(int k= deb;k<K[jj];k++)
                        {
                            itm[i][j][k] = imt(k, j, i);
                            itp[i][j][k] = citp(k, j, i);
                        }




                    ii = K[jj]+1;


                
                
                jj++;
            }
            costSetup();
            depassCost();
            coutStock();
            coutReptt();

            string yy = "";
            Console.WriteLine("-------XTS");
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < periode; j++)


                    Console.Write("s" + i + ",t" + j + " " + XtS[i][j] + " |");
                Console.WriteLine();
            }
            Console.WriteLine("-------Zt");
            for (int i = 0; i < zt.Length; i++)
                Console.Write(zt[i] + " | ");
            Console.WriteLine();

            Console.WriteLine("-------ITP");
            for (int i = 0; i < composants.Count; i++)
                for (int j = 0; j < s; j++)
                {
                    for (int k = 0; k < periode; k++)
                        Console.Write("c" + i + ",s" + j + ",t" + k + " " + itp[i][j][k] + " |");
                    Console.WriteLine();
                }
            Console.WriteLine("-------ITM");
            for (int i = 0; i < composants.Count; i++)
                for (int j = 0; j < s; j++)
                {
                    for (int k = 0; k < periode; k++)
                        Console.Write(itm[i][j][k] + " |");
                    Console.WriteLine();
                }
            return zt; 
        }
        public void prep_XtS()
        {

            
            XtS = new double[s][];
            xtp = new int[s][];
            for (int i = 0; i < s; i++)
            {
                XtS[i] = new double[periode];
                xtp[i] = new int[periode];
            }
        }
        public void CxTs(int t, int ss,int deb)
        {

            /*for (int i = 0; i < s; i++)
                XtS[i][0] = 0;*/
           
                
                        XtS[ss][t + DLTS[t][ss]] += zt[t];
        }
        public double GxTs(int t, int ss)
        {  
            return XtS[ss][t];
        }
        public double citp(int x, int s, int c)
        {
            int xp = 0;
            double res = 0.0;
            if (x - 1 <= 0)
                return 0;

            //res = citp(xp,s, c) + XtS[s][x] * composants.ElementAt<Composant>(c).getRendement() - composants.ElementAt<Composant>(c).getDemandeAt(x)-imt(xp,s, c);
         
            
            double ccc = itp[c][s][x - 1] +GxTs(x, s) * composants.ElementAt<Composant>(c).getRendement();
            double ccc1 = composants.ElementAt<Composant>(c).getDemandeAt(x);
            double ccc2 =imt(x-1,s, c);
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
            res = itm[c][s][x - 1] + composants.ElementAt<Composant>(c).getDemandeAt(x) -GxTs(x, s) * composants.ElementAt<Composant>(c).getRendement() -citp(x-1,s, c);
            if (res > 0)
            {
                itm[c][s][x] = res;
                return res;
            }
            itm[c][s][x] = 0;
            return 0;
        }
        public void prep_ipmt()
        {
            itp = new double[nbr_comp][][];
            itm = new double[nbr_comp][][];
            foreach (Composant c in composants)
            {
                itp[c.getNum()] = new double[s][];
                for (int i = 0; i < s; i++)
                    itp[c.getNum()][i] = new double[periode];
            }
            foreach (Composant c in composants)
            {
                itm[c.getNum()] = new double[s][];
                for (int i = 0; i < s; i++)
                    itm[c.getNum()][i] = new double[periode];
            }
        }
       
        public double zji2(int x, int d)

        {


            double somme = 0;
            double[][] prov = new double[composants.Count][];
            for (int i = 0; i < composants.Count; i++)
                prov[i] = new double[s];
            double ll = 0;
            int fi = 0;
            int fj = 0;
            int wdh = 0;
            for (int i2 = 0; i2 < composants.Count; i2++)
            {
                
                somme = 0;
                if (x == d)
                {
                    wdh = d;
                }
                else
                {
                    wdh = d ;
                }
                if (d < x)
                    wdh = x;
                string h = "";
                for (int k = x; k <= wdh; k++)
                {
                    somme += composants.ElementAt<Composant>(i2).getDemandeAt(k);
                }
                for (int j2=0; j2<s; j2++)
                {
                    
                        ll = somme;
                    

                    ll -= citp(x - 1, j2, i2);
                   
                    ll += imt(x - 1, j2, i2);
                   
                    ll /= composants.ElementAt<Composant>(i2).getRendement();
                    prov[i2][j2] = ll;

                    fj++;
                  
                }
                
            }
            double max = 0;
            for (int i = 0; i < composants.Count; i++)
                if (prov[i].Max() > max)
                    max = prov[i].Max();

            return max;

        }


        public void costSetup()
        {
            for (int j = 0; j < periode; j++)
            {
                if (zt[j] > 0)
                    coutSetup += p[j];
            }
        }
        public void depassCost()
        {
            for (int j = 0; j < zt.Length; j++)
                if (gg * zt[j] > temps[j])
                {
                    coutDep += (gg * zt[j] - temps[j]) * depp[j];
                }
        }
        public void coutStock()
        {
            for (int i = 0; i < composants.Count; i++)
                for (int j = 0; j < s; j++)
                    for (int k = 0; k < periode; k++)
                        couSTK += stock[i] * itp[i][j][k];
            couSTK /= s;

        }
        public void coutReptt()
        {
            for (int i = 0; i < composants.Count; i++)
                for (int j = 0; j < s; j++)
                    for (int k = 0; k < periode; k++)
                        coutRepture += repture[i] * itm[i][j][k];
            coutRepture /= s;

        }






    }
}
