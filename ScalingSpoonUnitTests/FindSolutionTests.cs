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
            DestinationCell dc = new DestinationCell(_model.Board[2, 2]);
            dc.WinningRobotId = _robot.Id;
            dc.CurrentWinningCell = true;
            _model.CurrentWinningDestination = dc;
            _model.WinningDestinations.Add(dc);

            dc = new DestinationCell(_model.Board[0, 0]);
            dc.WinningRobotId = _robot.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            List<RobotMove> movesToWin = new List<RobotMove>() { new RobotMove(_robot.Id, _model.Board[2, 1], _model.Board[2, 2]) };
            List<RobotMove> moves = new GameSolver(_model).FindSolution();

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
            dc.CurrentWinningCell = true;
            _model.CurrentWinningDestination = dc;
            _model.WinningDestinations.Add(dc);

            dc = new DestinationCell(_model.Board[0, 0]);
            dc.WinningRobotId = _robot.Id;
            dc.CurrentWinningCell = false;
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
            dc.CurrentWinningCell = true;
            _model.CurrentWinningDestination = dc;
            _model.WinningDestinations.Add(dc);

            dc = new DestinationCell(_model.Board[0, 0]);
            dc.WinningRobotId = _robot.Id;
            dc.CurrentWinningCell = false;
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
            DestinationCell dc = new DestinationCell(_model.Board[1, 14]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = true;
            _model.CurrentWinningDestination = dc;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[1, 14], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[2, 1]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[2, 1], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[2, 11]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[2, 11], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[4, 9]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[4, 9], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[5, 4]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[5, 4], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[5, 6]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[5, 6], Direction.Left, Direction.Down);

            dc = new DestinationCell(_model.Board[6, 10]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[6, 10], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[6, 13]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[6, 13], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[7, 4]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[7, 4], Direction.Right, Direction.Down);

            dc = new DestinationCell(_model.Board[9, 13]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[9, 13], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[10, 5]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[10, 5], Direction.Up, Direction.Right);

            dc = new DestinationCell(_model.Board[10, 11]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[10, 11], Direction.Up, Direction.Right);

            dc = new DestinationCell(_model.Board[11, 7]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[11, 7], Direction.Down, Direction.Right);

            dc = new DestinationCell(_model.Board[11, 9]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[11, 9], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[12, 5]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[12, 5], Direction.Left, Direction.Down);

            dc = new DestinationCell(_model.Board[13, 2]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[13, 2], Direction.Left, Direction.Down);

            dc = new DestinationCell(_model.Board[14, 6]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[14, 6], Direction.Left, Direction.Up);

            dc = new DestinationCell(_model.Board[14, 14]);
            dc.WinningRobotId = _robotGreen.Id;
            dc.CurrentWinningCell = false;
            _model.WinningDestinations.Add(dc);
            _model.CreateCellWall(_model.Board[14, 14], Direction.Right, Direction.Down);

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
            List<RobotMove> moves = new GameSolver(_model).FindSolution();
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

