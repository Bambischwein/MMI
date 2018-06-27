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
            file = "/home/hanna/Desktop/KM1.txt";

#else
            file = @"C:\Users\Hanna\\MMI\KM1txt";
#endif

            // Graph einlese1
            Graph newGraph = new Graph();
			IList<Node> NodeList = newGraph.ReadKantenListe(file, true, true);
            IList<Edge> EdgeList = newGraph.EdgeList;

            Calc newCalculation = new Calc(NodeList, EdgeList);

			// Console.WriteLine("Minfluss: {0}", newCalculation.SSP());
			newCalculation.CC();

            int a = 0;
          }
    }
}
