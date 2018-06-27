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


		/// <summary>
		/// Tiefensuche
		/// </summary>
		/// <param name="node">Node.</param>
		private List<Node> TiefensucheZiel(Node src, Node trg, Graph primGraph)
		{
			// Knoten initialisiert mit -1, also muss der counter  bei 1 starten
			int counter = 1;
			// Tiefensuche (node, counter);
			List<Node> nodeList = new List<Node>();
			nodeList.Add (new Node(src.ID));
			foreach (Node n in primGraph.NodeList)
			{
				if (n.ComponentCount == -1)
				{
					// Console.WriteLine("Berechne Komponente {0}", counter);
					// Console.WriteLine("Start Node {0}", n.ID);
					ComponentsList.Add(new List<Node>());
					TiefensucheZiel(n, trg, counter, nodeList, primGraph);
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
		private void TiefensucheZiel(Node src, Node trg, int counter, List<Node> nodeList, Graph primGraph)
		{
			src.IsVisited = true;
			// nodeList.Add(new Node(node.ID));
			src.ComponentCount = counter;
			ComponentsList[counter - 1].Add(src);
			// Console.WriteLine("Current Node: {0}", node.ID);
			foreach (Edge e in src.Edges)
			{
				Node nextVertice = primGraph.NodeList.Where(n => n.ID == e.TargetNode.ID).First();
				if (nextVertice.ComponentCount == -1 && nextVertice != trg)
				{
					nodeList.Add (new Node(nextVertice.ID));
					TiefensucheZiel(nextVertice, trg, counter, nodeList, primGraph);
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

        private List<Edge> BreitensucheMaxFluss(Node startNode, Node endNode, Graph resi)
        {
            Reset();

            List<Edge> tmp = new List<Edge>();
            Queue<Node> Q = new Queue<Node>();
            Q.Enqueue(startNode);
            startNode.IsVisited = true;
            while (Q.Any())
            {
                Node currentNode = Q.Dequeue();
                foreach (var edge in resi.NodeList[currentNode.ID].Edges)
                {
                    if (!edge.TargetNode.IsVisited)
                    {
                        Q.Enqueue(edge.TargetNode);
                        tmp.Add(edge);
                        edge.TargetNode.IsVisited = true;
                        if (edge.TargetNode.ID == endNode.ID)
                        {
							tmp = GetPath(startNode, endNode, resi, tmp);
                            return tmp;
                        }
                    }
                }
            }
            return new List<Edge>();
        }

		private List<Edge> BreitensucheMinFluss(Node startNode, Graph resi)
		{
			Reset();

			List<Edge> tmp = new List<Edge>();
			Queue<Node> Q = new Queue<Node>();
			Q.Enqueue(startNode);
			startNode.IsVisited = true;
			while (Q.Any())
			{
				Node currentNode = Q.Dequeue();
				foreach (var edge in resi.NodeList[currentNode.ID].Edges)
				{
					if (!edge.TargetNode.IsVisited)
					{
						Q.Enqueue(edge.TargetNode);
						tmp.Add(edge);
						edge.TargetNode.IsVisited = true;
					}
				}
			}
			return tmp;
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
			double actWeight = 0.0;
			_CountOfIterations = 0;
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
			// Ausgabe
			TimeSpan ts = stopWatch.Elapsed;
			Console.WriteLine ("Time: {0}: ", ts);
			Console.WriteLine ("Count: {0}", _CountOfIterations);
			Console.WriteLine ("Beste Tour mit Gewicht {0}: ", maxWeight);
			foreach (var node in bestPath) {
				Console.Write ("{0} ->", node.ID);
			}
			Console.WriteLine ();
		}

		private void AlleTourenRekursiv(ref double maxWeight, List<Node> actGraph, double actWeight, bool Ausgabe, ref List<Node> bestPath)
		{
			// Wenn Tour vollständig:
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
				// Ausgabe
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
						actGraph.Add (edge.TargetNode);
						AlleTourenRekursiv (ref maxWeight, actGraph, actWeight, Ausgabe, ref bestPath);		
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
			double actWeight = 0.0;
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
				Node tempNode = actGraph.Last ();
				foreach (var edge in tempNode.Edges) {
					if (!actGraph.Contains (edge.TargetNode)) {
						actWeight += edge.Weight;
						actGraph.Add (edge.TargetNode);
						if (actWeight < maxWeight) 
						{
							AlleTourenRekursiv (ref maxWeight, actGraph, actWeight, Ausgabe, ref bestPath);		
						}
						actWeight -= edge.Weight;
						actGraph.Remove (actGraph.Last ());							
					}
				}
			}
        }

        #endregion

        #region Dijkstra

		public Dictionary<Node, Tuple<double, int>> Dijkstra(Node s)
        {
			Reset ();
            // Initialisierung
			Dictionary<Node, Tuple<double, int>> kwb = new Dictionary<Node, Tuple<double, int>>();// Initialize(s, NodeList);               

            // Durchführung
            while (kwb.Any(n => n.Key.IsVisited == false)) // knoten finden
            {
                Node n = kwb.Where(b => b.Key.IsVisited == false && b.Value.Item1 == FindMin(kwb)).First().Key;
                foreach (Edge e in n.Edges)
                {
                    // Aktualisieren wenn nötig
                    if (kwb[e.TargetNode].Item1 > e.Weight + kwb[n].Item1)
                    {
                        double newWeight = kwb[n].Item1 + e.Weight;
                        kwb[e.TargetNode] = new Tuple<double, int>(newWeight, n.ID);
                    }

                }
				n.IsVisited = true;
            }
			return kwb;
        }

        private Dictionary<Node, Tuple<double, int>> Dijkstra(Node s, Graph resi)
        {
            Reset();
            // Initialisierung
            Dictionary<Node, Tuple<double, int>> kwb = Initialize(s, resi);

            // Durchführung
            while (kwb.Any(n => n.Key.IsVisited == false)) // knoten finden
            {
                Node n = kwb.Where(b => b.Key.IsVisited == false && b.Value.Item1 == FindMin(kwb)).First().Key;
                foreach (Edge e in n.Edges)
                {
                    // Aktualisieren wenn nötig
                    if (kwb[e.TargetNode].Item1 > e.Capacity + kwb[n].Item1)
                    {
                        double newWeight = kwb[n].Item1 + e.Capacity;
                        kwb[e.TargetNode] = new Tuple<double, int>(newWeight, n.ID);
                    }
                }
                n.IsVisited = true;
            }
            return kwb;
        }

        private double FindMin(Dictionary<Node, Tuple<double, int>> kwb)
        {
            double min = double.PositiveInfinity;
            foreach (Node n in kwb.Keys)
            {
                if (kwb[n].Item1 < min && n.IsVisited == false)
                {
                    min = kwb[n].Item1;
                }
            }
            return min;
        }

        #endregion

		public Dictionary<Node, Tuple<double, int>> Initialize(Node s, List<Node> tmpNodeList)
        {
            Dictionary<Node, Tuple<double, int>> kwb = new Dictionary<Node, Tuple<double, int>>();

            foreach (Node n in tmpNodeList)
            {
				kwb.Add(n, new Tuple<double, int>(double.PositiveInfinity, -1));
            }
			kwb [s] = new Tuple<double, int> (0.0, s.ID);
            return kwb;
        }

        private Dictionary<Node, Tuple<double, int>> Initialize(Node s, Graph resi)
        {
            Dictionary<Node, Tuple<double, int>> kwb = new Dictionary<Node, Tuple<double, int>>();

            foreach (Node n in resi.NodeList)
            {
                kwb.Add(n, new Tuple<double, int>(double.PositiveInfinity, -1));
            }
            kwb[kwb.Where(v => v.Key.ID == s.ID).First().Key] = new Tuple<double, int>(0.0, s.ID);
            return kwb;
        }

        #region Moore-Bellmann-Ford

        public Boolean MooreBellmanFord(Node s, ref Dictionary<Node, Tuple<double, int>> kwb)
        {
			Reset ();
            // Initialisierung
			// kwb = Initialize(s, NodeList);
            s.IsVisited = true;
            // Durchführung: n - 1 mal
            for (int i = 0; i < NodeList.Count() - 1; i++)
            {
                foreach (Edge  e in EdgeList)
                {
                    // Aktualisieren wenn nötig
                    if (kwb[e.TargetNode].Item1 > e.Weight + kwb[e.SourceNode].Item1 && e.SourceNode.IsVisited)
                    {
                        double newWeight = kwb[e.SourceNode].Item1 + e.Weight;
                        kwb[e.TargetNode] = new Tuple<double, int>(newWeight, e.SourceNode.ID);
                        e.TargetNode.IsVisited = true;
                    }
                }               
            }
            foreach (Edge e in EdgeList)
            {
                if (kwb[e.SourceNode].Item1 + e.Weight < kwb[e.TargetNode].Item1)
                {
                    return false;
                }
            }
            return true;
        }

        private Boolean MooreBellmanFord(Node s, ref Dictionary<Node, Tuple<double, int>> kwb, Graph resi)
        {
            Reset();
            // Initialisierung
            kwb = Initialize(s, resi);
            resi.NodeList[s.ID].IsVisited = true;
			// Durchführung: n - 1 mal
			for (int i = 0; i < resi.NodeList.Count() - 1; i++)
			{
				foreach (Edge  e in resi.AllEdges)
				{
                    // Aktualisieren wenn nötig
                    if (kwb.Where(k => k.Key.ID == e.TargetNode.ID).First().Value.Item1 >
                        e.Cost + kwb.Where(k => k.Key.ID == e.SourceNode.ID).First().Value.Item1 && e.SourceNode.IsVisited)
					{
						double newWeight = kwb.Where(k => k.Key.ID == e.SourceNode.ID).First().Value.Item1 + e.Cost;
						kwb[NodeList.Where(n => n.ID == e.TargetNode.ID).First()] = new Tuple<double, int>(newWeight, e.SourceNode.ID);
						e.TargetNode.IsVisited = true;
					}
				}               
			}
			foreach (Edge e in EdgeList)
			{
				if (kwb[e.SourceNode].Item1 + e.Cost < kwb[e.TargetNode].Item1)
				{
					return false;
				}
			}
			return true;
		}
			
        private List<Edge> GetPath(Node src, Node trg, Graph g, List<Edge> route)
        {
            List<Edge> shortestPath = new List<Edge>();
            List<Edge> tmpPath = new List<Edge>();

            try
            {
                Node n = trg;
                while (n.ID != src.ID)
                {
                    Edge tmpSrcEdge = route.Where(e => e.TargetNode.ID == n.ID).First();
                    tmpPath.Add(tmpSrcEdge);
                    n = tmpSrcEdge.SourceNode;
                }
                while (tmpPath.Count() > 0)
                {
                    shortestPath.Add(tmpPath.Last());
                    tmpPath.Remove(tmpPath.Last());
                }
            }
            catch (Exception)
            {
            }
            return shortestPath;
        }

        #endregion

        #region max Flüsse

        /*
         Input: Netzwerk (G, u, s, t).
        Output: Maximaler Fluss 𝑓.
        Schritt 1: Setzen Sie 𝑓(𝑒) = 0 für alle Kanten 𝑒 𝜖 𝐸.
        Schritt 2: Bestimmen Sie 𝐺^𝑓 und 𝑢^𝑓(𝑒).
        Schritt 3: Konstruieren Sie einen einfachen (s, t)-Weg 𝑝 in 𝐺^𝑓. Falls keiner existiert: STOPP.
        Schritt 4: Verändern Sie den Fluss 𝑓 entlang des Wegs 𝑝 um 𝛾 ∶= 𝑚𝑖𝑛_𝑒_𝜖_𝑝 𝑢^𝑓(𝑒).
        Schritt 5: Gehen Sie zu Schritt 2.
        */
		private double EdmondsKarpMaxFluss(Node src, Node trg, Graph resi)
        {
            Reset();
            Console.WriteLine("Calculate Edmons Karp:");
			Graph residualGraph = InitResidualGraph();
			residualGraph = CreateResidualGraph (resi);
			double maxFluss = 0.0;
            List<Edge> route = BreitensucheMaxFluss(src, trg, residualGraph);
            
            while (route.Count > 0)
            {
				double mFluss = minFluss(route);
				maxFluss += mFluss;
                Console.WriteLine("Max Fluss: {0}.", maxFluss);
                Console.WriteLine("Update Edges.");
                CalculateCapacities(residualGraph, route, mFluss, resi);
                residualGraph = CreateResidualGraph(resi);
                route = BreitensucheMaxFluss(src, trg, residualGraph);                             
            }
			resi = residualGraph;
			return maxFluss;
        }

		private double minFluss (List<Edge> route)
		{
			double minFluss = double.PositiveInfinity;
			foreach (var e in route)
            {
                minFluss = Math.Min(minFluss, e.Capacity);
            }
			return minFluss;
		}

		private void CalculateCapacities(Graph resi, List<Edge> route, double minFluss, Graph superGraph)
		{
            foreach (Node n in  resi.NodeList)
            {
                foreach (Edge e in n.Edges)
                {
                    if (route.Contains(e))
                    {
                        try
                        {                        
	                        Edge originalEdge = superGraph.AllEdges.Where(q => q.SourceNode.ID == e.SourceNode.ID && q.TargetNode.ID == e.TargetNode.ID).First();
	                        // update flow and residual capacity                         
	                        originalEdge.Flow += minFluss;
	                        originalEdge.Capacity -= minFluss;
	                        Console.WriteLine("Fluss und Kapazität von Kante {0} zu {1}. Fluss: {2}, Kapazität: {3}", e.SourceNode.ID, e.TargetNode.ID, originalEdge.Flow, originalEdge.Capacity);
                        }
                        catch (Exception)
                        {							
								Edge originalEdge = superGraph.AllEdges.Where(q => q.SourceNode.ID == e.TargetNode.ID && q.TargetNode.ID == e.SourceNode.ID).First();
								originalEdge.Flow -= minFluss;
								originalEdge.Capacity += minFluss;
                        }
                    }
                }
            }
		}

        private Graph InitResidualGraph()
        {
            Graph residualGraph = new Graph();
            foreach (Node n in NodeList)
            {
                residualGraph.NodeList.Add(n);            
            }

            return residualGraph;
        }

        private Graph CreateResidualGraph(Graph resi)
        {
            //Liste aller edges mit Kapazität > 0
			List<Edge> CEdges = resi.NodeList.SelectMany(node => node.Edges).Where(edge => edge.Capacity > 0 ).ToList(); //&& edge.Flow < edge.Capacity).ToList();

            //Rückkanten für alle Kanten, die Fluss haben
            List<Edge> FEdges = resi.AllEdges.Where(edge => edge.Flow > 0).ToList();

            Console.WriteLine("Capacity Edges count: {0}", CEdges.Count);

            Graph residualGraph = new Graph();
            foreach (Node node in resi.NodeList)
            {
                Node newNode = new Node(node.ID);
                newNode.Balance = node.Balance;
                residualGraph.NodeList.Add(newNode);
            }

            foreach (Edge e in CEdges)
            {
				Edge newEdge = new Edge(residualGraph.NodeList[e.SourceNode.ID], residualGraph.NodeList[e.TargetNode.ID], e.Weight, e.Capacity, e.Cost);
                newEdge.Flow = e.Flow;

                residualGraph.NodeList[e.SourceNode.ID].Add(newEdge);
            }

            
            Console.WriteLine("Flow Edges count: {0}", FEdges.Count);

            foreach (Edge e in FEdges)
            {
                Edge newReverseEdge = new Edge(residualGraph.NodeList[e.TargetNode.ID], residualGraph.NodeList[e.SourceNode.ID], 0.0, e.Flow, -e.Cost);
                newReverseEdge.Flow = 0;

                residualGraph.NodeList[e.TargetNode.ID].Add(newReverseEdge);
            }
            return residualGraph;
        }

        #endregion

		#region Minimale Fluesse

		public double SSP()
		{
            Graph resi = InitCleanResidualGraph();

			Console.WriteLine ("Calculate SSP");
            // Schritt 1: Setze f(e) und b(v)-> finde alle Kanten mit negativen Kosten und laste sie voll aus
            List<Edge> negEdges = NodeList.SelectMany(node => node.Edges).Where(e => e.Cost < 0).ToList();
            foreach (Edge e in negEdges)
            {
                if (e.Cost < 0)
                {
                    e.Flow = e.Capacity;
                    e.SourceNode.BalanceModified += e.Capacity;
                    e.TargetNode.BalanceModified -= e.Capacity;
                }
            }

            resi = CreateResidualGraph(resi);

            // Schritt 2:  Finde s und t                    
            IList<Node> SrcList = NodeList.Where(n => n.Balance - n.BalanceModified > 0).ToList();
            IList<Node> TrgList = NodeList.Where(n => n.Balance - n.BalanceModified < 0).ToList();


            // Schritt 3: Kürzesten Weg berechnen
            Dictionary<Node, Tuple<double, int>> kwb = new Dictionary<Node, Tuple<double, int>>();
            List<Edge> shortestPath = new List<Edge>();
            foreach (Node source in SrcList)
            {
                foreach (Node sink in TrgList)
                {
                    Console.WriteLine("Calculating SSP from {0} to {1}", source.ID, sink.ID);
                    MooreBellmanFord(source, ref kwb, resi);                       
                    shortestPath = GetShortestPath(NodeList[source.ID], NodeList[sink.ID], kwb, resi);                    
                    while (shortestPath.Count() > 0 && SrcList.Contains(source) && TrgList.Contains(sink))
                    {
                        printRoute(shortestPath);

                        SetFlowAndBalance(shortestPath);

                        SrcList = NodeList.Where(n => (n.Balance - n.BalanceModified) > 0).ToList();
                        TrgList = NodeList.Where(n => (n.Balance - n.BalanceModified) < 0).ToList();

                        resi = CreateResidualGraph(resi);

                        Console.WriteLine("Calculating SSP from {0} to {1}", source.ID, sink.ID);
                        MooreBellmanFord(source, ref kwb, resi);
                        shortestPath = GetShortestPath(NodeList[source.ID], NodeList[sink.ID], kwb, resi);
                    }
                }
            }
            foreach (Node n in NodeList)
            {
                if (n.Balance != n.BalanceModified)
                {
                    return double.NaN;
                }
            }
            return EdgeList.Sum(edge => edge.Cost * edge.Flow);
        }

        private void SetFlowAndBalance(List<Edge> shortestPath)
        {
            double maxFlow = Double.PositiveInfinity;
            // maximal mögliche Kapazität der route berechnen
            Node source = NodeList.Where(n => n.ID == shortestPath.First().SourceNode.ID).First();
            Node sink = NodeList.Where(s => s.ID == shortestPath.Last().TargetNode.ID).First();
            maxFlow = Math.Min(maxFlow, source.Balance - source.BalanceModified);
            maxFlow = Math.Min(maxFlow, sink.BalanceModified - sink.Balance);

            foreach (Edge e in shortestPath)
            {
                maxFlow = Math.Min(maxFlow, e.Capacity - e.Flow);
            }

            Console.WriteLine("max flow for this route: {0}", maxFlow);

            // Flow updaten
            Console.WriteLine("Updating flow in edges...");
            foreach (Edge e in shortestPath)
            {
                Edge originalEdge = getEdge(e.SourceNode, e.TargetNode);
                if (originalEdge != null)
                {
                    originalEdge.Flow += maxFlow;
                
                }
                else //reverse edge in residualgraph
                {
                    originalEdge = getEdge(e.TargetNode, e.SourceNode);
                    originalEdge.Flow -= maxFlow;
    
                }
                // originalEdge.ToString();
            }

            //update modified balance for begin and end of route
            Console.WriteLine("Updating modified Balances...");
            NodeList.ElementAt(shortestPath.First().SourceNode.ID).BalanceModified += maxFlow;
            NodeList.ElementAt(shortestPath.First().SourceNode.ID).ToString();

            NodeList.ElementAt(shortestPath.Last().TargetNode.ID).BalanceModified -= maxFlow;
            NodeList.ElementAt(shortestPath.Last().TargetNode.ID).ToString();
        }

        private Edge getEdge(Node src, Node trg)
        {
            return NodeList.SelectMany(node => node.Edges).Where(edge => edge.SourceNode.ID == src.ID && edge.TargetNode.ID == trg.ID).FirstOrDefault();
        }

        private List<Edge> GetShortestPath(Node source, Node sink, Dictionary<Node, Tuple<double, int>> kwb, Graph resi)
        {
            List<Edge> tmpshortestPath = new List<Edge>();
            List<Edge> shortestPath = new List<Edge>();

            Node n = sink;
            try
            {
                while (n.ID != source.ID)
                {
                    Node tmp = NodeList[kwb[n].Item2];
                    tmpshortestPath.Add(resi.AllEdges.Where(e => e.SourceNode.ID == tmp.ID && e.TargetNode.ID == n.ID).First());
                    n = tmp;
                }
                while (tmpshortestPath.Count() > 0)
                {
                    shortestPath.Add(resi.AllEdges.Where(e => e.SourceNode.ID == tmpshortestPath.Last().SourceNode.ID &&
                        e.TargetNode.ID == tmpshortestPath.Last().TargetNode.ID).First());
                    // shortestPath.Add(tmpshortestPath.Last());
                    tmpshortestPath.Remove(tmpshortestPath.Last());
                }
            }
            catch
            {
                return new List<Edge>();
            }
            return shortestPath;
        }


        private Graph InitCleanResidualGraph()
		{
			Graph residualGraph = new Graph();
			foreach (Node n in NodeList)
			{
				Node node = new Node (n.ID);
				foreach (Edge e in n.Edges) 
				{
					node.Add (new Edge (e.SourceNode, e.TargetNode, e.Weight, e.Capacity, e.Cost));
				}
				residualGraph.NodeList.Add(node);    
			}

			return residualGraph;
		}

        #endregion

        #region cycle canceling

        public double CC()
        {            
            Dictionary<Node, Tuple<double, int>> kwb = new Dictionary<Node, Tuple<double, int>>();
            List<Edge> zykel = new List<Edge>();
            Node zykelNode;
            double minResidualCapacity = double.PositiveInfinity;
            // um den originalgraphen nicht zu verändern und ihn an ek weiterzugeben
            Graph superGraph = InitResidualGraph ();

            // Schritt 1: B-Fluss suchen, wenn keiner gefunden STOPP
            // Super-Quelle und Super-Senke einfügen, dann edmonds-karp anwenden                
            IList<Node> SrcList = NodeList.Where(n => n.Balance > 0).ToList();
            IList<Node> TrgList = NodeList.Where(n => n.Balance < 0).ToList();

            Node superSource = new Node(superGraph.NodeList.Count());
            foreach (Node n in SrcList)
            {
                superSource.Add(new Edge(superSource, n, 0.0, n.Balance, 0.0));
				superGraph.AllEdges.Add(new Edge(superSource, n, 0.0, n.Balance, 0.0));
				superSource.Balance += n.Balance;
            }
			superGraph.NodeList.Add(superSource);
            Node superSink = new Node(superGraph.NodeList.Count());
            foreach (Node n in TrgList)
            {
                n.Add(new Edge(n, superSink, 0.0, -n.Balance, 0.0));
				superGraph.AllEdges.Add(new Edge(n, superSink, 0.0, -n.Balance, 0.0));
				superSink.Balance += n.Balance;
            }		
            superGraph.NodeList.Add(superSink);
			// Berechnung des Flusses:
			double initBFlow = EdmondsKarpMaxFluss(superSource, superSink, superGraph);
            if (initBFlow <= 0)
            {
                return double.NaN;
            }
            RemoveSuperNodes(ref superGraph, superSource, superSink);
            // Schritt 2: Residualgraphen bestimmen
            Graph resi = InitCleanResidualGraph();
            resi = CreateResidualGraph(superGraph);

            foreach (Node n in resi.NodeList)
            {
                while (true)
                {
                    // Schritt 3: f-augmentierenden Zykel im Residualgraphen mit negativen Kosten bestimmen, sonst STOPP
				    zykelNode = MooreBellmanFordZykel(SrcList.First(), ref kwb, resi);
                    if (resi.NodeList.Any(c => c.ID == zykelNode.ID))
                    {
                        zykel = FindZykel(zykelNode, kwb, resi);
                    }
                    if (zykel.Count() <= 0)
                    {
                        return double.PositiveInfinity;
                    }
                    else
                    {
                        // Schritt 4: verändern des B-Flusses entlang des Zykels um γ := min u (e) .
                        foreach (Edge e in zykel)
                        {
                            double cap = resi.AllEdges.Where(u => u.SourceNode.ID == e.SourceNode.ID && u.TargetNode.ID == e.TargetNode.ID).First().Capacity;
                            minResidualCapacity = Math.Min(minResidualCapacity, cap);
                        }
                        // Fluss vermindern IM Residualgraph
                        // f(e) = f(e), falls e nicht in Z
                        // f(e) = f(e) + m, falls e in Z
                        // f(e) = f(e) - m, falls e gegen Z

                        foreach (Edge e in zykel)
                        {
                            Edge originalEdge = resi.AllEdges.Where(k => k.SourceNode.ID == e.SourceNode.ID && k.TargetNode.ID == e.TargetNode.ID).First();
                            if (originalEdge != null)
                            {
                                originalEdge.Flow += minResidualCapacity;
                            }
                            else
                            {
                                originalEdge = resi.AllEdges.Where(j => j.SourceNode.ID == e.TargetNode.ID && j.TargetNode.ID == e.SourceNode.ID).First();
                                originalEdge.Flow -= minResidualCapacity;
                            }
                            // sonst bleibt so, wie es ist, da e nicht in Z
                            
                        }
                        resi = CreateResidualGraph(resi);
                    }
                    // Schritt 5: Weiter mit Schritt 2
                }
            }
            return EdgeList.Sum(edge => edge.Cost * edge.Flow);
        }

        private List<Edge> FindZykel(Node start, Dictionary<Node, Tuple<double, int>> kwb, Graph resi)
        {
            List<Edge> zykelRueck = new List<Edge>();
            List<Edge> zykel = new List<Edge>();
            int test = kwb[resi.NodeList.Where(b => b.ID == start.ID).First()].Item2;
             Node n = NodeList.Where(c => c.ID == kwb[resi.NodeList.Where(b => b.ID == start.ID).First()].Item2).First();
            Node pred;
            zykelRueck.Add(new Edge(start, n));
            while (n != start)
            {
                pred = n;
                n = NodeList.Where(c => c.ID == kwb[resi.NodeList.Where(b => b.ID == n.ID).First()].Item2).First();
                zykelRueck.Add(new Edge(pred, n));
            }
            int zykelcount = zykelRueck.Count();
            for (int i = 0; i < zykelcount; i++)
            {
                zykel.Add(new Edge(zykelRueck.Last().TargetNode, zykelRueck.Last().SourceNode));
                zykelRueck.Remove(zykelRueck.Last());
            }
            return zykel;
        }
        private void RemoveSuperNodes(ref Graph g, Node src, Node trg)
        {
            g.NodeList.Remove(src);
            List<Edge> edgeToSuperSink = g.AllEdges.Where(e => e.TargetNode.ID == trg.ID).ToList();
            foreach (Edge e in edgeToSuperSink)
            {
                e.SourceNode.Remove(e);
            }
            g.NodeList.Remove(trg);
        }

        private List<Edge> GetZykel(Node superSource, Node superSink, Dictionary<Node, Tuple<double, int>> kwb, Graph resi)
        {


            return new List<Edge>();
        }

        private Node MooreBellmanFordZykel(Node s, ref Dictionary<Node, Tuple<double, int>> kwb, Graph resi)
        {
            Reset();
            // Initialisierung
            kwb = Initialize(s, resi);
            resi.NodeList[s.ID].IsVisited = true;
            // Durchführung: n - 1 mal
            for (int i = 0; i < resi.NodeList.Count() - 1; i++)
            {
                foreach (Edge e in resi.AllEdges)
                {
                    // Aktualisieren wenn nötig
                    if (kwb.Where(k => k.Key.ID == e.TargetNode.ID).First().Value.Item1 >
                        e.Cost + kwb.Where(k => k.Key.ID == e.SourceNode.ID).First().Value.Item1 && e.SourceNode.IsVisited)
                    {
                        double newWeight = kwb.Where(k => k.Key.ID == e.SourceNode.ID).First().Value.Item1 + e.Cost;
                        kwb[resi.NodeList.Where(n => n.ID == e.TargetNode.ID).First()] = new Tuple<double, int>(newWeight, e.SourceNode.ID);
                        e.TargetNode.IsVisited = true;
                    }
                }
            }
            List<Edge> cycle = new List<Edge>();
            foreach (Edge e in EdgeList)
            {
                Node src = resi.NodeList.Where(n => n.ID == e.SourceNode.ID).First();
                Node trg = resi.NodeList.Where(ne => ne.ID == e.TargetNode.ID).First();
                if (kwb[src].Item1 + e.Cost < kwb[trg].Item1)
                {
                    cycle.Add(e);

                    return e.SourceNode;
                }
            }
            return new Node(NodeList.Count() + 1);
        }
        #endregion
        public void EdgeListToString()
        {
            foreach (Edge e in EdgeList)
            {
                e.ToString();
            }
        }


        public void printRoute(List<Edge> list)
        {
            if (list.Count() > 0)
            {

                foreach (Edge e in list)
                {
                    Console.Write("{0} -----> ", e.SourceNode.ID);
                }
                Console.WriteLine("{0}", list.Last().TargetNode.ID);
            }
            else
            {
                Console.WriteLine("no path found");
            }
        }
    }
}
 