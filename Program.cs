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

            // BS unterscheidung
#if __MonoCS__
            file = "/home/hanna/Desktop/Graph1.txt";

#else
            file = @"C:\Users\Hanna\\MMI\Graph1.txt";
#endif

            // Graph einlesen
            Graph newGraph = new Graph();

           	// newGraph.ReadKantenListe(file, false);
            newGraph.ReadAdjazenzmatrix(file);

             // Tiefen- und Breitensuche
			newGraph.Tiefensuche ();
            int compCountTiefensuche = newGraph.ComponentsList.Count();  
            
            newGraph.Breitensuche ();
            int compCountBreitensuche = newGraph.ComponentsList.Count();

            Console.WriteLine ("Anzahl der Zusammenhangskomponenten (Tiefensuche):  {0} ", compCountTiefensuche); 
            Console.WriteLine ("Anzahl der Zusammenhangskomponenten (Breitensuche):  {0} ", compCountBreitensuche);  
          }
    }
}
