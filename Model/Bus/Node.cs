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

        /// <summary>
        /// Determining if a node exists needs to check the (x,Y) coordinates of each robot.
        /// For simplicities sake, we are going to assume exactly 4 robots on exactly a 16x16 game board.
        /// 
        /// Each x and y value can be between 0 and 15, exactly 4 bits worth of values.
        /// 8 bits per robot = 1byte
        /// 32bits = 4bytes = 1 int/uint.
        /// 
        /// We can store a single integer/uint to know the location of all 4 robots at any given time!
        /// If we index a dictionary based on this int, we can access very quickly whether or not a board position has been reached,
        /// and quickly query for the information without having to calculate the index every time.
        /// </summary>
        /// <returns></returns>
        public int GetIndex()
        {
            if (this.Data == null || this.Data.CurrentRobotLocations == null || this.Data.CurrentRobotLocations.Count == 0)
                return 0;

            int index = 0;
            for (int i = 0; i < this.Data.CurrentRobotLocations.Count; i++)
            {
                index = index | (this.Data.CurrentRobotLocations[i].X << (((i + 1) * 8) - 4));
                index = index | (this.Data.CurrentRobotLocations[i].Y << (i * 8));
            }
            return index;
        }
    }
}
