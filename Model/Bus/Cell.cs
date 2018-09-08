using System.Diagnostics;

namespace ScalingSpoon.Model.Bus
{
    [DebuggerDisplay("({X},{Y},{RobotID}) - {HasNorthWall} {HasEastWall} {HasSouthWall} {HasWestWall}")]
    public class Cell
    {
        public int Id { get; set; }
        public bool HasNorthWall { get; set; }
        public bool HasEastWall { get; set; }
        public bool HasSouthWall { get; set; }
        public bool HasWestWall { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int RobotID { get; set; } = -1;
        public Cell()
        {
        }

        public Cell(int id)
        {
            Id = id;
        }
        
        public Cell(int id, bool hasNorthWall, bool hasEastWall, bool hasSouthWall, bool hasWestWall, int x, int y)
        {
            Id = id;
            HasNorthWall = hasNorthWall;
            HasEastWall = hasEastWall;
            HasSouthWall = hasSouthWall;
            HasWestWall = hasWestWall;
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
    }
}
