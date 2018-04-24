using System;
using System.Collections.Generic;

namespace MMITest
{
	public class Node
	{
        #region Public Member

        public int ID{ get; set;}
		public Boolean IsVisited{ get; set;}
        public IList<Edge> Edges { get; set; }
        public int BelongsToComponent { get; set; }
        #endregion

        #region Constructor

        public Node (int id)
		{
            ID = id;
            IsVisited = false;
            Edges = new List<Edge>();
			BelongsToComponent = -1;
		}

        public void Add(Edge edge)
        {
            Edges.Add(edge);
        }
        #endregion

    }
}