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

            for (int i = 0; i < robots; i ++)
            {
                int xLoc = rand.Next(xLength);
                int yLoc = rand.Next(yLength);
                while(this.RobotCurrentLocations.Any(loc => loc.Value.X == xLoc && loc.Value.Y == yLoc))
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
                    if ((x >= 6 && x <= 8) || (y >= 6 && y <= 8))
                        continue;

                    possibleWinningDestinations.Add(this.Board[x, y]);
                }
            }

            int r;
            Cell c;

            //Top edges
            r = rand.Next(1, yLength / 2 - 1);
            c = this.Board[0, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[0, r + 1], ref possibleWinningDestinations);


            r = rand.Next(yLength / 2 + 1, yLength - 1);
            c = this.Board[0, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[0, r + 1], ref possibleWinningDestinations);

            //Bottom edges
            r = rand.Next(1, yLength / 2 - 1);
            c = this.Board[xLength - 1, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[xLength - 1, r + 1], ref possibleWinningDestinations);

            r = rand.Next(yLength / 2 + 1, yLength - 1);
            c = this.Board[xLength - 1, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[xLength - 1, r + 1], ref possibleWinningDestinations);

            //Left edges
            r = rand.Next(1, xLength / 2 - 1);
            c = this.Board[r, 0];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, 0], ref possibleWinningDestinations);

            r = rand.Next(xLength / 2 + 1, xLength - 1);
            c = this.Board[r, 0];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, 0], ref possibleWinningDestinations);

            //Right edges
            r = rand.Next(1, xLength / 2 - 1);
            c = this.Board[r, yLength - 1];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, yLength - 1], ref possibleWinningDestinations);

            r = rand.Next(xLength / 2 + 1, xLength - 1);
            c = this.Board[r, yLength - 1];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, ref possibleWinningDestinations);
            RemoveCells(this.Board[r - 1, yLength - 1], ref possibleWinningDestinations);

            //Middle 2x2
            CreateCellWall(this.Board[7, 7], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[7, 8], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[8, 7], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[8, 8], Direction.Up, Direction.Right, Direction.Left, Direction.Down);

            for (int i = 0; i < destinations; i++)
            {
                c = possibleWinningDestinations[rand.Next(possibleWinningDestinations.Count)];
                RemoveCells(c, ref possibleWinningDestinations);
                DestinationCell dc = new DestinationCell(c);
                this.Board[c.X, c.Y] = dc;
                this.WinningDestinations.Add(dc);
                r = rand.Next(4);
                switch (r)
                {
                    case 0:
                        CreateCellWall(dc, Direction.Up, Direction.Right);
                        break;
                    case 1:
                        CreateCellWall(dc, Direction.Right, Direction.Down);
                        break;
                    case 2:
                        CreateCellWall(dc, Direction.Down, Direction.Left);
                        break;
                    case 3:
                        CreateCellWall(dc, Direction.Left, Direction.Up);
                        break;
                }
                dc.WinningRobotId = rand.Next(_robotID);
            }

            WriteBoardToConsole();
        }

        //Console is not very friendly at all for printing like this.
        // I just wanted to visible see if my CreateBoard() is working somewhat properly,
        // without having to implement a View.
        private void WriteBoardToConsole()
        {
            for (int x = 0; x < this.Board.GetLength(0); x++)
            {
                for (int y = 0; y < this.Board.GetLength(1); y++)
                {
                    Cell c = this.Board[x, y];
                    if (c.HasSouthWall && c.HasEastWall)
                        Console.WriteLine("_|");
                    else if (c.HasSouthWall)
                        Console.Write("__");
                    else if (c.HasEastWall)
                        Console.Write(" |");

                    if (!c.HasSouthWall && !c.HasEastWall)
                        Console.Write("  ");
                }
                Console.Write(Environment.NewLine);
            }
        }

        private void RemoveCells(Cell cellToRemove, ref List<Cell> cells)
        {
            cells.Remove(cellToRemove);
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X - 1 && c.Y == cellToRemove.Y));
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X + 1 && c.Y == cellToRemove.Y));
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X && c.Y == cellToRemove.Y - 1));
            cells.Remove(cells.FirstOrDefault(c => c.X == cellToRemove.X && c.Y == cellToRemove.Y + 1));
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
    }
}
