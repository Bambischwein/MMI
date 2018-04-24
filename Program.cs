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
            file = "/home/hanna/Desktop/Graph2.txt";

#else
            file = @"C:\Users\Hanna\\MMI\Graph2.txt";
#endif
            Graph newGraph = new Graph();
			newGraph.KantenListeEinlesen(file, false);

			newGraph.Tiefensuche ();
			int maxTiefensuche = newGraph._Components.Max(n => n.Value);

			List<List<int>> test = new List<List<int>> ();
			test.Add (new List<int>(1));
			test.Add (new List<int>(2));
			test [0].Add (1);
			int a = test.Count ();
			test.Add (new List<int> ());



			// newGraph.Breitensuche ();
			//int maxBreitensuche = newGraph._Components.Max(n => n.Value);
			Console.WriteLine ("Anzahl der Zusammenhangskomponenten (Tiefensuche):  {0} ", maxTiefensuche); 
			// Console.WriteLine ("Ergebnis Breitensuche:  {0} ", maxBreitensuche); 
        }
    }
}
