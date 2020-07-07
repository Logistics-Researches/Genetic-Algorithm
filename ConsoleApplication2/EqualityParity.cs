using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class EqualityParity : IEqualityComparer<List<int>>
    {
        public bool Equals(List<int> x, List<int> y)
        {
            return (x.GetHashCode() == y.GetHashCode());
        }

        public int GetHashCode(List<int> obj)
        {
            int hc = obj.Count ;
            for(int i=0;i<obj.Count;i++)
              hc = unchecked(hc * 3141 +obj[i]);

            return hc;

        }
    }
}
