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
        public List<List<Node>> _ComponentsList { get; set; }
        // Liste der Zusammenhangskomponenten
        // public Dictionary<int, int> _Components { get; set; }
        // Liste der Knoten
        public IList<Node> _NodeList { get; set; }

        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        public Graph()
        {
            _NodeList = new List<Node>();
            _ComponentsList = new List<List<Node>>();
            // _Components = new Dictionary<int, int> ();
        }

        #endregion

        #region Private Member

        private void Reset()
        {
            _ComponentsList = new List<List<Node>>();
            foreach (Node node in _NodeList)
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
            Tiefensuche(_NodeList.First());
        }


        /// <summary>
        /// Tiefensuche
        /// </summary>
        /// <param name="node">Node.</param>
        public void Tiefensuche(Node node)
        {
            // Knoten initialisiert mit 0, also muss der counter  bei 1 starten
            int counter = 1;
            // Tiefensuche (node, counter);
            foreach (Node n in _NodeList)
            {
                if (n.Zugehörigkeitskomponente == -1)
                {
                    Console.WriteLine("Berechne Komponente {0}", counter);
                    Console.WriteLine("Start Node {0}", n.ID);
                    _ComponentsList.Add(new List<Node>());
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
            node.Zugehörigkeitskomponente = counter;
            _ComponentsList[counter - 1].Add(node);
            Console.WriteLine("Current Node: {0}", node.ID);
            foreach (Edge e in node.Edges)
            {
                Node nextVertice = e.TargetNode;
                if (nextVertice.Zugehörigkeitskomponente == -1)
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
            Breitensuche(_NodeList.FirstOrDefault());
        }
        /// <summary>
        /// Breitensuche
        /// </summary>
        public void Breitensuche(Node startNode)
        {
            Console.WriteLine("Starte Breitensuche");
            int counter = 1;

            Reset();            
            while (_NodeList.Where(n => n.IsVisited == false).Count() > 0)
            {
                Console.WriteLine("Berechne Komponente {0}", counter);
                _ComponentsList.Add(new List<Node>());
                startNode = _NodeList.Where(n => n.IsVisited == false).FirstOrDefault();
                Console.WriteLine("StartNode: {0}", startNode.ID);
                Breitensuche(startNode, counter);
                counter++;
            }
        }


        /// <summary>
        /// Breitensuche
        /// </summary>
        /// <param name="Nodes">Nodes.</param>
        private void Breitensuche(Node startNode, int counter)
        {
            Queue<Node> Q = new Queue<Node>();
            Q.Enqueue(startNode);
            startNode.IsVisited = true;
            while (Q.Any())
            {
                Node currentNode = Q.Dequeue();            
                Console.WriteLine("Current Node: {0}", currentNode.ID);
                foreach (var edge in currentNode.Edges)
                {
                    if (!edge.TargetNode.IsVisited)
                    {
                        Q.Enqueue(edge.TargetNode);
                        edge.TargetNode.IsVisited = true;
                        _ComponentsList[counter - 1].Add(edge.TargetNode);
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
        public void AdjazenzmatrixEinlesen(String path)
        {
            Dictionary<int, Node> NodeLookUp = new Dictionary<int, Node>();
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
                _NodeList.Add(newNode);
                NodeLookUp[newNode.ID] = newNode;
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
                        NodeLookUp[k - 1].Add(new Edge(NodeLookUp[k - 1], NodeLookUp[j], weight));
                    }
                }                
            }
        }

        /// <summary>
        ///  Kantenliste einlesen
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="isDirected">isDirected.</param>
        public void KantenListeEinlesen(String path, Boolean isDirected)
        {                       
            Dictionary<int, Node> NodeLookUp = new Dictionary<int, Node>();
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
				_NodeList.Add(newNode);
                NodeLookUp[newNode.ID] = newNode;
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
                NodeLookUp[sourceID].Add(new Edge(NodeLookUp[sourceID], NodeLookUp[targetID], weight));
				// Rückrichtung nur einfügen, wenn Graph nicht gerichtet ist
                if (!isDirected)
                {
                    NodeLookUp[targetID].Add(new Edge(NodeLookUp[targetID], NodeLookUp[sourceID], weight));
                }
            }
        }

        #endregion
    }
}
