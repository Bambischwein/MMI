using System;

namespace MMITest
{
	public class Edge
    {
        #region Public Member

        public double Weight{ get; set; }
		public bool Visited{ get; set; }
		public Node SourceNode{ get; set; }
		public Node TargetNode { get; set; }

        #endregion

        #region Constructor

        public Edge (Node sourceNode, Node targetNode)
		{
			SourceNode = sourceNode;
            TargetNode = targetNode;
            Weight = 1.0;
            Visited = false;
		}

        public Edge(Node sourceNode, Node targetNode, double weight)
        {
            SourceNode = sourceNode;
            TargetNode = targetNode;
            Weight = weight;
            Visited = false;
        }
        #endregion
    }
}

