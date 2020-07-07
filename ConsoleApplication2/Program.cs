using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public class Program
    {
        
        int[] demandes;

        static void Main(string[] args)
        {
            Periode p;
            string fichier=System.IO.File.ReadAllText("D:\\222.txt");
            int periode = Convert.ToInt16(fichier[0].ToString());            
            p = new Periode(periode);
            Console.WriteLine(p.getMax());
            Console.ReadKey();

        }
    }
}
