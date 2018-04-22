using System;
using System.Collections.Generic;
using System.Linq;

namespace MMITest
{
	public class Searching
	{
        public List<List<Node>> _Components { get; set; }

        public int Tiefensuche(Node node, int counter)
        {
            node.IsVisited = true;
            node.ComponentCounter = counter;
            _Components[counter - 1].Add(node);

            foreach (Edge e in node.Edges)
            {
                Node nextVertice = e.TargetNode;
                if (nextVertice.ComponentCounter == 0)
                {
                    Tiefensuche(nextVertice, counter);
                }
            }
            return counter;
        }
    }
}

