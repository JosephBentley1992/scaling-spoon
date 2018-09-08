using ScalingSpoon.Model.Enums;
using System.Diagnostics;

namespace ScalingSpoon.Model.Bus
{
    [DebuggerDisplay("{RobotId} ({StartingCell.X},{StartingCell.Y}) -> ({EndingCell.X},{EndingCell.Y})")]
    public class RobotMove
    {
        public int RobotId { get; set; }
        public Cell StartingCell { get; set; }
        public Cell EndingCell { get; set; }

        public RobotMove(int robot)
        {
            RobotId = robot;
        }

        public RobotMove(int robot, Cell start, Cell end)
        {
            RobotId = robot;
            StartingCell = start;
            EndingCell = end;
        }

        public override bool Equals(object obj)
        {
            return this.RobotId == ((RobotMove)obj).RobotId
                && this.StartingCell.Equals(((RobotMove)obj).StartingCell)
                && this.EndingCell.Equals(((RobotMove)obj).EndingCell);
        }

        public override int GetHashCode()
        {
            return this.StartingCell.X * this.StartingCell.Y * this.EndingCell.X * this.EndingCell.Y & base.GetHashCode();
        }

        public Direction GetDirection()
        {
            if (StartingCell.X < EndingCell.X)
                return Direction.Down;
            else if (StartingCell.X > EndingCell.X)
                return Direction.Up;
            else if (StartingCell.Y < EndingCell.Y)
                return Direction.Right;
            else
                return Direction.Left;
        }
    }
}
