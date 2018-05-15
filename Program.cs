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
            file = @"C:\Users\Hanna\\MMI\K_12e.txt";
#endif

            // Graph einlesen
            Graph newGraph = new Graph();
           	IList<Node> NodeList = newGraph.ReadKantenListe(file, false);
            IList<Edge> EdgeList = newGraph.EdgeList;
			//IList<Node> NodeList = newGraph.ReadAdjazenzmatrix(file);

			Calc newCalculation = new Calc (NodeList, EdgeList);

            // Tiefen- und Breitensuche
            // newCalculation.Breitensuche ();
            // int compCountBreitensuche = newCalculation.ComponentsList.Count();
            // newCalculation.Tiefensuche ();
            // int compCountTiefensuche = newCalculation.ComponentsList.Count(); 
            // Console.WriteLine ("Anzahl der Zusammenhangskomponenten (Tiefensuche):  {0} ", compCountTiefensuche); 
            // Console.WriteLine ("Anzahl der Zusammenhangskomponenten (Breitensuche):  {0} ", compCountBreitensuche);   

            // Prim & Kruskal
            // newCalculation.Prim(NodeList.First());
            // newCalculation.Kruskal();

            // Nächster Nachbar
            for (int i = 0; i < NodeList.Count(); i++)
            {
                newCalculation.NaechsterNachbar(NodeList[i]);
            }

            // Doppelter Baum        
            newCalculation.DoppelterBaum(NodeList.First());            


            int a = 0;

            
          }
    }
}
