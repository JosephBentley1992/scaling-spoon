namespace ScalingSpoon.Model.Bus
{
    public class RobotMove
    {
        public Robot Robot { get; set; }
        public Cell StartingCell { get; set; }
        public Cell EndingCell { get; set; }

        public RobotMove(Robot r)
        {
            Robot = r;
        }

        public RobotMove(Robot r, Cell start, Cell end)
        {
            Robot = r;
            StartingCell = start;
            EndingCell = end;
        }
    }
}
