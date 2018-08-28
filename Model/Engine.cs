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
        public Cell[][] Board { get; set; }
        public List<RobotCell> RobotInitialLocations { get; set; }
        public List<RobotCell> RobotCurrentLocations { get; set; }
        public List<DestinationCell> WinningDestinations { get; set; }
        public DestinationCell CurrentWinningDestination { get; set; }

        public Engine()
        {
            
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

        public void MoveRobot(int robot, Direction d)
        {
            //TODO: Try to write tests first! but... without board generation code?... I could Unit Test a 3x3 board for starters.
        }

        public RobotCell UndoMove()
        {
            //TODO: Figure out how to structure the difference between Undo'ing a move on the current puzzle, vs Undo'ing a historic puzzle.
            return new RobotCell(new Robot(), new Cell());
        }
    }
}
