using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingSpoon.Model.Bus
{
    public class Node
    {
        public int Depth { get; set; }
        public NodeData Data { get; set; }
        public Node Previous { get; set; }
        public List<Node> Next { get; set; }

        public Node()
        {
            this.Data = null;
            this.Depth = -1;
            this.Previous = null;
            this.Next = new List<Node>();
        }

        public Node(NodeData data)
        {
            this.Data = data;
            this.Depth = -1;
            this.Previous = null;
            this.Next = new List<Node>();
        }

        public Node(NodeData data, int depth)
        {
            this.Data = data;
            this.Depth = depth;
            this.Previous = null;
            this.Next = new List<Node>();
        }

        public Node(NodeData data, int depth, Node prev)
        {
            this.Data = data;
            this.Depth = depth;
            this.Previous = prev;
            this.Next = new List<Node>();
        }
    }
}
