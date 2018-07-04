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

        public static void Main()
        {
            String file = String.Empty;

            // BS unterscheidung
#if __MonoCS__
			file = "/home/hanna/Desktop/KM3.txt";

#else
            file = @"C:\Users\Hanna\MMI\KM3.txt";z
#endif

            // Graph einlese1
            Graph newGraph = new Graph();
			// IList<Node> NodeList = newGraph.ReadKantenListe(file, true, true);
            // IList<Edge> EdgeList = newGraph.EdgeList;
			// matchinggraph einlesen
			List<Node> NodeListA = new List<Node>();
			List<Node> NodeListB = new List<Node>();
			newGraph.readMatchingGraph (ref NodeListA, ref NodeListB, file);		       


			// Calc c = new Calc (NodeList, EdgeList);
			// c.CC (newGraph);

			Calc newCalc = new Calc (newGraph.NodeList);
			newCalc.MaxMatchings (NodeListA, NodeListB, newGraph);

            int a = 0;
          }
    }
}
