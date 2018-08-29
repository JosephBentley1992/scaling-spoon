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
    }
}
