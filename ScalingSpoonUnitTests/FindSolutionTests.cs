using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScalingSpoon.Model;
using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System.Collections.Generic;
namespace ScalingSpoonTests
{
    [TestClass]
    public class FindSolutionTests
    {
        private static Engine _model;
        private static Robot _robot;
        private static Robot _robotBlue;
        private static Robot _robotGreen;
        private static Robot _robotYellow;

        [TestMethod]
        public void OneMoveSolution_Right()
        {
            /* a a a
            * b b b
            * c R ★
            */
            _model = new Engine();
            _model.Board = new Cell[3, 3];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            _robot = _model.CreateRobot(2, 1);

            _model.CreateWinningDestination(2, 2, _robot.Id, true);
            _model.CreateWinningDestination(0, 0, _robot.Id, false);
            List<RobotMove> movesToWin = new List<RobotMove>() { new RobotMove(_robot.Id, _model.Board[2, 1], _model.Board[2, 2]) };
            GameSolver solver = new GameSolver(_model);
            List<RobotMove> moves = solver.FindSolution();

            if (moves.Count < movesToWin.Count)
            {
                Assert.IsTrue(false);
                return;
            }

            for (int i = 0; i < movesToWin.Count; i++)
                Assert.AreEqual(movesToWin[i], moves[i]);
        }

        [TestMethod]
        public void AllDirections_OneRobot()
        {
            //★
            /* a a|a _ a
             * _ b b *|b
             * c c c c _
             * d d d d d
             * e e|e e R
             */
            _model = new Engine();
            _model.Board = new Cell[5, 5];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            _robot = _model.CreateRobot(4, 4);

            _model.CreateWinningDestination(1, 3, _robot.Id, true, Direction.Up, Direction.Right);
            _model.CreateWinningDestination(0, 0, _robot.Id, false);

            _model.CreateCellWall(_model.Board[0, 1], Direction.Right);
            _model.CreateCellWall(_model.Board[1, 0], Direction.Down);
            _model.CreateCellWall(_model.Board[2, 4], Direction.Down);
            _model.CreateCellWall(_model.Board[4, 1], Direction.Right);

            List<RobotMove> movesToWin = new List<RobotMove>()
            { new RobotMove(_robot.Id, _model.Board[4, 4], _model.Board[3, 4]),
            new RobotMove(_robot.Id, _model.Board[3, 4], _model.Board[3, 0]),
            new RobotMove(_robot.Id, _model.Board[3, 0], _model.Board[4, 0]),
            new RobotMove(_robot.Id, _model.Board[4, 0], _model.Board[4, 1]),
            new RobotMove(_robot.Id, _model.Board[4, 1], _model.Board[0, 1]),
            new RobotMove(_robot.Id, _model.Board[0, 1], _model.Board[0, 0]),
            new RobotMove(_robot.Id, _model.Board[0, 0], _model.Board[1, 0]),
            new RobotMove(_robot.Id, _model.Board[1, 0], _model.Board[1, 3])};
            GameSolver solver = new GameSolver(_model);
            List<RobotMove> moves = solver.FindSolution();

            if (moves.Count < movesToWin.Count)
            {
                Assert.IsTrue(false);
                return;
            }

            for (int i = 0; i < movesToWin.Count; i++)
                Assert.AreEqual(movesToWin[i], moves[i]);
        }

        [TestMethod]
        public void MovingAnotherRobot_Once()
        {
            //★
            /* a a|a _ a
             * _ b b *|b
             * c c B c _
             * d d d d d
             * e e|e e R
             */
            _model = new Engine();
            _model.Board = new Cell[5, 5];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            _robot = _model.CreateRobot(4, 4);
            _robotBlue = _model.CreateRobot(2, 2);

            _model.CreateWinningDestination(1, 3, _robot.Id, true, Direction.Up, Direction.Right);
            _model.CreateWinningDestination(0, 0, _robot.Id, false);

            _model.CreateCellWall(_model.Board[0, 1], Direction.Right);
            _model.CreateCellWall(_model.Board[1, 0], Direction.Down);
            _model.CreateCellWall(_model.Board[2, 4], Direction.Down);
            _model.CreateCellWall(_model.Board[4, 1], Direction.Right);

            List<RobotMove> movesToWin = new List<RobotMove>()
            { new RobotMove(_robotBlue.Id, _model.Board[2, 2], _model.Board[4, 2]),
            new RobotMove(_robot.Id, _model.Board[4, 4], _model.Board[4, 3]),
            new RobotMove(_robot.Id, _model.Board[4, 3], _model.Board[1, 3]) };
            GameSolver solver = new GameSolver(_model);
            List<RobotMove> moves = solver.FindSolution();

            if (moves.Count < movesToWin.Count)
            {
                Assert.IsTrue(false);
                return;
            }

            for (int i = 0; i < movesToWin.Count; i++)
                Assert.AreEqual(movesToWin[i], moves[i]);
        }

