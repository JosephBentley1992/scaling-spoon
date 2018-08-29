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
        private int _robotID = 1;

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
        public void ConstructBoard(int x, int y, int destinations, int robots)
        {
            //TODO: Good luck
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
