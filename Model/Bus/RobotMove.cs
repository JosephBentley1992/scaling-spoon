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
        public Direction Direction { get; set; }

        public RobotMove(int robot)
        {
            RobotId = robot;
        }

        public RobotMove(int robot, Cell start, Cell end, Direction d)
        {
            RobotId = robot;
            StartingCell = start;
            EndingCell = end;
            Direction = d;
        }

        public override bool Equals(object obj)
        {
            return this.RobotId == ((RobotMove)obj).RobotId
                && this.StartingCell.Equals(((RobotMove)obj).StartingCell)
                && this.EndingCell.Equals(((RobotMove)obj).EndingCell)
                && this.Direction == ((RobotMove)obj).Direction;
        }

        public override int GetHashCode()
        {
            return this.StartingCell.X * this.StartingCell.Y * this.EndingCell.X * this.EndingCell.Y * (int)this.Direction * base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} -> {2}", RobotId, StartingCell.ToString(), EndingCell.ToString());
        }
    }
}
