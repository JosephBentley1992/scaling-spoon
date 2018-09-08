using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingSpoon.Model.Bus
{
    public class NodeData
    {
        public RobotMove Move { get; set; }
        public Dictionary<int, Cell> CurrentRobotLocations { get; set; }

        public NodeData()
        {
            this.Move = null;
            this.CurrentRobotLocations = new Dictionary<int, Cell>();
        }

        public NodeData(RobotMove move)
        {
            this.Move = move;
            this.CurrentRobotLocations = new Dictionary<int, Cell>();
        }

        public NodeData(RobotMove move, Dictionary<int,Cell> locs)
        {
            this.Move = move;
            this.CurrentRobotLocations = locs;
        }
    }
}
