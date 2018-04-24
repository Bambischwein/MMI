using System;
using MMITest;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace MMITest
{
	class MainClass
	{

        public static void Main ()
		{            
            String file = String.Empty;
#if __MonoCS__
            file = "/home/hanna/Desktop/Graph4.txt";

#else
            file = @"C:\Users\Hanna\\MMI\Graph2.txt";
#endif
            Graph newGraph = new Graph();
			newGraph.KantenListeEinlesen(file, false);

			newGraph.Tiefensuche ();
			int maxTiefensuche = newGraph._Test.Max(n => n.Value);
			newGraph.Breitensuche ();
			int maxBreitensuche = newGraph._Test.Max(n => n.Value);
			Console.WriteLine ("Ergebnis Tiefensuche:  {0} ", maxTiefensuche); 
			Console.WriteLine ("Ergebnis Breitensuche:  {0} ", maxBreitensuche); 

            int b = 0;
        }
    }
}
