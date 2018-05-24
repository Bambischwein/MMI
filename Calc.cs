using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMITest;
using System.Diagnostics;
using System.Threading;

namespace MMITest
{
	public class Calc
	{

		int _CountOfIterations = 0;

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
			// Console.WriteLine("Starte Tiefensuche");
			Reset();
			Tiefensuche(NodeList.First(), new Graph());
			Console.WriteLine("");
		}


		/// <summary>
		/// Tiefensuche
		/// </summary>
		/// <param name="node">Node.</param>
		private List<Node> Tiefensuche(Node node, Graph primGraph)
		{
			// Knoten initialisiert mit -1, also muss der counter  bei 1 starten
			int counter = 1;
			// Tiefensuche (node, counter);
			List<Node> nodeList = new List<Node>();
			nodeList.Add (new Node(node.ID));
			foreach (Node n in primGraph.NodeList)
			{
				if (n.ComponentCount == -1)
				{
					// Console.WriteLine("Berechne Komponente {0}", counter);
					// Console.WriteLine("Start Node {0}", n.ID);
					ComponentsList.Add(new List<Node>());
					Tiefensuche(n, counter, nodeList, primGraph);
					counter++;

				}
			}
			return nodeList;
		}

		/// <summary>
		/// Tiefensuche
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="counter">Counter.</param>
		private void Tiefensuche(Node node, int counter, List<Node> nodeList, Graph primGraph)
		{
			node.IsVisited = true;
			// nodeList.Add(new Node(node.ID));
			node.ComponentCount = counter;
			ComponentsList[counter - 1].Add(node);
			// Console.WriteLine("Current Node: {0}", node.ID);
			foreach (Edge e in node.Edges)
			{
				Node nextVertice = primGraph.NodeList.Where(n => n.ID == e.TargetNode.ID).First();
				if (nextVertice.ComponentCount == -1)
				{
					nodeList.Add (new Node(nextVertice.ID));
					Tiefensuche(nextVertice, counter, nodeList, primGraph);
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
		private Graph PrimReturn(Node startNode)
        {
            Reset();

			Graph primGraph = new Graph ();
            // List<Node> primNodeList = new List<Node>();
            for (int i = 0; i < NodeList.Count(); i++)
            {
                primGraph.NodeList.Add(new Node(i));
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
                primGraph.NodeList[e.SourceNode.ID].Edges.Add(e);
                edgeList.Remove(e);
            }
            return primGraph;
            
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

		public double NaechsterNachbar(Node startKnoten)
		{
			Reset();
			// Schritt 1: : Wähle einen beliebigen Knoten als Startknoten v
			startKnoten.IsVisited = true;
			double weight = 0.0;
			List<Node> hKreis = new List<Node> ();

			Node nextNode = startKnoten; // Damit Startknoten nicht überschrieben wird
			while (NodeList.Where(n => n.IsVisited == false).Any()) // n mal ist effizienter
			{
				// Schritt 2: : Ermittle die niedrigste Kante, welche den aktuellen Knoten v mit einem
				// unbesuchten Knoten vu verbindet.
				Edge minEdge = nextNode.Edges.ToList().OrderBy(e => e.Weight).Where(e => e.TargetNode.IsVisited == false).First(); // geht effizienter
				weight += minEdge.Weight;
				// Schritt 3: : Setze v = vu                
				hKreis.Add (new Node(nextNode.ID));
				hKreis.Last ().Add (minEdge);
				nextNode = minEdge.TargetNode;
				nextNode.IsVisited = true;
				// Schritt 4: : Wenn noch nicht alle Knoten besucht wurden gehe wieder zu Schritt 2.
			}

			// Schritt 5: : Füge die Kante vom letzten besuchten Knoten zum Startknoten hinzu um den Kreis zu schließen.
			hKreis.Add(new Node(nextNode.ID));
			Edge eds = EdgeList.Where(n => n.SourceNode.ID == hKreis.Last().ID && n.TargetNode.ID == hKreis.First().ID).First();
			hKreis.Last().Add(eds);
			weight += nextNode.Edges.Where(node => ((node.TargetNode == startKnoten))).First().Weight;  


			// Ausgabe
			// foreach (var n in hKreis) 
			// {
			// 	Console.Write ("({0}|{1}) ", n.Edges.First().SourceNode.ID, n.Edges.First().TargetNode.ID);
			// }
			// Console.WriteLine ();
			// Console.WriteLine("Startknoten: {0}; Distanz: {1}", startKnoten.ID, weight);

			// return hKreis;
			return weight;
		}
        #endregion

        #region Doppelter Baum
		// mit prim graph zurück geben und eine tiefensuche durchführen, die die reihenfolge zurück gibt

		public void DoppelterBaum(Node startNode)
		{
			// Tiefensuche auf Prim!!
			Graph primGraph = PrimReturn(startNode);
			// nlist ist die List der Knoten ohne Kanten aus der Tiefensuche
			List<Node> nList = Tiefensuche (primGraph.NodeList.First(), primGraph);
			double weight = 0.0;
			for (int i = 0; i < nList.Count() - 1; i++) 
			{
				Node s	= nList [i];
				Node t = nList [i+1];
				// finde richtige Kante in der Nodelist
				Node tmp = NodeList.Where(n => n.ID == s.ID).First();
				Edge e = tmp.Edges.Where (n => n.TargetNode.ID == t.ID).First ();
				s.Add (new Edge(s, t, e.Weight));
				weight += e.Weight;
			}
			Node se = nList.Last();
			Node te = nList.First();
			Node temp = NodeList.Where(n => n.ID == se.ID).First();
			Edge ee = temp.Edges.Where (n => n.SourceNode.ID == se.ID && n.TargetNode.ID == te.ID).First ();
			se.Add (new Edge(se, te, ee.Weight));
			weight += ee.Weight;


			// foreach (Node n in nList) 
			// {
			// 	Console.Write ("({0}|{1}) ", n.Edges.First().SourceNode.ID, n.Edges.First().TargetNode.ID);
			// }
			// Console.WriteLine ();
			// Console.WriteLine("Distanz: {0}", weight);


		}
        #endregion            

		#region Alle Touren 

		public void AlleTouren()
		{			
			Console.WriteLine ("Alle Touren:");
			// erste optimale Tour finden
			double maxWeight = NaechsterNachbar (NodeList.First ());
			List<Node> actGraph = new List<Node> ();
			double actWeight = 0;
			List<Node> bestPath = new List<Node> ();
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			foreach (var node in NodeList)
			{
				actGraph.Add (node);
				AlleTourenRekursiv (ref maxWeight, actGraph, actWeight, false, ref bestPath);
				actGraph = new List<Node> ();
			}
			stopWatch.Stop ();
			TimeSpan ts = stopWatch.Elapsed;
			Console.WriteLine ("Time: {0}: ", ts);
			Console.WriteLine ("Count: {0}", _CountOfIterations);
			Console.WriteLine ("Beste Tour mit Gewicht: {0}, ", maxWeight);
			foreach (var node in bestPath) {
				Console.Write ("{0} ->", node.ID);
			}
			Console.WriteLine ();
		}

		private void AlleTourenRekursiv(ref double maxWeight, List<Node> actGraph, double actWeight, bool Ausgabe, ref List<Node> bestPath)
		{
			if (actGraph.Count () == NodeList.Count ()) 
			{
				actWeight += NodeList.SelectMany (n => n.Edges).ToList().
					Where (n => n.SourceNode.ID == actGraph.Last ().ID && n.TargetNode.ID == actGraph.First().ID).First().Weight;
				// bestes Gewicht und besten Graphen aktualisieren
				if (actWeight < maxWeight) 
				{
					maxWeight = actWeight;
					bestPath = new List<Node> ();
					foreach (var node in actGraph) 
					{
						bestPath.Add(node);	
					}
				}
				_CountOfIterations++;
				
				if (Ausgabe) {
					Console.WriteLine ("Graph mit Gewicht: {0}: ", actWeight);
					foreach (var node in actGraph) {
						Console.Write ("{0} ->", node.ID);
					}
					Console.WriteLine ();
				}
			} 
			else 
			{
				Node tempNode = actGraph.Last ();
				foreach (var edge in tempNode.Edges) {
					if (!actGraph.Contains (edge.TargetNode)) {
						actWeight += edge.Weight;
						List<Node> tempGraph = actGraph;
						tempGraph.Add (edge.TargetNode);
							AlleTourenRekursiv (ref maxWeight, tempGraph, actWeight, Ausgabe, ref bestPath);		
							actWeight -= edge.Weight;
							actGraph.Remove (actGraph.Last ());							
					}
				}
			}

		}

		#endregion

		#region Branch and Bound 

		public void BranchAndBond()
		{			
			Console.WriteLine ("Branch and Bound:");
			// erste optimale Tour finden
			double maxWeight = NaechsterNachbar (NodeList.First ());
			_CountOfIterations = 0;
			List<Node> actGraph = new List<Node> ();
			double actWeight = 0;
			List<Node> bestPath = new List<Node> ();
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			foreach (var node in NodeList)
			{
				actGraph.Add (node);
				BranchAndBoundRekursiv (ref maxWeight, actGraph, actWeight, false, ref bestPath);
				actGraph = new List<Node> ();
			}
			stopWatch.Stop ();
			TimeSpan ts = stopWatch.Elapsed;
			Console.WriteLine ("Time: {0}: ", ts);
			Console.WriteLine ("Count: {0}", _CountOfIterations);
			Console.WriteLine ("Beste Tour mit Gewicht {0}: ", maxWeight);
			foreach (var node in bestPath) {
				Console.Write ("{0} ->", node.ID);
			}
			Console.WriteLine ();
		}

		private void BranchAndBoundRekursiv(ref double maxWeight, List<Node> actGraph, double actWeight, bool Ausgabe, ref List<Node> bestPath)
		{
            if (actGraph.Count() == NodeList.Count())
            {
                actWeight += NodeList.SelectMany(n => n.Edges).ToList().
                    Where(n => n.SourceNode.ID == actGraph.Last().ID && n.TargetNode.ID == actGraph.First().ID).First().Weight;
                // bestes Gewicht und besten Graphen aktualisieren
                if (actWeight < maxWeight)
                {
                    maxWeight = actWeight;
                    bestPath = new List<Node>();
                    foreach (var node in actGraph)
                    {
                        bestPath.Add(node);
                    }
                }
                _CountOfIterations++;

                if (Ausgabe)
                {
                    Console.WriteLine("Graph mit Gewicht: {0}: ", actWeight);
                    foreach (var node in actGraph)
                    {
                        Console.Write("{0} ->", node.ID);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Node tempNode = actGraph.Last();
                foreach (var edge in tempNode.Edges)
                {
                    if (!actGraph.Contains(edge.TargetNode))
                    {
                        actWeight += edge.Weight;
                        List<Node> tempGraph = actGraph;
                        tempGraph.Add(edge.TargetNode);
                        if (actWeight < maxWeight)
                        {
                            BranchAndBoundRekursiv(ref maxWeight, tempGraph, actWeight, Ausgabe, ref bestPath);
                        }
                        actWeight -= edge.Weight;
                        actGraph.Remove(actGraph.Last());
                    }
                }
            }

        }

        #endregion

        #region Dijkstra

        public void Dijkstra(Node s)
        {
            // Initialisierung
            Dictionary<Node, Tuple<double, Node>> kwb = Initialize(s);               

            // Durchführung
            while (kwb.Any(n => n.Key.IsVisited == false && n.Value.Item1 == FindMin(kwb))) // knoten finden
            {
                Node n = kwb.Where(b => b.Key.IsVisited == false && b.Value.Item1 == FindMin(kwb)).First().Key;
                foreach (Edge e in n.Edges)
                {
                    // Aktualisieren wenn nötig
                    if (kwb[e.TargetNode].Item1 > e.Weight || kwb[e.TargetNode].Item1 < 0)
                    {
                        kwb[e.TargetNode] = new Tuple<double, Node>(e.Weight, n);
                    }
                    n.IsVisited = true;
                }
            }
            
        }


        private Dictionary<Node, Tuple<double, Node>> Initialize(Node s)
        {
            Dictionary<Node, Tuple<double, Node>> kwb = new Dictionary<Node, Tuple<double, Node>>();

            foreach (Node n in NodeList)
            {
                kwb.Add(n, new Tuple<double, Node>(double.PositiveInfinity, null));
            }
            kwb[s] = new Tuple<double, Node>(0.0, s);
            return kwb;
        }

        private double FindMin(Dictionary<Node, Tuple<double, Node>> kwb)
        {
            double min = 0.0;           
            foreach (Node n in kwb.Keys)
            {
                    min = kwb[n].Item1 < min ? kwb[n].Item1 : min;
            }
            return min;
        }
        #endregion

        #region Moore-Bellmann-Ford


        public void MooreBellmanFord(Node s)
        {
            // Initialisierung
            Dictionary<Node, Tuple<double, Node>> kwb = Initialize(s);

            // Durchführung
            for (int i = 0; i < NodeList.Count() - 1; i++)
            {
                foreach (Edge  e in NodeList[i].Edges)
                {
                    // Aktualisieren wenn nötig
                    if (kwb[e.TargetNode].Item1 > e.Weight || kwb[e.TargetNode].Item1 < 0)
                    {
                        kwb[e.TargetNode] = new Tuple<double, Node>(e.Weight, NodeList[i]);
                    }
                }
                foreach (Edge e in EdgeList)
                {
                    if (true)
                    {

                    }
                }
            }
        }
        #endregion
    }
}