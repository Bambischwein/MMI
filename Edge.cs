using System;

namespace MMITest
{
	public class Edge
	{
		public double Weight{ get; set; }
		public bool Visited{ get; set; }
		public Vertices SrcNode{ get; set; }
		public Vertices TrgNode{ get; set; }

		public Edge (Vertices srcNode, Vertices trgNode, double weight, Boolean visited)
		{
			SrcNode = srcNode;
			TrgNode = trgNode;

		}
	}
}

