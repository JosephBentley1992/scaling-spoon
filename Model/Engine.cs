using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;

namespace ScalingSpoon.Model
{
    /// <summary>
    /// I feel liks I have the minimum requirements of a solution for the Model, with some of the game management being handled by the Controller.
    /// When a Move reaches the destination, the Model can update the new winning destination, but its going to be up to the Controller to present that knowledge to the View.
    /// So does the Model really need to return "You reached a destination! Congratulations!" or should that just be inferred from the fact that a new winning destination was set?
    /// </summary>
    public partial class Engine
    {
        public Cell[,] Board { get; set; }
        public Dictionary<int, Cell> RobotInitialLocations { get; set; }
        public Dictionary<int, Cell> RobotCurrentLocations { get; set; }
        public List<DestinationCell> WinningDestinations { get; set; }
        public DestinationCell CurrentWinningDestination { get; set; }
        private int _robotID = 0;
        public bool AutoSetNextWinningDestination { get; set; } = true;
        public bool AutoSetRobotPath { get; set; } = false;

        public Engine()
        {
            this.RobotInitialLocations = new Dictionary<int, Cell>();
            this.RobotCurrentLocations = new Dictionary<int, Cell>();
            this.WinningDestinations = new List<DestinationCell>();
            this.CurrentWinningDestination = new DestinationCell();
        }

        public void CreateWinningDestination(int x, int y, int robotId, bool currentWinningCell, params Direction[] directions)
        {
            DestinationCell dc = new DestinationCell(this.Board[x, y]);
            dc.WinningRobotId = robotId;
            dc.CurrentWinningCell = currentWinningCell;
            if (currentWinningCell)
                this.CurrentWinningDestination = dc;

            this.WinningDestinations.Add(dc);
            this.CreateCellWall(this.Board[x, y], directions);
        }

        public void CreateCellWall(Cell c, params Direction[] directions)
        {
            CreateCellWalls(c, true, directions);
        }

        public void RemoveCellWall(Cell c, params Direction[] directions)
        {
            CreateCellWalls(c, false, directions);
        }

        //Helper - Sets the current cells wall, and the adjacent cell.
        private void CreateCellWalls(Cell c, bool setWall, params Direction[] directions)
        {
            Cell adj = new Cell();
            foreach (Direction d in directions)
            {
                int xDiff = d == Direction.Up ? -1 : d == Direction.Down ? 1 : 0;
                int yDiff = d == Direction.Left ? -1 : d == Direction.Right ? 1 : 0;
                int x = c.X + xDiff;
                int y = c.Y + yDiff;
                if (setWall)
                    c.Walls = (CellWalls)((int)c.Walls | (int)d);
                else
                    c.Walls = (CellWalls)((int)c.Walls ^ (int)d);

                if ((xDiff != 0 && (c.X == 0 || c.X == this.Board.GetLength(0) - 1)) ||
                    (yDiff != 0 && (c.Y == 0 || c.Y == this.Board.GetLength(1) - 1)))
                    continue;

                adj = this.Board[x, y];

                //uhhhh
                if (setWall)
                {
                    if (xDiff - yDiff == -1)
                        adj.Walls = (CellWalls)((int)adj.Walls | (int)d << (xDiff - yDiff) * -1);
                    else
                        adj.Walls = (CellWalls)((int)adj.Walls | (int)d >> (xDiff - yDiff));
                }
                else
                {
                    if (xDiff - yDiff == -1)
                        adj.Walls = (CellWalls)((int)adj.Walls & ~(int)d << (xDiff - yDiff) * -1);
                    else
                        adj.Walls = (CellWalls)((int)adj.Walls & ~(int)d >> (xDiff - yDiff));
                }
            }
        }

        public Robot CreateRobot(int x, int y)
        {
            //TODO: 
            //  Validation / Unit Tests:
            //  Can't create a robot on the same Cell as another robot
            //  Can't create a robot out of bounds of Board
            //  Can't create a robot in a cell that is surrounded by walls (middle 2x2)
            //  Can't create a robot after the game has started
            Robot r = new Robot(_robotID++);
            this.RobotInitialLocations.Add(r.Id, this.Board[x, y]);
            this.RobotCurrentLocations.Add(r.Id, this.Board[x, y]);
            this.Board[x, y].RobotID = r.Id;
            return r;
        }

