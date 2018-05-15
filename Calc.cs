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
		public IList<Node> NodeList{ get; set;}
        public IList<Edge> EdgeList { get; set; }

		public Calc (IList<Node> nodeList, IList<Edge> edgeList)
		{
			ComponentsList = new List<List<Node>> ();
			NodeList = nodeList;
            EdgeList = edgeList;
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
                edgeList.Remove(e);
            }
        }
        /// <summary>
        /// Berechnet den Algorithmus von Prim
        /// </summary>
        private List<Node> PrimReturn(Node startNode)
        {
            Reset();

            List<Node> primNodeList = new List<Node>();
            for (int i = 0; i < NodeList.Count(); i++)
            {
                primNodeList.Add(new Node(i));
            }
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
                primNodeList[e.SourceNode.ID].Edges.Add(e);
                edgeList.Remove(e);
            }
            return primNodeList;
            
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
                List<Node> completeComponentCount = NodeList.Where(node => node.ComponentCount == Math.Max(e.SourceNode.ComponentCount, e.TargetNode.ComponentCount)).ToList();
                foreach (Node n in completeComponentCount)
                {
                    n.ComponentCount = Math.Min(e.SourceNode.ComponentCount, e.TargetNode.ComponentCount);
                }
                sortedEdges.Remove(e);
            }
            Console.WriteLine("Gesamtkosten Kruskal: {0}", mspWeight);
        }

        #endregion

        #region Nächster Nachbar


        public Dictionary<Node, Node> NaechsterNachbar(Node startKnoten)
        {
            Reset();
            // Schritt 1: : Wähle einen beliebigen Knoten als Startknoten v
            // Node startKnoten = NodeList.First();
            startKnoten.IsVisited = true;
            List<Edge> listOfMinEdge = new List<Edge>(); // NodeList.SelectMany(node => node.Edges).OrderBy(edge => edge.Weight).ToList();
            double weight = 0.0;
            Dictionary<Node, Node> hamiltonKreis = new Dictionary<Node, Node>();
            // Damit Startknoten nicht überschrieben wird
            Node nextNode = startKnoten;
            while (NodeList.Where(n => n.IsVisited == false).Any())
            {
                // Schritt 2: : Ermittle die niedrigste Kante, welche den aktuellen Knoten v mit einem
                // unbesuchten Knoten vu verbindet.
                Edge minEdge = nextNode.Edges.ToList().OrderBy(e => e.Weight).Where(e => e.TargetNode.IsVisited == false).First();
                weight += minEdge.Weight;
                // Schritt 3: : Setze v = vu                
                hamiltonKreis.Add(nextNode, minEdge.TargetNode);
                nextNode = minEdge.TargetNode;
                nextNode.IsVisited = true;
                // Schritt 4: : Wenn noch nicht alle Knoten besucht wurden gehe wieder zu Schritt 2.
            }
            // Schritt 5: : Füge die Kante vom letzten besuchten Knoten zum Startknoten hinzu um
            // den Kreis zu schließen.
            hamiltonKreis.Add(nextNode, startKnoten);
            weight += nextNode.Edges.Where(node => ((node.TargetNode == startKnoten))).First().Weight;
            // Console.WriteLine("Startknoten: {0}, Distanz: {1}", startKnoten.ID, weight);
            return hamiltonKreis;
        }
        #endregion

        #region Doppelter Baum

        public void DoppelterBaum(Node startKnoten)
        {
            Reset();
            double weight = 0.0;

            // Schritt 1: : Konstruiere einen minimal spannenden Baum T von Kn.
            List<Node> primMinSpannbaum = PrimReturn(startKnoten);

            // Schritt 2: : Verdopple alle Kanten von T(daraus resultiert ein eulerscher Graph Td).           
            List<Edge> edgeList = primMinSpannbaum.SelectMany(node => node.Edges).ToList();
            int tmp = edgeList.Count();
            for (int i = 0; i < tmp; i++)
            {
                edgeList.Add(new Edge(edgeList[i].TargetNode, edgeList[i].SourceNode));
            }

            // Schritt 3: : Berechne eine Euler - Tour in Td. Wähle Knoten v0 und konstruiere von v0 ausgehend einen Unterkreis K in G, der keine Kante in G zweimal durchläuft
            Node startNode = edgeList[0].SourceNode;
            List<Node> euler = new List<Node>();
            euler.Add(startNode);
            while (edgeList.Where(n => n.Visited == false).Any())
            {                
                Edge e = edgeList.Where(n => n.SourceNode == startNode && n.Visited == false).First();
                startNode = e.TargetNode;            
                startNode.IsVisited = true;
                e.Visited = true;
                euler.Add(startNode);        
            }

            // Schritt 4: : Durchlaufe die Euler - Tour von einem Startknoten aus. Falls dabei ein Knoten schon besucht wurde, nehme die Abkürzung zum nächsten unbesuchten Knoten auf der Tour.
            Reset();
            List<Node> durchlauf = new List<Node>();
            startNode = euler.First();
            Node nextNode = startNode;
            while (durchlauf.Count() < NodeList.Count())
            {
                startNode.IsVisited = true;
                euler.Remove(startNode);
                durchlauf.Add(new Node(startNode.ID));
                if (NodeList.Where(n => n.IsVisited == false).Count() > 0)
                {
                    nextNode = euler.First(n => n.IsVisited == false);
                    Edge e = startNode.Edges.Where(x => x.TargetNode == nextNode).First();
                    durchlauf.Last().Add(e);
                    e.Visited = true;
                    weight += e.Weight;
                    startNode = nextNode;
                }
            }
            // Letzte Kante und letztes Gewicht hinzufügen
            Edge eds = EdgeList.Where(n => n.SourceNode.ID == durchlauf.Last().ID && n.TargetNode.ID == durchlauf.First().ID).First();
            durchlauf.Last().Add(eds);

            weight += eds.Weight;
            Console.WriteLine("Distanz doppelter Baum: {0}", weight);
        }
        #endregion            

    }
}