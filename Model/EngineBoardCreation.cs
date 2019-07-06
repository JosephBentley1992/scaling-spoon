using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScalingSpoon.Model
{
    public partial class Engine
    {
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
        public void ConstructBoard(int xDimension, int yDimension, int destinations, int robots, int deflectors, int portals)
        {
            CreateBoardDefaults(xDimension, yDimension);

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

            CreateEdgeWalls(ref possibleWinningDestinations);
            CreateWinningDestinations(destinations, robots, ref possibleWinningDestinations);
            CreateDeflectors(deflectors);
            CreatePortals(portals);

            //Create the last x (3) robots
            for (int i = 1; i < robots; i++)
            {
                int xLoc = rand.Next(xLength);
                int yLoc = rand.Next(yLength);
                while (this.RobotCurrentLocations.Any(loc => loc.Value.X == xLoc && loc.Value.Y == yLoc)
                    || ((xLoc >= 6 && xLoc <= 9) || (yLoc >= 6 && yLoc <= 9))
                    || this.Board[xLoc, yLoc].Deflector != null
                    || this.Board[xLoc, yLoc].Portal != null)
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

        private void CreateBoardDefaults(int xDimension, int yDimension)
        {
            this.Board = new Cell[xDimension, yDimension];

            int id = 0;
            for (int x = 0; x <= this.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= this.Board.GetLength(1) - 1; y++)
                    this.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            //Middle 2x2
            CreateCellWall(this.Board[7, 7], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[7, 8], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[8, 7], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            CreateCellWall(this.Board[8, 8], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
        }

        private void CreateEdgeWalls(ref List<Cell> cells)
        {
            Random rand = new Random();
            int xLength = this.Board.GetLength(0);
            int yLength = this.Board.GetLength(1);

            int r;
            Cell c;

            //Top edges
            r = rand.Next(2, yLength / 2 - 3);
            c = this.Board[0, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[0, r + 1], false, ref cells);


            r = rand.Next(yLength / 2 + 2, yLength - 3);
            c = this.Board[0, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[0, r + 1], false, ref cells);

            //Bottom edges
            r = rand.Next(2, yLength / 2 - 3);
            c = this.Board[xLength - 1, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[xLength - 1, r + 1], false, ref cells);

            r = rand.Next(yLength / 2 + 2, yLength - 3);
            c = this.Board[xLength - 1, r];
            CreateCellWall(c, Direction.Right);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[xLength - 1, r + 1], false, ref cells);

            //Left edges
            r = rand.Next(2, xLength / 2 - 3);
            c = this.Board[r, 0];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[r - 1, 0], false, ref cells);

            r = rand.Next(xLength / 2 + 2, xLength - 3);
            c = this.Board[r, 0];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[r - 1, 0], false, ref cells);

            //Right edges
            r = rand.Next(2, xLength / 2 - 3);
            c = this.Board[r, yLength - 1];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[r - 1, yLength - 1], false, ref cells);

            r = rand.Next(xLength / 2 + 2, xLength - 3);
            c = this.Board[r, yLength - 1];
            CreateCellWall(c, Direction.Up);
            RemoveCells(c, false, ref cells);
            RemoveCells(this.Board[r - 1, yLength - 1], false, ref cells);
        }

        private void CreateWinningDestinations(int destinations, int robots, ref List<Cell> possibleWinningDestinations)
        {
            Random rand = new Random();
            int xLength = this.Board.GetLength(0);
            int yLength = this.Board.GetLength(1);
            int r;
            Cell c;

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

                    c = possibleWinningDestinations[rand.Next(possibleWinningDestinations.Count)];

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
        }

        private void CreateDeflectors(int deflectors)
        {
            if (deflectors <= 0)
                return;

            deflectors = Math.Min(deflectors, 8);

            int xLength = this.Board.GetLength(0);
            int yLength = this.Board.GetLength(1);

            //Spawn deflectors
            // * Max 2 per quadrant
            // * Max 2 per robotId
            // * Max 4 of each type (/ \)
            List<List<int>> quadrants = new List<List<int>>()
            {
                new List<int> {(int)DeflectorType.Backward, (int)DeflectorType.Forward },
                new List<int> {(int)DeflectorType.Backward, (int)DeflectorType.Forward },
                new List<int> {(int)DeflectorType.Backward, (int)DeflectorType.Forward },
                new List<int> {(int)DeflectorType.Backward, (int)DeflectorType.Forward }
            };
            List<int> robotsForDeflectors = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3 };

            Random rand = new Random();
            int r;
            int r2;

            //remove [differential] amount of deflectors, randomly.
            for (int i = 0; i < (8 - deflectors); i++)
            {
                r = rand.Next(4);
                r2 = rand.Next(2);
                if (quadrants[r].Count > 1)
                    quadrants[r].RemoveAt(r2);
                else if (quadrants[r].Count == 1)
                    quadrants[r].RemoveAt(0);
                else
                    i--;
            }

            //Remove random robots
            for (int i = 0; i < (8 - deflectors); i++)
            {
                r = rand.Next(robotsForDeflectors.Count);
                robotsForDeflectors.RemoveAt(r);
            }

            List<Cell> possibleDeflectorLocations = new List<Cell>();
            for (int x = 1; x <= xLength - 2; x++)
            {
                for (int y = 1; y <= yLength - 2; y++)
                {
                    if ((x >= 6 && x <= 9) && (y >= 6 && y <= 9))
                        continue;

                    if (quadrants[this.Board[x, y].GetQuadrant()].Count == 0)
                        continue;

                    possibleDeflectorLocations.Add(this.Board[x, y]);
                }
            }

            //Deflectors will not spawn adjacent to a winning location, but can spawn diagonally from one.
            foreach (DestinationCell dc in WinningDestinations)
            {
                Cell temp = this.Board[dc.X, dc.Y];
                RemoveCells(temp, false, ref possibleDeflectorLocations);
            }

            CreateDeflectors(possibleDeflectorLocations, robotsForDeflectors, quadrants);
        }

        private void CreateDeflectors(List<Cell> locations, List<int> robots, List<List<int>> quadrants)
        {
            Random rand = new Random();
            int r;
            Cell c;
            while (robots.Count != 0)
            {
                if (locations.Count == 0)
                    break;

                c = locations[rand.Next(locations.Count())];
                int q = c.GetQuadrant();
                r = rand.Next(robots.Count);
                c.Deflector = new Deflector(robots[r], (DeflectorType)quadrants[q][0]);
                robots.RemoveAt(r);
                quadrants[q].RemoveAt(0);

                RemoveCells(c, false, ref locations);

                if (quadrants[q].Count == 0)
                {
                    List<Cell> cellsToRemove = new List<Cell>();
                    foreach (Cell d in locations)
                        if (d.GetQuadrant() == q)
                            cellsToRemove.Add(d);

                    foreach (Cell d in cellsToRemove)
                        locations.Remove(d);
                }
            }
        }

        private void CreatePortals(int portals)
        {
            if (portals <= 0)
                return;

            portals = Math.Min(portals, 4);

            int xLength = this.Board.GetLength(0);
            int yLength = this.Board.GetLength(1);
            Random rand = new Random();
            int r;
            int r2;
            int robot;

            //Spawn Portals
            // * Max 2 per quadrant
            // * Max 2 per robotId
            List<int> initialRobots = new List<int> { 0, 1, 2, 3 };
            List<int> robotsForPortals = new List<int> { };
            List<Cell> possiblePortalLocations = new List<Cell>();
            List<List<int>> quadrants = new List<List<int>>()
            {
                new List<int> { 0, 1 },
                new List<int> { 0, 1 },
                new List<int> { 0, 1 },
                new List<int> { 0, 1 }
            };

            //Create the robot pairs for the portals
            for (int i = 0; i < portals; i++)
            {
                r = rand.Next(initialRobots.Count);
                robot = initialRobots[r];
                initialRobots.RemoveAt(r);
                robotsForPortals.Add(robot);
                robotsForPortals.Add(robot);
            }

            //Remove random quadrants.
            for (int i = 0; i < (8 - (portals * 2)); i++)
            {
                r = rand.Next(4);
                r2 = rand.Next(2);
                if (quadrants[r].Count > 1)
                    quadrants[r].RemoveAt(r2);
                else if (quadrants[r].Count == 1)
                    quadrants[r].RemoveAt(0);
                else
                    i--;
            }
            
            for (int x = 1; x <= xLength - 2; x++)
            {
                for (int y = 1; y <= yLength - 2; y++)
                {
                    if ((x >= 6 && x <= 9) && (y >= 6 && y <= 9))
                        continue;

                    if (quadrants[this.Board[x, y].GetQuadrant()].Count == 0)
                        continue;

                    possiblePortalLocations.Add(this.Board[x, y]);
                }
            }

            //Portals will not spawn adjacent to a winning location, but can spawn diagonally from one.
            foreach (DestinationCell dc in WinningDestinations)
            {
                Cell temp = this.Board[dc.X, dc.Y];
                RemoveCells(temp, false, ref possiblePortalLocations);
            }

            //Portals will not spawn adjacent to a Deflector, but can spawn diagonally from one.
            foreach (Cell c2 in this.Board)
            {
                if (c2.Deflector == null)
                    continue;

                RemoveCells(c2, false, ref possiblePortalLocations);
            }

            CreatePortals(possiblePortalLocations, robotsForPortals, quadrants);
        }

        private void CreatePortals(List<Cell> locations, List<int> robots, List<List<int>> quadrants)
        {
            List<Cell> portalLocations = new List<Cell>();
            Random rand = new Random();
            int r;
            Cell c;
            while (robots.Count != 0)
            {
                if (locations.Count == 0)
                    break;

                c = locations[rand.Next(locations.Count())];
                int q = c.GetQuadrant();
                r = rand.Next(robots.Count);
                c.Portal = new Portal(robots[r], null);
                robots.RemoveAt(r);
                quadrants[q].RemoveAt(0);

                RemoveCells(c, false, ref locations);

                if (quadrants[q].Count == 0)
                {
                    List<Cell> cellsToRemove = new List<Cell>();
                    foreach (Cell d in locations)
                        if (d.GetQuadrant() == q)
                            cellsToRemove.Add(d);

                    foreach (Cell d in cellsToRemove)
                        locations.Remove(d);
                }

                portalLocations.Add(c);
            }

            for (int i = 0; i < 4; i++)
            {
                if (portalLocations.Where(c2 => c2.Portal.RobotID == i).Count() < 2)
                    continue;

                Cell entrance = portalLocations.Where(c2 => c2.Portal.RobotID == i).ElementAt(0);
                Cell exit = portalLocations.Where(c2 => c2.Portal.RobotID == i).ElementAt(1);
                if (entrance == null || exit == null)
                    continue;

                entrance.Portal.Exit = exit;
                exit.Portal.Exit = entrance;
            }
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
    }
}
