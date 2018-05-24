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
            file = "/home/hanna/Desktop/K_10.txt";

#else
            file = @"C:\Users\Hanna\\MMI\K_10.txt";
#endif

            // Graph einlesen
            Graph newGraph = new Graph();
           	IList<Node> NodeList = newGraph.ReadKantenListe(file, false);
            IList<Edge> EdgeList = newGraph.EdgeList;

			Calc newCalculation = new Calc (NodeList, EdgeList);
			newCalculation.AlleTouren ();
			newCalculation.BranchAndBond ();
            int a = 0;

            
          }
    }
}
