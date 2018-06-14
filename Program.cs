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
            file = "/home/hanna/Desktop/Fluss.txt";

#else
            file = @"C:\Users\Hanna\\MMI\G_1_2.txt";
#endif

            // Graph einlesen
            Graph newGraph = new Graph();
            IList<Node> NodeList = newGraph.ReadKantenListe(file, true);
            IList<Edge> EdgeList = newGraph.EdgeList;

            Calc newCalculation = new Calc(NodeList, EdgeList);


            //Dictionary<Node, Tuple<double, int>> kwbMBF = new Dictionary<Node, Tuple<double, int>>();
            //if (newCalculation.MooreBellmanFord(NodeList[0], ref kwbMBF))
            //{
            //    Console.WriteLine("Der Graph {0} enthält keinen negativen Zykel.", file);
            //}
            //else
            //{
            //    Console.WriteLine("Der Graph {0} enthält einen negativen Zykel.", file);
            //}
            //Dictionary<Node, Tuple<double, int>> kwbD = newCalculation.Dijkstra(NodeList[0]);

			double test = newCalculation.EdmondsKarpMaxFluss(NodeList[0], NodeList[7]);

            int a = 0;
          }
    }
}
