using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMITest
{
    class Graphenbibiliothek
    {
        public List<List<Node>> _Components { get; set; }

        public IList<Node> KantenListeEinlesen(String path, Boolean isDirected)
        {
            // List of Nodes
            IList<Node> Nodes = new List<Node>();
            // Dictionary for fast data lookup
            Dictionary<int, Node> NodeLookUp = new Dictionary<int, Node>();

            List<string> list = new List<string>();
            string line = String.Empty;

            using (StreamReader reader = new StreamReader(path))
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }

            // Get number of nodes
            int numberOfNodes = Convert.ToInt32(list[0]);

            //create all nodes and store them in dictionary
            for (int i = 0; i < numberOfNodes; i++)
            {
                Node newNode = new Node(i);
                Nodes.Add(newNode);
                NodeLookUp[newNode.ID] = newNode;
            }

            //read all data
            for (int i = 1; i < list.Count; i++)
            {
                //default values
                double weight = 1.0;
                int sourceID = -1;
                int targetID = -1;

                string[] elements = list[i].Split('\t');

                sourceID = Convert.ToInt32(elements[0]);
                targetID = Convert.ToInt32(elements[1]);

                //connect Source and Targetnode
                NodeLookUp[sourceID].Add(new Edge(NodeLookUp[sourceID], NodeLookUp[targetID], weight));
                if (!isDirected)
                {
                    NodeLookUp[targetID].Add(new Edge(NodeLookUp[targetID], NodeLookUp[sourceID], weight));
                }
            }
            return Nodes;
        }
    }
}