        public List<RobotMove> MoveRobot(int robot, params Direction[] directions)
        {
            List<RobotMove> moves = new List<RobotMove>();
            foreach (Direction d in directions)
            {
                RobotMove move = MoveRobotHelper(robot, d);
                if (move != null)
                    moves.Add(move);
            }

            if (AutoSetNextWinningDestination)
                SetNextWinningDestination();

            return moves;
        }

        private RobotMove MoveRobotHelper(int robot, Direction d)
        {
            RobotMove move = null;
            Cell initialLoc = this.RobotCurrentLocations[robot];
            Cell currentLoc = initialLoc;
            Cell nextLoc = null;

            if (initialLoc.Walls.HasFlag(GetCellWallFromDirection(d)) || OnEdgeOfBoard(initialLoc, d))
                return null;

            Direction temp = d;
            while (move == null)
            {
                if (OnEdgeOfBoard(currentLoc, temp))
                {
                    move = new RobotMove(robot, initialLoc, currentLoc, d);
                    continue;
                }

                nextLoc = GetAdjacentCell(currentLoc, temp);
                if (nextLoc.Walls.HasFlag(GetCellWallFromDirection(GetOppositeDirection(temp))) || nextLoc.RobotID != -1)
                {
                    move = new RobotMove(robot, initialLoc, currentLoc, d);
                    continue;
                }

                SetRobotPath(robot, currentLoc, nextLoc, temp);

                if (nextLoc.Deflector != null && nextLoc.Deflector.RobotID != robot)
                    temp = nextLoc.Deflector.GetNewDirection(temp);

                if (nextLoc.Portal != null && nextLoc.Portal.RobotID != robot)
                    nextLoc = nextLoc.Portal.Exit;

                currentLoc = nextLoc;
                continue;
            }

            //Update the Robot position
            this.UpdateRobotPosition(robot, initialLoc, currentLoc);
            this.CurrentWinningDestination.MoveHistory.Push(move);

            return move;
        }

        private bool OnEdgeOfBoard(Cell c, Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    return c.X == 0;
                case Direction.Down:
                    return c.X == Board.GetLength(0) - 1;
                case Direction.Left:
                    return c.Y == 0;
                case Direction.Right:
                    return c.Y == Board.GetLength(1) - 1;
            }
            return false;
        }

