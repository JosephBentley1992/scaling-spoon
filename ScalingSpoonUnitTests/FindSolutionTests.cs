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
            DestinationCell dc = new DestinationCell(_model.Board[2, 2]);
            dc.WinningRobotId = _robot.Id;
            _model.CurrentWinningDestination = dc;
            _model.WinningDestinations.Add(dc);
            List<RobotMove> movesToWin = new List<RobotMove>() { new RobotMove(_robot.Id, _model.Board[2, 1], _model.Board[2, 2]) };
            List<RobotMove> moves = new GameSolver(_model).FindSolution();

            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);

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
            DestinationCell dc = new DestinationCell(_model.Board[1, 3]);
            dc.WinningRobotId = _robot.Id;
            _model.CurrentWinningDestination = dc;
            _model.WinningDestinations.Add(dc);

            _model.CreateCellWall(_model.Board[0, 1], Direction.Right);
            _model.CreateCellWall(_model.Board[1, 0], Direction.Down);
            _model.CreateCellWall(_model.Board[1, 3], Direction.Up, Direction.Right);
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
            List<RobotMove> moves = new GameSolver(_model).FindSolution();

            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);

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

            DestinationCell dc = new DestinationCell(_model.Board[1, 3]);
            dc.WinningRobotId = _robot.Id;
            _model.CurrentWinningDestination = dc;
            _model.WinningDestinations.Add(dc);

            _model.CreateCellWall(_model.Board[0, 1], Direction.Right);
            _model.CreateCellWall(_model.Board[1, 0], Direction.Down);
            _model.CreateCellWall(_model.Board[1, 3], Direction.Up, Direction.Right);
            _model.CreateCellWall(_model.Board[2, 4], Direction.Down);
            _model.CreateCellWall(_model.Board[4, 1], Direction.Right);

            List<RobotMove> movesToWin = new List<RobotMove>()
            { new RobotMove(_robotBlue.Id, _model.Board[2, 2], _model.Board[4, 2]),
            new RobotMove(_robot.Id, _model.Board[4, 4], _model.Board[4, 3]),
            new RobotMove(_robot.Id, _model.Board[4, 3], _model.Board[1, 3]) };
            List<RobotMove> moves = new GameSolver(_model).FindSolution();

            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);

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

