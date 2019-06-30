using ScalingSpoon.Model.Enums;
using System.Diagnostics;

namespace ScalingSpoon.Model.Bus
{
    [DebuggerDisplay("({X},{Y},{RobotID}) - {Walls}")]
    public class Cell
    {
        public int Id { get; set; }
        public CellWalls Walls { get; set; } = CellWalls.None;
        public Deflector Deflector { get; set; } = null;
        public Portal Portal { get; set; } = null;
        public int X { get; set; }
        public int Y { get; set; }
        public int RobotID { get; set; } = -1;
        public int RobotPath { get; set; } = 0;
        public Cell()
        {
        }

        public Cell(int id)
        {
            Id = id;
        }
        
        public Cell(int id, CellWalls walls, int x, int y)
        {
            Id = id;
            Walls = walls;
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return this.X == ((Cell)obj).X && this.Y == ((Cell)obj).Y;
        }

        public override int GetHashCode()
        {
            return this.X * this.Y * base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }

        public int GetQuadrant()
        {
            if (this.X <= 7 && this.Y <= 7)
                return 1;
            if (this.X <= 7 && this.Y >= 8)
                return 0;
            if (this.X >= 8 && this.Y <= 7)
                return 2;
            if (this.X >= 8 && this.Y >= 8)
                return 3;
            return 0;
        }
    }
}