        private Cell GetAdjacentCell(Cell c, Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    return c.X == 0 ? null : Board[c.X - 1, c.Y];
                case Direction.Down:
                    return c.X == Board.GetLength(0) - 1 ? null : Board[c.X + 1, c.Y];
                case Direction.Left:
                    return c.Y == 0 ? null : Board[c.X, c.Y - 1];
                case Direction.Right:
                    return c.Y == Board.GetLength(1) - 1 ? null : Board[c.X, c.Y + 1];
            }
            return null;
        }

        private CellWalls GetCellWallFromDirection(Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    return CellWalls.Up;
                case Direction.Down:
                    return CellWalls.Down;
                case Direction.Left:
                    return CellWalls.Left;
                case Direction.Right:
                    return CellWalls.Right;
            }
            return CellWalls.None;
        }

        /// <summary>
        /// To draw a path on the board, we need to know the direction each robot enters and leaves every cell during a solution.
        /// We need to pass all of this information to the UI so the UI can draw it, without doing its own calculations of what cells need to be updated.
        /// 
        /// Each cell can be entered and left by each robot in each direction, so up down right left x 4 robots = 16bits.
        /// </summary>
        /// <param name="robotId"></param>
        /// <param name="currentLoc"></param>
        /// <param name="nextLoc"></param>
        /// <param name="d"></param>
        private void SetRobotPath(int robotId, Cell currentLoc, Cell nextLoc, Direction d)
        {
            if (!AutoSetRobotPath)
                return;

            currentLoc.RobotPath = currentLoc.RobotPath | ((int)d << robotId * 4);
            nextLoc.RobotPath = nextLoc.RobotPath | ((int)GetOppositeDirection(d) << robotId * 4);
        }

        private Direction GetOppositeDirection(Direction d)
        {
            switch (d)
            {
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
            }
            return Direction.Up;
        }

        public void SetNextWinningDestination()
        {
            if (this.AtWinningDestiation())
            {
                CurrentWinningDestination.CurrentWinningCell = false;
                CurrentWinningDestination = WinningDestinations[WinningDestinations.IndexOf(CurrentWinningDestination) + 1 == WinningDestinations.Count ? 0 : WinningDestinations.IndexOf(CurrentWinningDestination) + 1];
                CurrentWinningDestination.CurrentWinningCell = true;
            }
        }

        public void UndoMove()
        {
            int index = this.WinningDestinations.IndexOf(this.CurrentWinningDestination);

            //Start of game, nothing to undo.
            if (index == 0 && this.CurrentWinningDestination.MoveHistory.Count == 0)
                return;

            if (this.CurrentWinningDestination.MoveHistory.Count == 0)
            {
                this.CurrentWinningDestination.CurrentWinningCell = false;
                this.CurrentWinningDestination.RobotID = -1;
                this.CurrentWinningDestination = this.WinningDestinations[index - 1];
                this.CurrentWinningDestination.CurrentWinningCell = true;
            }

            //Pop a move from the history
            RobotMove move = this.CurrentWinningDestination.MoveHistory.Pop();
            this.CurrentWinningDestination.PoppedHistory.Push(move);

            //Update the Robot position
            this.UpdateRobotPosition(move.RobotId, move.EndingCell, move.StartingCell);
        }

        public void RedoMove()
        {
            //int index = this.WinningDestinations.IndexOf(this.CurrentWinningDestination);
            //
            //if (this.CurrentWinningDestination.PoppedHistory.Count == 0 && this.CurrentWinningDestination.MoveHistory.Count > 0)
            //{
            //    this.CurrentWinningDestination.CurrentWinningCell = false;
            //    this.CurrentWinningDestination = this.WinningDestinations[index - 1];
            //    this.CurrentWinningDestination.CurrentWinningCell = true;
            //}
            //
            ////Pop a move from the history
            //RobotMove move = this.CurrentWinningDestination.MoveHistory.Pop();
            //this.CurrentWinningDestination.PoppedHistory.Push(move);
            //
            ////Remove the robot from the ending point of the move.
            //DestinationCell dc = this.WinningDestinations.FirstOrDefault(d => d.X == move.EndingCell.X && d.Y == move.EndingCell.Y);
            //if (dc != null)
            //    dc.RobotID = -1;
            //move.EndingCell.RobotID = -1;
            //
            ////Set the robot to the starting point of the move.
            //move.StartingCell.RobotID = move.RobotId;
            //dc = this.WinningDestinations.FirstOrDefault(d => d.X == move.StartingCell.X && d.Y == move.StartingCell.Y);
            //if (dc != null)
            //    dc.RobotID = move.RobotId;
            //
            ////Set the RobotCurrentLocation cell to the StartingCell reference.
            //this.RobotCurrentLocations[move.RobotId] = move.StartingCell;
        }

        public void UpdateRobotPosition(int robotId, Cell initialLoc, Cell newLoc)
        {
            Cell initialLocToUpdate = this.Board[initialLoc.X, initialLoc.Y];
            Cell newLocToUpdate = this.Board[newLoc.X, newLoc.Y];

            initialLocToUpdate.RobotID = -1;
            newLocToUpdate.RobotID = robotId;
            DestinationCell dc = this.WinningDestinations.FirstOrDefault(wd => wd.X == initialLoc.X && wd.Y == initialLoc.Y);
            if (dc != null)
                dc.RobotID = -1;

            dc = this.WinningDestinations.FirstOrDefault(wd => wd.X == newLoc.X && wd.Y == newLoc.Y);
            if (dc != null)
                dc.RobotID = robotId;

            this.RobotCurrentLocations[robotId] = newLocToUpdate;
        }

        public void ResetCurrentWinningPosition()
        {
            int loopCount = this.CurrentWinningDestination.MoveHistory.Count;
            for (int i = 0; i < loopCount; i++)
                this.UndoMove();
        }

        public bool AtWinningDestiation()
        {
            return WinningDestinations.Count > 0 && this.Board[CurrentWinningDestination.X, CurrentWinningDestination.Y].RobotID == CurrentWinningDestination.WinningRobotId;
        }
    }
}
