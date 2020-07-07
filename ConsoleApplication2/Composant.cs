using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
   public class Composant
    {
        int numero;
        int stk_initial;
        int rendement;
        int[] demandes;
        public Composant()
        {

        }
        public void setNuméro(int n)
        {
            numero = n;
        }
        public void setSI(int s)
        {
            stk_initial = s;
        }
        public void setRendement(int r)
        {
            rendement = r;
        }
        public int getRendement()
        {
            return rendement;
        }
        public int getNum()
        {
            return numero;
        }
        public int getSI()
        {
            return stk_initial;
        }
        public int getDemandeAt(int i)
        {
            return demandes[i];
        }
        public int[]getDemandes()
        {
            return demandes;
        }
        public void setDemandes(int[] d)
        {
            demandes = new int[d.Length];
            demandes = d;
        }
    }
}
