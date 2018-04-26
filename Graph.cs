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

        // Liste der Zusammenhangskomponenten
        public List<List<Node>> ComponentsList { get; set; }
        // Liste der Knoten
        public IList<Node> NodeList { get; set; }

        #endregion

        #region Konstruktor

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Graph()
        {
            NodeList = new List<Node>();
            ComponentsList = new List<List<Node>>();
        }

        #endregion

        #region Private Member

        private void Reset()
        {
            ComponentsList = new List<List<Node>>();
            foreach (Node node in NodeList)
            {
                node.IsVisited = false;
                foreach (Edge e in node.Edges)
                {
                    e.Visited = false;
                }
            }

        }

        #endregion

        #region Tiefensuche

        /// <summary>
        /// Tiefensuche
        /// </summary>
        public void Tiefensuche()
        {
            Console.WriteLine("Starte Tiefensuche");
            Reset();
            Tiefensuche(NodeList.First());
			Console.WriteLine("");
        }


        /// <summary>
        /// Tiefensuche
        /// </summary>
        /// <param name="node">Node.</param>
        public void Tiefensuche(Node node)
        {
            // Knoten initialisiert mit -1, also muss der counter  bei 1 starten
            int counter = 1;
            // Tiefensuche (node, counter);
            foreach (Node n in NodeList)
            {
                if (n.ComponentCount == -1)
                {
                    Console.WriteLine("Berechne Komponente {0}", counter);
                    Console.WriteLine("Start Node {0}", n.ID);
                    ComponentsList.Add(new List<Node>());
                    Tiefensuche(n, counter);
                    counter++;
                    
                }
            }
        }

        /// <summary>
        /// Tiefensuche
        /// </summary>
        /// <param name="node">Node.</param>
        /// <param name="counter">Counter.</param>
        private void Tiefensuche(Node node, int counter)
        {
            node.IsVisited = true;
            node.ComponentCount = counter;
            ComponentsList[counter - 1].Add(node);
            Console.WriteLine("Current Node: {0}", node.ID);
            foreach (Edge e in node.Edges)
            {
                Node nextVertice = e.TargetNode;
                if (nextVertice.ComponentCount == -1)
                {
                    Tiefensuche(nextVertice, counter);
                }
            }
        }

        #endregion

        #region Breitensuche

        /// <summary>
        /// Breitensuche
        /// </summary>
        public void Breitensuche()
        {
            Breitensuche(NodeList.FirstOrDefault());

        }
        /// <summary>
        /// Breitensuche
        /// </summary>
		/// <param name="startNode">startNode</param>
        public void Breitensuche(Node startNode)
        {
            Console.WriteLine("Starte Breitensuche");
            int counter = 1;

            Reset();            
            while (NodeList.Where(n => n.IsVisited == false).Count() > 0)
            {
                Console.WriteLine("Berechne Komponente {0}", counter);
                ComponentsList.Add(new List<Node>());
                startNode = NodeList.Where(n => n.IsVisited == false).FirstOrDefault();
                Console.WriteLine("StartNode: {0}", startNode.ID);
                Breitensuche(startNode, counter);
                counter++;
            }
        }


        /// <summary>
        /// Breitensuche
        /// </summary>
        /// <param name="startNodes">startNode.</param>
		/// <param name="counter">Counter for components</param>
        private void Breitensuche(Node startNode, int counter)
        {
            Queue<Node> Q = new Queue<Node>();
            Q.Enqueue(startNode);
            startNode.IsVisited = true;
            while (Q.Any())
            {
				// Knoten aus der Queue nehmen
                Node currentNode = Q.Dequeue();            
                Console.WriteLine("Current Node: {0}", currentNode.ID);
				// Jeden Nachbarn, der noch nicht besucht wurde, in die 
				// Queue einfügen und als besucht markieren
                foreach (var edge in currentNode.Edges)
                {
                    if (!edge.TargetNode.IsVisited)
                    {
                        Q.Enqueue(edge.TargetNode);
                        edge.TargetNode.IsVisited = true;
                        ComponentsList[counter - 1].Add(edge.TargetNode);
                    }
                }

            }

        }

        #endregion

        #region Einlesen

        /// <summary>
        ///  Adjazenzmatrix einlesen
        /// </summary>
        /// <param name="path">Path.</param>
        public void ReadAdjazenzmatrix(String path)
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
        }

        /// <summary>
        ///  Kantenliste einlesen
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="isDirected">isDirected.</param>
        public void ReadKantenListe(String path, Boolean isDirected)
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

                // Source- und Targetnode verbinden
				NodeList[sourceID].Add(new Edge(NodeList[sourceID], NodeList[targetID], weight));
				// Rückrichtung nur einfügen, wenn Graph nicht gerichtet ist
                if (!isDirected)
                {
					NodeList[targetID].Add(new Edge(NodeList[targetID], NodeList[sourceID], weight));             
                }
            }
        }

        #endregion
    }
}
