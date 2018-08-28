using System.Collections.Generic;

namespace ScalingSpoon.Model.Bus
{
    public class DestinationCell : Cell
    {
        public int WinningRobotId { get; set; }
        public Dictionary<int, Cell> RobotInitialLocations { get; set; }
        public Dictionary<int, Cell> RobotFinalLocations { get; set; }
        public Stack<RobotMove> MoveHistory { get; set; }
        public Queue<RobotMove> PoppedHistory { get; set; }

        public DestinationCell()
        {
        }

        public DestinationCell(int id)
        {
            Id = id;
        }

        public DestinationCell(int id, bool hasNorthWall, bool hasEastWall, bool hasSouthWall, bool hasWestWall, int x, int y)
            : base(id, hasNorthWall, hasEastWall, hasSouthWall, hasWestWall, x, y)
        {

        }

        public int NumberOfMoves()
        {
            return MoveHistory.Count + PoppedHistory.Count;
        }
    }
}
