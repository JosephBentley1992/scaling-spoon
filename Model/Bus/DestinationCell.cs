using ScalingSpoon.Model.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace ScalingSpoon.Model.Bus
{
    [DebuggerDisplay("({X},{Y}) - {Walls}")]
    public class DestinationCell : Cell
    {
        public int WinningRobotId { get; set; }
        public Dictionary<int, Cell> RobotInitialLocations { get; set; }
        public Dictionary<int, Cell> RobotFinalLocations { get; set; }
        public Stack<RobotMove> MoveHistory { get; set; }
        public Stack<RobotMove> PoppedHistory { get; set; }
        public bool CurrentWinningCell { get; set; }

        public DestinationCell()
        {
            this.RobotInitialLocations = new Dictionary<int, Cell>();
            this.RobotFinalLocations = new Dictionary<int, Cell>();
            this.MoveHistory = new Stack<RobotMove>();
            this.PoppedHistory = new Stack<RobotMove>();
        }

        public DestinationCell(int id)
        {
            Id = id;
        }

        public DestinationCell(int id, CellWalls walls, int x, int y)
            : base(id, walls, x, y)
        {

        }

        public DestinationCell(Cell c)
        {
            this.Id = c.Id;
            this.Walls = c.Walls;
            this.X = c.X;
            this.Y = c.Y;
            this.RobotID = c.RobotID;
            this.RobotInitialLocations = new Dictionary<int, Cell>();
            this.RobotFinalLocations = new Dictionary<int, Cell>();
            this.MoveHistory = new Stack<RobotMove>();
            this.PoppedHistory = new Stack<RobotMove>();
        }

        public int NumberOfMoves()
        {
            return MoveHistory.Count + PoppedHistory.Count;
        }
    }
}
