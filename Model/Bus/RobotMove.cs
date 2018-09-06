namespace ScalingSpoon.Model.Bus
{
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
    }
}
