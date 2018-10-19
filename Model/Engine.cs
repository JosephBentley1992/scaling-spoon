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
    public class Engine
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

        /// <summary>
        /// Create a x by y board to play on.
        /// 16x16 preferred (only dimensions allowed for now?)
        /// Middle 2x2 are walls
        /// Walls are placed randomly in the four quadrants, but never touch the edges of the board
        /// Placed walls are always in an L shape, and never touch each other (adjust storage?)
        /// 2 additional walls extend out from the edge of the board per side, 2 in each quadrant of the board, never 1 away from a corner, and never touch L walls.
        /// Walls need to be known from both directions. A wall between two Cells needs to be able to have each Cell identify theres a wall there.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="destinations"></param>
        /// <param name="robots"></param>
        public void ConstructBoard(int xDimension, int yDimension, int destinations, int robots)
        {
            this.Board = new Cell[xDimension, yDimension];

            int id = 0;
            for (int x = 0; x <= this.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= this.Board.GetLength(1) - 1; y++)
                    this.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            Random rand = new Random();
            int xLength = this.Board.GetLength(0);
            int yLength = this.Board.GetLength(1);

            CreateRobot(0, 0);

            List<Cell> possibleWinningDestinations = new List<Cell>();
            for (int x = 1; x <= xLength - 2; x++)
            {
                for (int y = 1; y <= yLength - 2; y++)
                {
                    if ((x >= 6 && x <= 9) && (y >= 6 && y <= 9))
                        continue;

                    possibleWinningDestinations.Add(this.Board[x, y]);
                }
            }

            int r;
            Cell c;

            //Top edges
            r = rand.Next(2, yLength / 2 - 3);
            c = this.Board[0, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[0, r + 1], false, ref possibleWinningDestinations);


            r = rand.Next(yLength / 2 + 2, yLength - 3);
            c = this.Board[0, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[0, r + 1], false, ref possibleWinningDestinations);

            //Bottom edges
            r = rand.Next(2, yLength / 2 - 3);
            c = this.Board[xLength - 1, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[xLength - 1, r + 1], false, ref possibleWinningDestinations);

            r = rand.Next(yLength / 2 + 2, yLength - 3);
            c = this.Board[xLength - 1, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[xLength - 1, r + 1], false, ref possibleWinningDestinations);

            //Left edges
            r = rand.Next(2, xLength / 2 - 3);
            c = this.Board[r, 0];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, 0], false, ref possibleWinningDestinations);

            r = rand.Next(xLength / 2 + 2, xLength - 3);
            c = this.Board[r, 0];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, 0], false, ref possibleWinningDestinations);

            //Right edges
            r = rand.Next(2, xLength / 2 - 3);
            c = this.Board[r, yLength - 1];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, yLength - 1], false, ref possibleWinningDestinations);

            r = rand.Next(xLength / 2 + 2, xLength - 3);
            c = this.Board[r, yLength - 1];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, yLength - 1], false, ref possibleWinningDestinations);

            //Middle 2x2
            CreateCellWall(this.Board[7, 7], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[7, 8], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[8, 7], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[8, 8], Direction.Up, Direction.Right, Direction.Left, Direction.Down);

            List<int> availableRows = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            List<int> availableCols = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            List<List<int>> quadrants = new List<List<int>>()
            {
                new List<int> { 0, 1, 2, 3 },
                new List<int> { 0, 1, 2, 3 },
                new List<int> { 0, 1, 2, 3 },
                new List<int> { 0, 1, 2, 3 }
            };
            for (int i = 0; i < destinations; i++)
            {
                if (possibleWinningDestinations.Count == 0)
                    break;

                bool destinationAssigned = false;
                while (!destinationAssigned)
                {
                    if (possibleWinningDestinations.Count == 0)
                        break;

                    if (availableRows.Count != 0 && availableCols.Count != 0)
                    {
                        int x = availableRows[rand.Next(availableRows.Count)];
                        int y = availableCols[rand.Next(availableCols.Count)];
                        List<Cell> tempList = possibleWinningDestinations.Where(pwd => pwd.X == x || pwd.Y == y).ToList();
                        if (tempList.Count == 0)
                        {
                            c = possibleWinningDestinations[rand.Next(possibleWinningDestinations.Count)];
                        }
                        else
                        {
                            c = tempList[rand.Next(tempList.Count)];
                            if (c != null)
                            {
                                availableRows.Remove(x);
                                availableCols.Remove(y);
                            }
                        }
                    }
                    else
                    {
                        c = possibleWinningDestinations[rand.Next(possibleWinningDestinations.Count)];
                    }

                    DestinationCell dc = new DestinationCell(c);

                    //Create L shaped wall, but not an L shape that causes the following 1 space gap with an edge wall.
                    //   _  _
                    //  |    |
                    //  
                    //  |_  _|
                    //
                    List<int> triedWalls = new List<int> { 0, 1, 2, 3 };

                    //While (we have more walls to try) && (we haven't assigned walls to the destination)
                    while (triedWalls.Count > 0 && dc.Walls == CellWalls.None)
                    {
                        r = rand.Next(4);
                        switch (r)
                        {
                            case 0:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].Walls.HasFlag(CellWalls.Up) || this.Board[dc.X, dc.Y - 2].Walls.HasFlag(CellWalls.Up)))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].Walls.HasFlag(CellWalls.Right) || this.Board[dc.X - 2, dc.Y].Walls.HasFlag(CellWalls.Right)))
                                    || !quadrants[dc.GetQuadrant()].Contains(0))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Up, Direction.Right);
                                if (this.CheckForLoops(dc))
                                {
                                    RemoveCellWall(dc, Direction.Up, Direction.Right);
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }
                                else
                                {
                                    CreateCellWall(dc, Direction.Up, Direction.Right);
                                    quadrants[dc.GetQuadrant()].Remove(0);
                                }
                                break;
                            case 1:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].Walls.HasFlag(CellWalls.Down) || this.Board[dc.X, dc.Y - 2].Walls.HasFlag(CellWalls.Down)))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].Walls.HasFlag(CellWalls.Right) || this.Board[dc.X - 2, dc.Y].Walls.HasFlag(CellWalls.Right)))
                                    || !quadrants[dc.GetQuadrant()].Contains(1))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Right, Direction.Down);
                                if (this.CheckForLoops(dc))
                                {
                                    RemoveCellWall(dc, Direction.Right, Direction.Down);
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }
                                else
                                {
                                    CreateCellWall(dc, Direction.Right, Direction.Down);
                                    quadrants[dc.GetQuadrant()].Remove(1);
                                }
                                break;
                            case 2:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].Walls.HasFlag(CellWalls.Down) || this.Board[dc.X, dc.Y - 2].Walls.HasFlag(CellWalls.Down)))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].Walls.HasFlag(CellWalls.Left) || this.Board[dc.X - 2, dc.Y].Walls.HasFlag(CellWalls.Left)))
                                    || !quadrants[dc.GetQuadrant()].Contains(2))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Down, Direction.Left);
                                if (this.CheckForLoops(dc))
                                {
                                    RemoveCellWall(dc, Direction.Down, Direction.Left);
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }
                                else
                                {
                                    CreateCellWall(dc, Direction.Down, Direction.Left);
                                    quadrants[dc.GetQuadrant()].Remove(2);
                                }
                                break;
                            case 3:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].Walls.HasFlag(CellWalls.Up) || this.Board[dc.X, dc.Y - 2].Walls.HasFlag(CellWalls.Up)))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].Walls.HasFlag(CellWalls.Left) || this.Board[dc.X - 2, dc.Y].Walls.HasFlag(CellWalls.Left)))
                                    || !quadrants[dc.GetQuadrant()].Contains(3))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Left, Direction.Up);
                                if (this.CheckForLoops(dc))
                                {
                                    RemoveCellWall(dc, Direction.Left, Direction.Up);
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }
                                else
                                {
                                    CreateCellWall(dc, Direction.Left, Direction.Up);
                                    quadrants[dc.GetQuadrant()].Remove(3);
                                }
                                break;
                        }
                    }

                    //None of the 4 L walls work for this position. Remove it, and try another cell.
                    if (dc.Walls == CellWalls.None)
                    {
                        possibleWinningDestinations.Remove(dc);
                    }
                    else
                    {
                        int q = dc.GetQuadrant();
                        if (quadrants[q].Count == 0)
                        {
                            List<Cell> cellsToRemove = new List<Cell>();
                            foreach (Cell d in possibleWinningDestinations)
                                if (d.GetQuadrant() == q)
                                    cellsToRemove.Add(d);

                            foreach (Cell d in cellsToRemove)
                                possibleWinningDestinations.Remove(d);
                        }
                        this.Board[c.X, c.Y] = dc;
                        dc.WinningRobotId = rand.Next(robots);
                        RemoveCells(c, true, ref possibleWinningDestinations);
                        destinationAssigned = true;

                        //Checking for loops actually adds to the history... just clear it for now i guess.
                        dc.MoveHistory.Clear();
                        dc.PoppedHistory.Clear();
                        dc.CurrentWinningCell = false;

                        this.WinningDestinations.Add(dc);
                    }
                }
            }

            CurrentWinningDestination = WinningDestinations[0];
            CurrentWinningDestination.CurrentWinningCell = true;

            //Create the last x (3) robots
            for (int i = 1; i < robots; i++)
            {
                int xLoc = rand.Next(xLength);
                int yLoc = rand.Next(yLength);
                while (this.RobotCurrentLocations.Any(loc => loc.Value.X == xLoc && loc.Value.Y == yLoc)
                    || ((xLoc >= 6 && xLoc <= 9) || (yLoc >= 6 && yLoc <= 9)))
                {
                    xLoc = rand.Next(xLength);
                    yLoc = rand.Next(yLength);
                }

                CreateRobot(xLoc, yLoc);
            }

            foreach (DestinationCell dc in this.WinningDestinations)
            {
                dc.MoveHistory.Clear();
                dc.PoppedHistory.Clear();
            }

            foreach (Cell cT in this.Board)
                cT.RobotPath = 0;
        }

        private void RemoveCells(Cell cellToRemove, bool removeDiagonals, ref List<Cell> cells)
        {
            cells.Remove(cellToRemove);
            //Remove adjacent cells
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X - 1 && c.Y == cellToRemove.Y));
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X + 1 && c.Y == cellToRemove.Y));
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X && c.Y == cellToRemove.Y - 1));
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X && c.Y == cellToRemove.Y + 1));

            //Remove diagonal adjacent cells
            if (removeDiagonals)
            {
                cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X + 1 && c.Y == cellToRemove.Y + 1));
                cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X + 1 && c.Y == cellToRemove.Y - 1));
                cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X - 1 && c.Y == cellToRemove.Y + 1));
                cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X - 1 && c.Y == cellToRemove.Y - 1));
            }

            //Never have more than two winning cells in the same row or column.
            if (WinningDestinations.Count(c => c.X == cellToRemove.X) == 2)
            {
                List<Cell> cellsInSameRowToRemove = new List<Cell>();
                foreach (Cell cell in cells.Where(c => c.X == cellToRemove.X))
                    cellsInSameRowToRemove.Add(cell);
                foreach (Cell cell in cellsInSameRowToRemove)
                    cells.Remove(cell);
            }

            if (WinningDestinations.Count(c => c.Y == cellToRemove.Y) == 2)
            {
                List<Cell> cellsInSameColumnToRemove = new List<Cell>();
                foreach (Cell cell in cells.Where(c => c.Y == cellToRemove.Y))
                    cellsInSameColumnToRemove.Add(cell);
                foreach (Cell cell in cellsInSameColumnToRemove)
                    cells.Remove(cell);
            }
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

        private bool CheckForLoops(DestinationCell d)
        {
            List<DestinationCell> cellsToCheck = new List<DestinationCell>();
            cellsToCheck.AddRange(WinningDestinations);
            cellsToCheck.Add(d);

            foreach (DestinationCell dc in cellsToCheck)
            {
                int originalX = dc.X;
                int originalY = dc.Y;
                //Cell c = this.Board[dc.X, dc.Y];
                Dictionary<CellWalls, List<Direction>> loops = new Dictionary<CellWalls, List<Direction>>()
                {
                    {CellWalls.DownAndLeft, new List<Direction>() { Direction.Up, Direction.Right, Direction.Down, Direction.Left, Direction.Right, Direction.Up, Direction.Left, Direction.Down} },
                    {CellWalls.LeftAndUp, new List<Direction>() {Direction.Down, Direction.Right, Direction.Up, Direction.Left, Direction.Right, Direction.Down, Direction.Left, Direction.Up } },
                    {CellWalls.UpAndRight, new List<Direction>() {Direction.Left, Direction.Down, Direction.Right, Direction.Up, Direction.Down, Direction.Left, Direction.Up, Direction.Right } },
                    {CellWalls.RightAndDown, new List<Direction>() {Direction.Up, Direction.Left, Direction.Down, Direction.Right, Direction.Left, Direction.Up, Direction.Right, Direction.Down } }
                };

                int i = 0;
                for (int y = 0; y < 2; y++)
                {
                    this.UpdateRobotPosition(0, this.RobotCurrentLocations[0], dc);
                    this.MoveRobot(0, loops[dc.Walls][i++]);
                    if (this.MoveRobot(0, loops[dc.Walls][i++]).Count == 0)
                        continue;

                    if (this.MoveRobot(0, loops[dc.Walls][i++]).Count == 0)
                        continue;
                    this.MoveRobot(0, loops[dc.Walls][i++]);

                    if (this.RobotCurrentLocations[0].X == originalX && this.RobotCurrentLocations[0].Y == originalY)
                        return true;
                }
            }

            return false;
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
                    move = new RobotMove(robot, initialLoc, currentLoc);
                    continue;
                }

                nextLoc = GetAdjacentCell(currentLoc, temp);
                if (nextLoc.Walls.HasFlag(GetCellWallFromDirection(GetOppositeDirection(temp))) || nextLoc.RobotID != -1)
                {
                    move = new RobotMove(robot, initialLoc, currentLoc);
                    continue;
                }
                
                if (nextLoc.Deflector != null && nextLoc.Deflector.RobotID != robot)
                    temp = nextLoc.Deflector.GetNewDirection(temp);
                
                SetRobotPath(robot, currentLoc, nextLoc, d);
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
