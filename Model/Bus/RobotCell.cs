namespace ScalingSpoon.Model.Bus
{
    public class RobotCell
    {
        public Robot Robot { get; set; }
        public Cell Cell { get; set; }

        public RobotCell(Robot r, Cell c)
        {
            Robot = r;
            Cell = c;
        }
    }
}
