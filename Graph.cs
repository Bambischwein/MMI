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

        // public List<List<Node>> _Components { get; set; }
		public Dictionary<int, int> _Components { get; set; }
		IList<Node> _NodeList { get; set; }

		#endregion

		/// <summary>
		/// Konstruktor
		/// </summary>
		public Graph()
		{
			_NodeList = new List<Node>();
			// _Components = new List<List<Node>> ();
			_Components = new Dictionary<int, int> ();
		}

		/// <summary>
		/// Tiefensuche
		/// </summary>
		public void Tiefensuche()
		{ 
			_Test = new Dictionary<int, int> ();
			Tiefensuche (_NodeList.First());
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
				if (n.BelongsToComponent == -1)
				{
					// Console.WriteLine("Found connected component #" + BelongsToComponent);
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
			node.BelongsToComponent = counter;
			//_Components[counter - 1].Add(node);
			_Components.Add(node.ID, counter);
			foreach (Edge e in node.Edges)
			{
				Node nextVertice = e.TargetNode;
				if (nextVertice.BelongsToComponent == -1)
				{
					Tiefensuche(nextVertice, counter);
				}
			}
		}

		/// <summary>
		/// Breitensuche
		/// </summary>
		public void Breitensuche()
		{
			_Components = new Dictionary<int, int> ();
			Breitensuche (_NodeList.FirstOrDefault ());
		}

		/// <summary>
		/// Breitensuche
		/// </summary>
		/// <param name="Nodes">Nodes.</param>
		public void Breitensuche(Node node)
		{
			Queue<Node> Q = new Queue<Node> ();
			Node lastNode = new Node ();
			node.IsVisited = true;
			Q.Enqueue (node);
			Breitensuche(Q, lastNode);

		}

		/// <summary>
		/// Breitensuche
		/// </summary>
		/// <param name="q">Q.</param>
		/// <param name="lastNode">Last node.</param>
		private void Breitensuche(Queue<Node> q, Node lastNode)
		{
			while (q.Any())
			{
				// Nächsten Knoten aus der Queue nehmen
				Node currentNode = q.Dequeue ();
				foreach (Edge e in currentNode.Edges)
				{
					if (!e.TargetNode.IsVisited) {
						e.Visited = true;
						e.TargetNode.IsVisited = true;

						// TODO: falls ungerichtet andere kannte auch markieren
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
				// rückrichtung nur einfügen, wenn Graph nicht gerichtet ist
                if (!isDirected)
                {
                    NodeLookUp[targetID].Add(new Edge(NodeLookUp[targetID], NodeLookUp[sourceID], weight));
                }
            }
        }
    }
}
