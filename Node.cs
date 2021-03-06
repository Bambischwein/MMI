﻿using System;
using System.Collections.Generic;

namespace MMITest
{
	public class Node
	{
        #region Public Member

        public int ID{ get; set;}
		public Boolean IsVisited{ get; set; }
        public IList<Edge> Edges { get; set; }
        public int ComponentCount { get; set; }
		public double Balance { get; set; }
        public double BalanceModified { get; set; }
        public Node Antecessor { get; set; }
        public double Distance { get; set; }

        #endregion

        #region Konstruktor


        public Node()
        {
            IsVisited = false;
            Edges = new List<Edge>();
            ID = -1;
        }
        public Node (int id)
		{
            ID = id;
            IsVisited = false;
            Edges = new List<Edge>();
			ComponentCount = -1;
			Balance = 0.0;
            BalanceModified = 0.0;
		}

		#endregion

		#region Public Member

        public void Add(Edge edge)
        {
            Edges.Add(edge);
        }

		public void Remove(Edge edge)
		{
			Edges.Remove (edge);
		}

		public void AddReverse(Edge e)
		{
			Node src = e.TargetNode;
			Node trg = e.SourceNode;
			Edge newE = new Edge (src, trg, 0.0, 0.0);
			Edges.Add (newE);
		}			
			

        public void EToString()
        {
            Console.WriteLine("Node {0} with b={1} and b'={2}", ID,Balance,BalanceModified);

        }

        #endregion

    }
}