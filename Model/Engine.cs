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
    /// When a Move reaches the destination, the Model can update the new winning destination, but its going to be up to the Controller to present that knowledge to the View..
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
                    this.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            Random rand = new Random();
            int xLength = this.Board.GetLength(0);
            int yLength = this.Board.GetLength(1);

            for (int i = 0; i < robots; i++)
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
            for (int i = 0; i < destinations; i++)
            {
                bool destinationAssigned = false;
                while (!destinationAssigned)
                {
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
                    while (triedWalls.Count > 0 && (!(dc.HasNorthWall || dc.HasEastWall || dc.HasSouthWall || dc.HasWestWall)))
                    {
                        r = rand.Next(4);
                        switch (r)
                        {
                            case 0:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].HasNorthWall || this.Board[dc.X, dc.Y - 2].HasNorthWall))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].HasEastWall || this.Board[dc.X - 2, dc.Y].HasEastWall)))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Up, Direction.Right);
                                break;
                            case 1:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].HasSouthWall || this.Board[dc.X, dc.Y - 2].HasSouthWall))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].HasEastWall || this.Board[dc.X - 2, dc.Y].HasEastWall)))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Right, Direction.Down);
                                break;
                            case 2:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].HasSouthWall || this.Board[dc.X, dc.Y - 2].HasSouthWall))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].HasWestWall || this.Board[dc.X - 2, dc.Y].HasWestWall)))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Down, Direction.Left);
                                break;
                            case 3:
                                if (((dc.Y + 2 <= yLength - 1 && dc.Y - 2 >= 0) && (this.Board[dc.X, dc.Y + 2].HasNorthWall || this.Board[dc.X, dc.Y - 2].HasNorthWall))
                                    || ((dc.X + 2 <= xLength - 1 && dc.X - 2 >= 0) && (this.Board[dc.X + 2, dc.Y].HasWestWall || this.Board[dc.X - 2, dc.Y].HasWestWall)))
                                {
                                    if (triedWalls.Contains(r))
                                        triedWalls.Remove(r);
                                    break;
                                }

                                CreateCellWall(dc, Direction.Left, Direction.Up);
                                break;
                        }
                    }

                    //None of the 4 L walls work for this position. Remove it, and try another cell.
                    if (!(dc.HasNorthWall || dc.HasEastWall || dc.HasSouthWall || dc.HasWestWall))
                    {
                        possibleWinningDestinations.Remove(dc);
                    }
                    else
                    {
                        this.Board[c.X, c.Y] = dc;
                        this.WinningDestinations.Add(dc);
                        dc.WinningRobotId = rand.Next(_robotID);
                        RemoveCells(c, true, ref possibleWinningDestinations);
                        destinationAssigned = true;
                    }
                }
            }

            CurrentWinningDestination = WinningDestinations[0];
            CurrentWinningDestination.CurrentWinningCell = true;
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

        //Helper - Sets the current cells wall, and the adjacent cell.
        public void CreateCellWall(Cell c, params Direction[] directions)
        {
            Cell adj = new Cell();
            foreach (Direction d in directions)
            {
                switch (d)
                {
                    case Direction.Up:
                        if (c.X == 0)
                            break;
                        adj = this.Board[c.X - 1, c.Y];
                        c.HasNorthWall = true;
                        adj.HasSouthWall = true;
                        break;
                    case Direction.Down:
                        if (c.X == this.Board.GetLength(0) - 1)
                            break;
                        adj = this.Board[c.X + 1, c.Y];
                        c.HasSouthWall = true;
                        adj.HasNorthWall = true;
                        break;
                    case Direction.Left:
                        if (c.Y == 0)
                            break;
                        adj = this.Board[c.X, c.Y - 1];
                        c.HasWestWall = true;
                        adj.HasEastWall = true;
                        break;
                    case Direction.Right:
                        if (c.Y == this.Board.GetLength(1) - 1)
                            break;
                        adj = this.Board[c.X, c.Y + 1];
                        c.HasEastWall = true;
                        adj.HasWestWall = true;
                        break;
                }
            }
        }

        public Robot CreateRobot(int x, int y)
        {
            //TODO: Can't create a robot on the same Cell
            //  Can't create a robot out of bounds of Board
            //  Can't create a robot in a cell that is surrounded by walls (middle 2x2)
            Robot r = new Robot(_robotID++);
            this.RobotInitialLocations.Add(r.Id, this.Board[x, y]);
            this.RobotCurrentLocations.Add(r.Id, this.Board[x, y]);
            this.Board[x, y].RobotID = r.Id;
            return r;
        }

        public void MoveRobot(int robot, params Direction[] directions)
        {
            foreach (Direction d in directions)
            {
                switch (d)
                {
                    case Direction.Up:
                        MoveUp(robot);
                        break;
                    case Direction.Right:
                        MoveRight(robot);
                        break;
                    case Direction.Down:
                        MoveDown(robot);
                        break;
                    case Direction.Left:
                        MoveLeft(robot);
                        break;
                }
            }

            if (CurrentWinningDestination.RobotID == CurrentWinningDestination.WinningRobotId)
            {
                CurrentWinningDestination.CurrentWinningCell = false;
                CurrentWinningDestination = WinningDestinations[WinningDestinations.IndexOf(CurrentWinningDestination) + 1 == WinningDestinations.Count ? 0 : WinningDestinations.IndexOf(CurrentWinningDestination) + 1];
                CurrentWinningDestination.CurrentWinningCell = true;
            }
        }

        private void MoveUp(int robot)
        {
            Cell initialLoc = this.RobotCurrentLocations[robot];
            if (initialLoc.HasNorthWall || initialLoc.X == 0)
                return;

            Cell currentLoc = initialLoc;
            Cell nextLoc;
            for (int i = initialLoc.X - 1; i >= 0; i--)
            {
                nextLoc = this.Board[i, initialLoc.Y];
                if (nextLoc.HasSouthWall || nextLoc.RobotID != -1)
                    break;

                currentLoc = nextLoc;
            }

            initialLoc.RobotID = -1;
            currentLoc.RobotID = robot;
            this.CurrentWinningDestination.MoveHistory.Push(new RobotMove(robot, initialLoc, currentLoc));
            this.RobotCurrentLocations[robot] = currentLoc;
        }

        private void MoveDown(int robot)
        {
            Cell initialLoc = this.RobotCurrentLocations[robot];
            if (initialLoc.HasSouthWall || initialLoc.X == Board.GetLength(0) - 1)
                return;

            Cell currentLoc = initialLoc;
            Cell nextLoc;
            for (int i = initialLoc.X + 1; i <= Board.GetLength(0) - 1; i++)
            {
                nextLoc = this.Board[i, initialLoc.Y];
                if (nextLoc.HasNorthWall || nextLoc.RobotID != -1)
                    break;

                currentLoc = nextLoc;
            }

            initialLoc.RobotID = -1;
            currentLoc.RobotID = robot;
            this.CurrentWinningDestination.MoveHistory.Push(new RobotMove(robot, initialLoc, currentLoc));
            this.RobotCurrentLocations[robot] = currentLoc;
        }

        private void MoveLeft(int robot)
        {
            Cell initialLoc = this.RobotCurrentLocations[robot];
            if (initialLoc.HasWestWall || initialLoc.Y == 0)
                return;

            Cell currentLoc = initialLoc;
            Cell nextLoc;
            for (int i = initialLoc.Y - 1; i >= 0; i--)
            {
                nextLoc = this.Board[initialLoc.X, i];
                if (nextLoc.HasEastWall || nextLoc.RobotID != -1)
                    break;

                currentLoc = nextLoc;
            }

            initialLoc.RobotID = -1;
            currentLoc.RobotID = robot;
            this.CurrentWinningDestination.MoveHistory.Push(new RobotMove(robot, initialLoc, currentLoc));
            this.RobotCurrentLocations[robot] = currentLoc;
        }

        private void MoveRight(int robot)
        {
            Cell initialLoc = this.RobotCurrentLocations[robot];
            if (initialLoc.HasEastWall || initialLoc.Y == Board.GetLength(1) - 1)
                return;

            Cell currentLoc = initialLoc;
            Cell nextLoc;
            for (int i = initialLoc.Y + 1; i <= Board.GetLength(1) - 1; i++)
            {
                nextLoc = this.Board[initialLoc.X, i];
                if (nextLoc.HasWestWall || nextLoc.RobotID != -1)
                    break;

                currentLoc = nextLoc;
            }

            initialLoc.RobotID = -1;
            currentLoc.RobotID = robot;
            this.CurrentWinningDestination.MoveHistory.Push(new RobotMove(robot, initialLoc, currentLoc));
            this.RobotCurrentLocations[robot] = currentLoc;
        }

        public void UndoMove()
        {
            //TODO: Figure out how to structure the difference between Undo'ing a move on the current puzzle, vs Undo'ing a historic puzzle.
        }

        #region SolveTheGame o.O

        public List<RobotMove> FindSolution()
        {
            MoveRobot(0, Direction.Right);
            return new List<RobotMove>() { new RobotMove(0, this.Board[2, 1], this.Board[2, 2]) };
            //return new List<RobotMove>();
        }
        #endregion SolveTheGame o.O
    }
}
