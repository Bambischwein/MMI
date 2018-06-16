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
                            return tmp;
                        }
                    }
                }
            }
            return new List<Edge>();
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
			Dictionary<Node, Tuple<double, int>> kwb = Initialize(s);               

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

        private Dictionary<Node, Tuple<double, int>> Initialize(Node s)
        {
            Dictionary<Node, Tuple<double, int>> kwb = new Dictionary<Node, Tuple<double, int>>();

            foreach (Node n in NodeList)
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
			kwb = Initialize(s);
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

		private List<Edge> GetPath(Node src, Node trg, Dictionary<Node, Tuple<double, int>> path)
		{
			List<Edge> tmpPath = new List<Edge> ();
			List<Edge> shortestPath = new List<Edge> ();

			Node n = trg;
            try
            {

			    while (n.ID != src.ID)
			    {
                    Node tmptrg = path.Keys.Where(w => w.ID == n.ID).First();
                    int vorgaengerid = path[tmptrg].Item2;
                    Node tmpsrc = path.Keys.Where(g => g.ID == vorgaengerid).First();
				    Edge e = tmpsrc.Edges.Where (v => v.TargetNode == tmptrg).First();
				    tmpPath.Add(e);
				    n = tmpsrc;	
			    }
                while (tmpPath.Count() > 0)
                {
                    shortestPath.Add(tmpPath.Last());
                    tmpPath.Remove(tmpPath.Last());
                }
                return shortestPath;
            }
            catch (Exception)
            {

                return new List<Edge>();
            }
        }

        private List<Edge> GetPath(Node src, Node trg, Graph g, ref List<Edge> route)
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
		public double EdmondsKarpMaxFluss(Node src, Node trg)
        {
            Reset();
            Console.WriteLine("Ford Fulkerson");
			Graph residualGraph = InitResidualGraph();
			double maxFluss = 0.0;

            //Dictionary<Node, Tuple<double, int>> d = Dijkstra(src, residualGraph);
            //List<Edge> route = GetPath(src, trg, d);

            List<Edge> route = BreitensucheMaxFluss(src, trg, residualGraph);
            route = GetPath(src, trg, residualGraph, ref route);

            while (route.Count > 0)
            {
				double mFluss = minFluss(route);
				maxFluss += mFluss;
                Console.WriteLine("Max Fluss:{0}.", maxFluss);
                Console.WriteLine("Update Edges.");
                CalculateCapacities(residualGraph, route, mFluss);
                residualGraph = CreateResidualGraph(residualGraph);

                route = BreitensucheMaxFluss(src, trg, residualGraph);
                route = GetPath(src, trg, residualGraph, ref route);

                // d = Dijkstra(src, residualGraph);
                // route = new List<Edge>();
                // route = GetPath(src, trg, d);                
            }
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

		private void CalculateCapacities(Graph resi, List<Edge> route, double minFluss)
		{
            foreach (Node n in  resi.NodeList)
            {
                foreach (Edge e in n.Edges)
                {
                    if (route.Contains(e))
                    {
                        try
                        {                        
                        Edge originalEdge = EdgeList.Where(q => q.SourceNode.ID == e.SourceNode.ID && q.TargetNode.ID == e.TargetNode.ID).First();
                        // update flow and residual capacity                         
                        originalEdge.Flow += minFluss;
                        originalEdge.Capacity -= minFluss;
                        Console.WriteLine("Update Flow and Capacity of Edge {0} zu {1}. Flow: {2}, Capacity: {3}", e.SourceNode.ID, e.TargetNode.ID, originalEdge.Flow, originalEdge.Capacity);
                        }
                        catch (Exception)
                        {
                            Edge originalEdge = EdgeList.Where(q => q.SourceNode.ID == e.TargetNode.ID && q.TargetNode.ID == e.SourceNode.ID).First();
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
            List<Edge> CEdges = NodeList.SelectMany(node => node.Edges).Where(edge => edge.Capacity > 0).ToList();

            //Rückkanten für alle Kanten, die Fluss haben
            List<Edge> FEdges = EdgeList.Where(edge => edge.Flow > 0).ToList();

            Console.WriteLine("Capacity Edges count: {0}", CEdges.Count);

            resi = new Graph();
            foreach (Node node in NodeList)
            {
                Node newNode = new Node(node.ID);
                resi.NodeList.Add(newNode);
            }

            foreach (Edge e in CEdges)
            {
                Edge newEdge = new Edge(resi.NodeList[e.SourceNode.ID], resi.NodeList[e.TargetNode.ID], e.Weight, e.Capacity);
                newEdge.Flow = e.Flow;

                resi.NodeList[e.SourceNode.ID].Add(newEdge);
            }

            
            Console.WriteLine("Flow Edges count: {0}", FEdges.Count);

            foreach (Edge e in FEdges)
            {
                Edge newReverseEdge = new Edge(resi.NodeList[e.TargetNode.ID], resi.NodeList[e.SourceNode.ID], 0.0, e.Flow);
                newReverseEdge.Flow = 0;

                resi.NodeList[e.TargetNode.ID].Add(newReverseEdge);
            }

            return resi;
        }

        #endregion
    }
}
 