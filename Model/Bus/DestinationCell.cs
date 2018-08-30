using System.Collections.Generic;
using System.Diagnostics;

namespace ScalingSpoon.Model.Bus
{
    [DebuggerDisplay("({X},{Y}) - {HasNorthWall} {HasEastWall} {HasSouthWall} {HasWestWall}")]
    public class DestinationCell : Cell
    {
        public int WinningRobotId { get; set; }
        public Dictionary<int, Cell> RobotInitialLocations { get; set; }
        public Dictionary<int, Cell> RobotFinalLocations { get; set; }
        public Stack<RobotMove> MoveHistory { get; set; }
        public Queue<RobotMove> PoppedHistory { get; set; }
        public bool CurrentWinningCell { get; set; }

        public DestinationCell()
        {
            this.RobotInitialLocations = new Dictionary<int, Cell>();
            this.RobotFinalLocations = new Dictionary<int, Cell>();
            this.MoveHistory = new Stack<RobotMove>();
            this.PoppedHistory = new Queue<RobotMove>();
        }

        public DestinationCell(int id)
        {
            Id = id;
        }

        public DestinationCell(int id, bool hasNorthWall, bool hasEastWall, bool hasSouthWall, bool hasWestWall, int x, int y)
            : base(id, hasNorthWall, hasEastWall, hasSouthWall, hasWestWall, x, y)
        {

        }

        public DestinationCell(Cell c)
        {
            this.Id = c.Id;
            this.HasNorthWall = c.HasNorthWall;
            this.HasEastWall = c.HasEastWall;
            this.HasSouthWall = c.HasSouthWall;
            this.HasWestWall = c.HasWestWall;
            this.X = c.X;
            this.Y = c.Y;
            this.RobotID = c.RobotID;
            this.RobotInitialLocations = new Dictionary<int, Cell>();
            this.RobotFinalLocations = new Dictionary<int, Cell>();
            this.MoveHistory = new Stack<RobotMove>();
            this.PoppedHistory = new Queue<RobotMove>();
        }

        public int NumberOfMoves()
        {
            return MoveHistory.Count + PoppedHistory.Count;
        }
    }
}
