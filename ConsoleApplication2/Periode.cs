using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public class Periode
    {
        int max;
        int[] periodes;
        public Periode(int m)
        {
            periodes = new int[m];
            max = m;
            for(int i=0;i<m;i++)
            {
                periodes[i] = i+1;
            }
        }
        public int getMax()
        {
            return max;
        }
    }
}
