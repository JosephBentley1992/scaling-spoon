using System;
namespace ScalingSpoon.Model.Enums
{
    [Flags]
    public enum CellWalls
    {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Right = 1 << 2,
        Left = 1 << 3,
        UpAndRight = Up | Right,
        RightAndDown = Right | Down,
        DownAndLeft = Down | Left,
        LeftAndUp = Left | Up,
        All = Up | Right | Down | Left
    }
}
