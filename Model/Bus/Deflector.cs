using ScalingSpoon.Model.Enums;

namespace ScalingSpoon.Model.Bus
{
    public class Deflector
    {
        public int RobotID;
        public DeflectorType DeflectorType;

        public Deflector(int robotId, DeflectorType type)
        {
            RobotID = robotId;
            DeflectorType = type;
        }

        public Direction GetNewDirection(Direction fromDirection)
        {
            if (this.DeflectorType == DeflectorType.Forward)
            {
                if (fromDirection == Direction.Up) return Direction.Right;
                if (fromDirection == Direction.Right) return Direction.Up;
                if (fromDirection == Direction.Down) return Direction.Left;
                if (fromDirection == Direction.Left) return Direction.Down;
            }
            else
            {
                if (fromDirection == Direction.Up) return Direction.Left;
                if (fromDirection == Direction.Left) return Direction.Up;
                if (fromDirection == Direction.Down) return Direction.Right;
                if (fromDirection == Direction.Right) return Direction.Down;
            }

            return Direction.Up;
        }
    }
}
