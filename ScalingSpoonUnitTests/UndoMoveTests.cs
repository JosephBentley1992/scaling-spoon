using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScalingSpoon.Model;
using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System.Collections.Generic;
namespace ScalingSpoonTests
{
    [TestClass]
    public class UndoMoveTests
    {
        private static Engine _model;
        private static Robot _robotRed;
        private static Robot _robotBlue;

        [TestInitialize]
        public void Setup()
        {
            /* a a a
             * b R b
             * c c c
             */
            _model = new Engine();
            _model.Board = new Cell[3, 3];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _robotRed = _model.CreateRobot(1, 1);
            _robotBlue = _model.CreateRobot(2, 2);

        }

        [TestMethod]
        public void UndoMovementUp()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Up);
            _model.UndoMove();
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
        }

        [TestMethod]
        public void UndoMovementDown()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Down);
            _model.UndoMove();
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
        }

        [TestMethod]
        public void UndoMovementRight()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            _model.UndoMove();
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
        }

        [TestMethod]
        public void UndoMovementLeft()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Left);
            _model.UndoMove();
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
        }

        [TestMethod]
        public void UndoAllDirections()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Up, Direction.Right, Direction.Down, Direction.Left);
            _model.UndoMove();
            _model.UndoMove();
            _model.UndoMove();
            _model.UndoMove();
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
        }

        [TestMethod]
        public void UndoMultipleRobotMoves()
        {
            /*
            * a a a
            * b R b //initial pos
            * c c B
            */
            _model.MoveRobot(_robotRed.Id, Direction.Down, Direction.Left);
            _model.MoveRobot(_robotBlue.Id, Direction.Left, Direction.Up);

            /* a B a
            *  b b b
            *  R c c
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 1]);

            _model.UndoMove();
            /* a a a
            *  b b b
            *  R B c
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 1]);

            _model.UndoMove();
            /* a a a
            *  b b b
            *  R c B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);

            _model.UndoMove();
            /* a a a
            *  b b b
            *  c R B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 1]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);

            _model.UndoMove();
            /* a a a
            *  b R b
            *  c c B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);
        }
    }

    [TestClass]
    public class UndoWinningPositionsTests
    {
        private static Engine _model;
        private static Robot _robotRed;
        private static Robot _robotBlue;

        [TestInitialize]
        public void Setup()
        {
            /* * a a
             * b R *
             * * c B
             */
            _model = new Engine();
            _model.Board = new Cell[3, 3];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _robotRed = _model.CreateRobot(1, 1);
            _robotBlue = _model.CreateRobot(2, 2);

            _model.CreateWinningDestination(2, 0, _robotRed.Id, true);
            _model.CreateWinningDestination(1, 2, _robotBlue.Id, false);
            _model.CreateWinningDestination(0, 0, _robotRed.Id, false);
        }

        [TestMethod]
        public void AllTestsInOne_AddIndividualTestLater()
        {
            /*
            * a a a
            * b R b //initial pos
            * * c B
            */
            _model.MoveRobot(_robotRed.Id, Direction.Down, Direction.Left);

            /* a a a
            *  b b *
            *  R c B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);
            Assert.AreEqual(_model.CurrentWinningDestination, _model.Board[1, 2]);
            Assert.IsFalse(_model.WinningDestinations[0].CurrentWinningCell);
            Assert.IsTrue(_model.WinningDestinations[1].CurrentWinningCell);

            _model.MoveRobot(_robotBlue.Id, Direction.Up, Direction.Left, Direction.Down, Direction.Right);
            /* * a a
            *  b b B
            *  R c c
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[1, 2]);
            Assert.AreEqual(_model.CurrentWinningDestination, _model.Board[0, 0]);
            Assert.IsFalse(_model.WinningDestinations[1].CurrentWinningCell);
            Assert.IsTrue(_model.WinningDestinations[2].CurrentWinningCell);

            _model.UndoMove();
            /* a a a
            *  B b *
            *  R c c
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[1, 0]);
            Assert.AreEqual(_model.CurrentWinningDestination, _model.Board[1, 2]);
            Assert.IsTrue(_model.WinningDestinations[1].CurrentWinningCell);
            Assert.IsFalse(_model.WinningDestinations[2].CurrentWinningCell);

            _model.UndoMove();
            /* B a a
            *  b b *
            *  R c c
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 0]);

            _model.UndoMove();
            /* a a B
            *  b b *
            *  R c c
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 2]);

            _model.UndoMove();
            /* a a a
            *  b b *
            *  R c B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);

            _model.UndoMove();
            /* a a a
            *  b b b
            *  * R B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[2, 1]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);
            Assert.AreEqual(_model.CurrentWinningDestination, _model.Board[2, 0]);
            Assert.IsTrue(_model.WinningDestinations[0].CurrentWinningCell);
            Assert.IsFalse(_model.WinningDestinations[1].CurrentWinningCell);

            _model.UndoMove();
            /* a a a
            *  b R b
            *  * c B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);

            _model.UndoMove();
            /* a a a
            *  b R b
            *  * c B
            */
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 1]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 2]);
            Assert.AreEqual(_model.CurrentWinningDestination, _model.Board[2, 0]);
            Assert.IsTrue(_model.WinningDestinations[0].CurrentWinningCell);
        }
    }
}
