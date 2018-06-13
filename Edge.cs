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
        public double Flow { get; set; }
        public double Capacity { get; set; }

        #endregion

        #region Constructor

        public Edge (Node sourceNode, Node targetNode)
		{
			SourceNode = sourceNode;
            TargetNode = targetNode;
            Weight = 1.0;
            Visited = false;
            Flow = 0.0;
            Capacity = Weight;
		}

        public Edge(Node sourceNode, Node targetNode, double weight)
            :this(sourceNode, targetNode)
        {
            Weight = weight;
            Capacity = weight;
        }

        public Edge(Node sourceNode, Node targetNode, double weight, double capacity)
            :this(sourceNode, targetNode, weight)
        {
            Capacity = capacity;
        }

        public double ResidualCapacity
        {
            get 
			{
				return Capacity - Flow; 
			}
        }

        #endregion
    }
}

