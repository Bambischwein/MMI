using System;
using System.Collections.Generic;
using System.Linq;

namespace MMITest
{
	public class Searching
	{
        public List<List<Node>> _Components { get; set; }

		public int Tiefensuche(IList<Node> nodeList)
		{ 
			// nodes initialisiert mit 0, also muss der counter  bei 1 starten
			int counter = 1;
			Node startNode = nodeList.First ();	
			Tiefensuche (startNode, counter);

			return counter;
		}


		public int Tiefensuche(Node node, int counter)
        {			
			node.IsVisited = true;
			node.BelongsToComponent = counter;
            // _Components[counter - 1].Add(startNode);

            foreach (Edge e in node.Edges)
            {
                Node nextVertice = e.TargetNode;
                if (nextVertice.BelongsToComponent == -1)
                {
                    Tiefensuche(nextVertice, counter);
                }
            }
            return counter;
        }
    }
}

