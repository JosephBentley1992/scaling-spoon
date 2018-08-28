namespace ScalingSpoon.Model.Bus
{
    public class Cell
    {
        public int Id { get; set; }
        public bool HasNorthWall { get; set; }
        public bool HasEastWall { get; set; }
        public bool HasSouthWall { get; set; }
        public bool HasWestWall { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
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
    }
}
