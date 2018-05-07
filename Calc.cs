using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMITest;

namespace MMITest
{
	public class Calc
	{

		// Liste der Zusammenhangskomponenten
		public List<List<Node>> ComponentsList { get; set; }
		public IList<Node> NodeList{get; set;}

		public Calc (IList<Node> nodeList)
		{
			ComponentsList = new List<List<Node>> ();
			NodeList = nodeList;

		}

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
			foreach (Node n in NodeList)
			{
				if (!n.IsVisited)
				{							
					Console.WriteLine ("Berechne Komponente {0}", counter);
					ComponentsList.Add (new List<Node> ());
					startNode = n;
					Console.WriteLine ("StartNode: {0}", startNode.ID);
					Breitensuche (startNode, counter);
					counter++;
				}
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

        #region Prim
        /// <summary>
        /// Berechnet den Algorithmus von Prim
        /// </summary>
        public void Prim(Node startNode)
        {
            Reset();
            List<Edge> edgeList = NodeList.SelectMany(node => node.Edges).OrderBy(edge => edge.Weight).ToList();
            startNode.IsVisited = true;
            double primWeight = 0.0;

            // solang nicht alle Knoten besucht sind
            while (NodeList.Where(n => n.IsVisited == false).Any())
            {
                // Suche nächsten erreichbaren Knoten mit minimalem Gewicht
                Edge e = edgeList.Where(edge => edge.SourceNode.IsVisited && !edge.TargetNode.IsVisited).First();
                e.TargetNode.IsVisited = true;
                primWeight += e.Weight;
                //Console.WriteLine("Ausgabe: {0}    {1}     {2}", e.SourceNode.ID.ToString(), e.TargetNode.ID, e.Weight);
                edgeList.Remove(e);
            }
            Console.WriteLine("Gesamtkosten Prim: {0}", primWeight);
        }
        #endregion

        #region Kruskal

        /// <summary>
        /// Berechnet den Algorithmus von Kruskal
        /// </summary>
        public void Kruskal()
        {
            Reset();

            double mspWeight = 0.0;
            List<Edge> sortedEdges = NodeList.SelectMany(node => node.Edges).OrderBy(edge => edge.Weight).ToList();
            
            foreach (Node n in NodeList)
            {
                n.ComponentCount = n.ID;
            }

            for (int i = 0; i < NodeList.Count() - 1; i++)
            {
                Edge e = sortedEdges.Where(edge => ((edge.TargetNode.ComponentCount != edge.SourceNode.ComponentCount))).FirstOrDefault();
                mspWeight += e.Weight;
                //Console.WriteLine("Ausgabe: {0}    {1}     {2}", e.SourceNode.ID.ToString(), e.TargetNode.ID, e.Weight);
                List<Node> toDo = NodeList.Where(node => node.ComponentCount == Math.Max(e.SourceNode.ComponentCount, e.TargetNode.ComponentCount)).ToList();

                foreach (Node n in toDo)
                {
                    n.ComponentCount = Math.Min(e.SourceNode.ComponentCount, e.TargetNode.ComponentCount);
                }
                sortedEdges.Remove(e);
            }
            Console.WriteLine("Gesamtkosten Kruskal: {0}", mspWeight);
        }
        #endregion
    }
}