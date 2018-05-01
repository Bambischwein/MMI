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

		public IList<Node> Prim()
		{
			IList<Node> primTree = new List<Node> ();
			primTree.Add(NodeList.FirstOrDefault());
			primTree.First ().IsVisited = true;	

			Queue<Node> q = new Queue<Node> ();

			double[] pi = new double[NodeList.Count()];
			double[] adj = new double[NodeList.Count()];
			double[] wert = new double[NodeList.Count()];

			for (int a = 0; a <= NodeList.Count(); a++)
			{
				pi[a] = 0;
				wert[a] = double.NaN;
			}


			double weight = double.NaN;
			foreach (Edge edge in primTree.First().Edges) 
			{
				if (edge.Weight > weight) 
				{
					weight = edge.Weight;
				}
			}



			return primTree;
		}
		#endregion
	}
}