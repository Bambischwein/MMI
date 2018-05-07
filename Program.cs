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
            file = "/home/hanna/Desktop/Graph2.txt";

#else
            file = @"C:\Users\Hanna\\MMI\G_1_2.txt";
#endif

            // Graph einlesen
            Graph newGraph = new Graph();
           	IList<Node> NodeList = newGraph.ReadKantenListe(file, false);
			//IList<Node> NodeList = newGraph.ReadAdjazenzmatrix(file);

			Calc newCalculation = new Calc (NodeList);

			// Tiefen- und Breitensuche
			// newCalculation.Breitensuche ();
			// int compCountBreitensuche = newCalculation.ComponentsList.Count();
			// newCalculation.Tiefensuche ();
			// int compCountTiefensuche = newCalculation.ComponentsList.Count(); 
			// onsole.WriteLine ("Anzahl der Zusammenhangskomponenten (Tiefensuche):  {0} ", compCountTiefensuche); 
			// Console.WriteLine ("Anzahl der Zusammenhangskomponenten (Breitensuche):  {0} ", compCountBreitensuche);  

			newCalculation.Prim (NodeList.First());
            newCalculation.Kruskal();
            int a = 0;

            
          }
    }
}