        [TestMethod]
        public void MovingOneRobot_16x16Board()
        {
            //Board: MovingOneRobot_16x16.png
            _model = new Engine();
            _model.Board = new Cell[16, 16];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            _robot = _model.CreateRobot(4,2);
            _robotGreen = _model.CreateRobot(10, 2);
            _robotYellow = _model.CreateRobot(14, 1);
            _robotBlue = _model.CreateRobot(5, 15);

            //Destination Cells
            _model.CreateWinningDestination(1, 14, _robotGreen.Id, true, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(2, 1, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(2, 11, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(4, 9, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(5, 4, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(5, 6, _robotGreen.Id, false, Direction.Left, Direction.Down);
            _model.CreateWinningDestination(6, 10, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(6, 13, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(7, 4, _robotGreen.Id, false, Direction.Right, Direction.Down);
            _model.CreateWinningDestination(9, 13, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(10, 5, _robotGreen.Id, false, Direction.Up, Direction.Right);
            _model.CreateWinningDestination(10, 11, _robotGreen.Id, false, Direction.Up, Direction.Right);
            _model.CreateWinningDestination(11, 7, _robotGreen.Id, false, Direction.Right, Direction.Down);
            _model.CreateWinningDestination(11, 9, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(12, 5, _robotGreen.Id, false, Direction.Left, Direction.Down);
            _model.CreateWinningDestination(13, 2, _robotGreen.Id, false, Direction.Left, Direction.Down);
            _model.CreateWinningDestination(14, 6, _robotGreen.Id, false, Direction.Left, Direction.Up);
            _model.CreateWinningDestination(14, 14, _robotGreen.Id, false, Direction.Right, Direction.Down);

            //Edges
            _model.CreateCellWall(_model.Board[0, 3], Direction.Right);
            _model.CreateCellWall(_model.Board[0, 11], Direction.Right);
            _model.CreateCellWall(_model.Board[15, 3], Direction.Right);
            _model.CreateCellWall(_model.Board[15, 10], Direction.Right);
            _model.CreateCellWall(_model.Board[3, 0], Direction.Down);
            _model.CreateCellWall(_model.Board[12, 0], Direction.Down);
            _model.CreateCellWall(_model.Board[4, 15], Direction.Down);
            _model.CreateCellWall(_model.Board[10, 15], Direction.Down);

            //Middle 2x2
            _model.CreateCellWall(_model.Board[7, 7], Direction.Up, Direction.Right, Direction.Down, Direction.Left);
            _model.CreateCellWall(_model.Board[7, 8], Direction.Up, Direction.Right, Direction.Down, Direction.Left);
            _model.CreateCellWall(_model.Board[8, 7], Direction.Up, Direction.Right, Direction.Down, Direction.Left);
            _model.CreateCellWall(_model.Board[8, 8], Direction.Up, Direction.Right, Direction.Down, Direction.Left);

            List<RobotMove> movesToWin = new List<RobotMove>()
            { new RobotMove(_robot.Id, _model.Board[4, 2], _model.Board[0, 2]),
            new RobotMove(_robotGreen.Id, _model.Board[10, 2], _model.Board[1, 2]),
            new RobotMove(_robotGreen.Id, _model.Board[1, 2], _model.Board[1, 13]),
            new RobotMove(_robotGreen.Id, _model.Board[1, 13], _model.Board[5, 13]),
            new RobotMove(_robotGreen.Id, _model.Board[5, 13], _model.Board[5, 14]),
            new RobotMove(_robotGreen.Id, _model.Board[5, 14], _model.Board[1, 14]) };
            GameSolver solver = new GameSolver(_model);
            List<RobotMove> moves = solver.FindSolution();

            System.Diagnostics.Debugger.Launch();
            if (moves.Count < movesToWin.Count)
            {
                Assert.IsTrue(false);
                return;
            }

            for (int i = 0; i < movesToWin.Count; i++)
                Assert.AreEqual(movesToWin[i], moves[i]);
        }
    }
}

