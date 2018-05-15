using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMITest
{
    class Graph
    {
        #region Public Member
        // Liste der Knoten
        public IList<Node> NodeList { get; set; }
        // Liste der Kanten
        public IList<Edge> EdgeList { get; set; }

        #endregion

        #region Konstruktor

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Graph()
        {
            NodeList = new List<Node>();
        }

        #endregion

        #region Einlesen

        /// <summary>
        ///  Adjazenzmatrix einlesen
        /// </summary>
        /// <param name="path">Path.</param>
		public IList<Node> ReadAdjazenzmatrix(String path)
        {
            List<string> list = new List<string>();
            string line = String.Empty;

            using (StreamReader reader = new StreamReader(path))
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }

            // Anzahl der Knoten
            int numberOfNodes = Convert.ToInt32(list[0]);

            // Knoten erstellen und in Knotenliste einfügen
            for (int i = 0; i < numberOfNodes; i++)
            {
                Node newNode = new Node(i);
                NodeList.Add(newNode);                
            }

            // Daten einlesen und verarbeiten
            for (int k = 1; k < list.Count; k++)
            {
                // Default Values
                double weight = 1.0;

                string[] elements = list[k].Split('\t');

                for (int j = 0; j < elements.Count(); j++)
                {
                    if (Convert.ToInt32(elements[j]) == 1)
                    {                                                
						NodeList[k - 1].Add(new Edge(NodeList[k - 1], NodeList[j], weight));
                    }
                }                
            }
            EdgeList = NodeList.SelectMany(node => node.Edges).ToList();
            return NodeList;
        }

        /// <summary>
        ///  Kantenliste einlesen
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="isDirected">isDirected.</param>
		public IList<Node> ReadKantenListe(String path, Boolean isDirected)
        {                       
            List<string> list = new List<string>();
            string line = String.Empty;

            using (StreamReader reader = new StreamReader(path))
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }

            // Anzahl der Knoten
            int numberOfNodes = Convert.ToInt32(list[0]);

            // Knoten erstellen und in Dictionary einfügen
            for (int i = 0; i < numberOfNodes; i++)
            {
                Node newNode = new Node(i);
				NodeList.Add(newNode);
            }

            // Daten einlesen und verarbeiten
            for (int i = 1; i < list.Count; i++)
            {
                // Default Values
                double weight = 1.0;
                int sourceID = -1;
                int targetID = -1;

                string[] elements = list[i].Split('\t');
                sourceID = Convert.ToInt32(elements[0]);
                targetID = Convert.ToInt32(elements[1]);
                if (elements.Count() == 2)
	            {
                    // Source- und Targetnode verbinden, da ungerichtet auch rückrichtung verbinden                                       
                    NodeList[sourceID].Add(new Edge(NodeList[sourceID], NodeList[targetID], weight));
                }
                else if (elements.Count() == 3)
	            {
                    // Source- und Targetnode verbinden
                    weight = Convert.ToDouble(elements[2].Replace(".", ","));                    
                    NodeList[sourceID].Add(new Edge(NodeList[sourceID], NodeList[targetID], weight));
                }
                if (!isDirected)
                {
                    NodeList[targetID].Add(new Edge(NodeList[targetID], NodeList[sourceID], weight));
                }

            }
            EdgeList = NodeList.SelectMany(node => node.Edges).ToList();
            return NodeList;
        }

        #endregion

        /// <summary>
        /// Kopiert eine EdgeList
        /// </summary>
        /// <param name="edgeList"></param>
        /// <returns></returns>
        public List<Edge> CopyEdgeList(List<Edge> edgeList)
        {
            List<Edge> newList = new List<Edge>();
            foreach (Edge e in edgeList)
            {
                newList.Add(e);
            }
            return newList;

        }
    }
}
