using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
		public IList<Node> ReadKantenListe(String path, Boolean isDirected, Boolean isCost)
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

			List<double> balances = new List<double> ();
			if (isCost)
			{				
				for (int i = 1; i < numberOfNodes + 1; i++)
				{
#if __MonoCS__
					balances.Add(Convert.ToDouble(list[i]));

#else
                    balances.Add(Convert.ToDouble(list[i].Replace(".", ",")));
#endif
                    
				}
			}




            // Knoten erstellen und in Dictionary einfügen
            for (int i = 0; i < numberOfNodes; i++)
            {
                Node newNode = new Node(i);
				NodeList.Add(newNode);
				if (isCost) 
				{
					newNode.Balance = balances[i];
				}
            }

            // Daten einlesen und verarbeiten
			int j = 1;
			if (isCost) 
			{
				j += numberOfNodes;
			}
			for (; j < list.Count; j++)
            {
                // Default Values
                double weight = 1.0;
				double cost = 0.0;
                int sourceID = -1;
                int targetID = -1;

                string[] elements = list[j].Split('\t');
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
#if __MonoCS__
            weight = Convert.ToDouble(elements[2]);

#else
                    weight = Convert.ToDouble(elements[2].Replace(".", ","));
#endif

                    NodeList[sourceID].Add(new Edge(NodeList[sourceID], NodeList[targetID], weight));
                }
				else if (elements.Count() == 4)
				{
					// Source- und Targetnode verbinden
					#if __MonoCS__
					weight = Convert.ToDouble(elements[2]);
					cost = Convert.ToDouble(elements[3]);

					#else
					weight = Convert.ToDouble(elements[2].Replace(".", ","));
					cost = Convert.ToDouble(elements[3].Replace(".", ","));
					#endif


					NodeList[sourceID].Add(new Edge(NodeList[sourceID], NodeList[targetID], 0.0, cost, weight));
				}
                if (!isDirected)
                {
					NodeList[targetID].Add(new Edge(NodeList[targetID], NodeList[sourceID], 0.0, cost, weight));
                }

            }
            EdgeList = NodeList.SelectMany(node => node.Edges).ToList();
            return NodeList;
        }

        #endregion

        /// <summary>
        /// Gibt die gesuchte Kante zurück
        /// </summary>
        /// <param name="edgeList"></param>
        public Edge getEdge(Node src, Node trg)
        {
			foreach (Node n in NodeList) 
			{
				foreach (Edge e in n.Edges)
				{
					if (e.SourceNode == src && e.TargetNode == trg) 
					{
						return e;
					}
				}
			}
			return null;
        }
			
        public IList<Edge> AllEdges
        {
            get
            {
                return NodeList.SelectMany(node => node.Edges).ToList();
            }
        }

		public void GetBalance(Node n)
		{			
			n.Balance = 0.0;
			foreach (Edge e in AllEdges)
			{
				if (e.SourceNode.ID == n.ID) 
				{
					n.Balance -= e.Flow;
				}
				else if (e.TargetNode.ID == n.ID) 
				{
					n.Balance += e.Flow;
				}
			}
		}


        public void EdgeListToString()
        {
            foreach (Edge e in AllEdges)
            {
                e.ToString();
            }
        }

    }
}
