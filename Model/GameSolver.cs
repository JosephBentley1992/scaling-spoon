using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingSpoon.Model
{
    public class GameSolver
    {
        private Engine _model = new Engine();
        public GameSolver(Engine e)
        {
            _model = e;
        }

        public List<RobotMove> FindSolution()
        {
            _model.MoveRobot(0, Direction.Right);
            return new List<RobotMove>() { new RobotMove(0, _model.Board[2, 1], _model.Board[2, 2]) };
            //return new List<RobotMove>();
        }
    }
}
