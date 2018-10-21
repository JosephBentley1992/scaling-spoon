using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScalingSpoon.Model;
using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
namespace ScalingSpoonTests
{
    [TestClass]
    public class MovementTestsBoardEdges
    {
        private static Engine _model;
        private static Robot _robot;

        [TestInitialize]
        public void Setup()
        {
            /* R a a
             * b b b
             * c c c
             */
            _model = new Engine();
            _model.Board = new Cell[3, 3];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _robot = _model.CreateRobot(0, 0);
        }

        [TestMethod]
        public void RobotCollidesWithRightOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 2]);
        }

        [TestMethod]
        public void RobotCollidesWithBottomOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right, Direction.Down);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);
        }

        [TestMethod]
        public void RobotCollidesWithLeftOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right, Direction.Down, Direction.Left);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 0]);
        }

        [TestMethod]
        public void RobotCollidesWithTopOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right, Direction.Down, Direction.Left, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 0]);
        }
    }

    [TestClass]
    public class MovementTestsWalls
    {
        private static Engine _model = new Engine();
        private static Robot _robot;

        [TestInitialize]
        public void Setup()
        {
            /* a a|a a
             * 
             * b b b b
             * _
             * c c c c
             * d d d d
             */
            _model = new Engine();
            _model.Board = new Cell[4, 4];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _robot = _model.CreateRobot(0, 0);

            _model.CreateCellWall(_model.Board[0, 1], Direction.Right);
            _model.CreateCellWall(_model.Board[1, 0], Direction.Down);
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingRight()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 1]);
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingLeft()
        {
            _model.MoveRobot(_robot.Id, Direction.Right, Direction.Down, Direction.Right, Direction.Up, Direction.Left);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 2]);
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingDown()
        {
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[1, 0]);
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingUp()
        {
            _model.MoveRobot(_robot.Id, Direction.Right, Direction.Down, Direction.Left, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 0]);
        }
    }

    [TestClass]
    public class MovementTestsRobots
    {
        private static Engine _model;
        private static Robot _robotRed;
        private static Robot _robotBlue;

        [TestInitialize]
        public void Setup()
        {
            /* R a a B
             * b b b b
             * c c c c
             * d d d d
             */
            _model = new Engine();
            _model.Board = new Cell[4, 4];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _robotRed = _model.CreateRobot(0, 0);
            _robotBlue = _model.CreateRobot(0, 3);
        }

        [TestMethod]
        public void RobotsCollideMovingRight()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[0, 2]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 3]);
        }

        [TestMethod]
        public void RobotsCollideMovingLeft()
        {
            _model.MoveRobot(_robotBlue.Id, Direction.Left);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[0, 0]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 1]);
        }

        [TestMethod]
        public void RobotsCollideMovingUp()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Down, Direction.Right, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 3]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 3]);
        }

        [TestMethod]
        public void RobotsCollideMovingDown()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Down, Direction.Right);
            _model.MoveRobot(_robotBlue.Id, Direction.Down);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[3, 3]);
            Assert.AreEqual(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 3]);
        }
    }

    [TestClass]
    public class DeflectorForwardMovementTests_NoCollisions
    {
        private static Engine _model = new Engine();
        private static Robot _robot;

        [TestInitialize]
        public void Setup()
        {
            /* a a a a a
             * b b b b b
             * c c / c c
             * d d d d d
             * e e e e e
             */
            _model = new Engine();
            _model.Board = new Cell[5, 5];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _model.Board[2, 2].Deflector = new Deflector(2, DeflectorType.Forward);
        }

        [TestMethod]
        public void RobotDeflectsMovingUp()
        {
            _robot = _model.CreateRobot(4, 2);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 4]);
        }

        [TestMethod]
        public void RobotDeflectsMovingDown()
        {
            _robot = _model.CreateRobot(0, 2);
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 0]);
        }

        [TestMethod]
        public void RobotDeflectsMovingLeft()
        {
            _robot = _model.CreateRobot(2, 4);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[4, 2]);
        }

        [TestMethod]
        public void RobotDeflectsMovingRight()
        {
            _robot = _model.CreateRobot(2, 0);
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 2]);
        }
    }

    [TestClass]
    public class DeflectorBackwardMovementTests_NoCollisions
    {
        private static Engine _model = new Engine();
        private static Robot _robot;

        [TestInitialize]
        public void Setup()
        {
            /* a a a a a
             * b b b b b
             * c c \ c c
             * d d d d d
             * e e e e e
             */
            _model = new Engine();
            _model.Board = new Cell[5, 5];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _model.Board[2, 2].Deflector = new Deflector(2, DeflectorType.Backward);
        }

        [TestMethod]
        public void RobotDeflectsMovingUp()
        {
            _robot = _model.CreateRobot(4, 2);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 0]);
        }

        [TestMethod]
        public void RobotDeflectsMovingDown()
        {
            _robot = _model.CreateRobot(0, 2);
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 4]);
        }

        [TestMethod]
        public void RobotDeflectsMovingLeft()
        {
            _robot = _model.CreateRobot(2, 4);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 2]);
        }

        [TestMethod]
        public void RobotDeflectsMovingRight()
        {
            _robot = _model.CreateRobot(2, 0);
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[4, 2]);
        }
    }

    [TestClass]
    public class DeflectorCollidingWithRobotGetsStuck
    {
        private static Engine _model = new Engine();
        private static Robot _robot;
        private static Robot _robot2;

        [TestInitialize]
        public void Setup()
        {
            /* a a a a a
             * b b b b b
             * c c / c c
             * d d d d d
             * e e e e e
             */
            _model = new Engine();
            _model.Board = new Cell[5, 5];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _model.Board[2, 2].Deflector = new Deflector(2, DeflectorType.Forward);
        }

        [TestMethod]
        public void RobotDeflectsMovingUp()
        {
            _robot = _model.CreateRobot(4, 2);
            _robot2 = _model.CreateRobot(2, 3);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);
        }

        [TestMethod]
        public void RobotDeflectsMovingDown()
        {
            _robot = _model.CreateRobot(0, 2);
            _robot2 = _model.CreateRobot(2, 1);
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);
        }

        [TestMethod]
        public void RobotDeflectsMovingLeft()
        {
            _robot = _model.CreateRobot(2, 4);
            _robot2 = _model.CreateRobot(3, 2);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);
        }

        [TestMethod]
        public void RobotDeflectsMovingRight()
        {
            _robot = _model.CreateRobot(2, 0);
            _robot2 = _model.CreateRobot(1, 2);
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);
        }
    }

    [TestClass]
    public class DeflectorForwardMovementTests_PhasingThrough
    {
        private static Engine _model = new Engine();
        private static Robot _robot;

        [TestInitialize]
        public void Setup()
        {
            /* a a a a a
             * b b b b b
             * c c / c c
             * d d d d d
             * e e e e e
             */
            _model = new Engine();
            _model.Board = new Cell[5, 5];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _model.Board[2, 2].Deflector = new Deflector(0, DeflectorType.Forward);
        }

        [TestMethod]
        public void RobotPhasesThroughMovingUp()
        {
            _robot = _model.CreateRobot(4, 2);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 2]);
        }

        [TestMethod]
        public void RobotPhasesThroughMovingDown()
        {
            _robot = _model.CreateRobot(0, 2);
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[4, 2]);
        }

        [TestMethod]
        public void RobotPhasesThroughMovingLeft()
        {
            _robot = _model.CreateRobot(2, 4);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 0]);
        }

        [TestMethod]
        public void RobotPhasesThroughMovingRight()
        {
            _robot = _model.CreateRobot(2, 0);
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 4]);
        }
    }

    [TestClass]
    public class DeflectorMultipleCollisions
    {
        private static Engine _model = new Engine();
        private static Robot _robot;
        private static Robot _robot2;

        [TestInitialize]
        public void Setup()
        {
            /* a a a a a
             * b b b b b
             * c / c \ c
             * d d d d d
             * e e e e e
             */
            _model = new Engine();
            _model.Board = new Cell[5, 5];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);

            _model.Board[2, 1].Deflector = new Deflector(2, DeflectorType.Forward);
            _model.Board[2, 3].Deflector = new Deflector(3, DeflectorType.Backward);
        }

        [TestMethod]
        public void RobotChangesDirectionsTwiceMovingUp()
        {
            _robot = _model.CreateRobot(4, 1);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[4, 3]);
        }

        [TestMethod]
        public void RobotChangesDirectionsTwiceMovingUp_2()
        {
            _robot = _model.CreateRobot(4, 3);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[4, 1]);
        }

        [TestMethod]
        public void RobotDeflectsButThenGetsStuckOnDeflector()
        {
            _robot = _model.CreateRobot(4, 1);
            _robot2 = _model.CreateRobot(3, 3);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 3]);
        }

        [TestMethod]
        public void RobotDeflectsAndThenPhasesThrough()
        {
            _model.Board[2, 3].Deflector.RobotID = 0;
            _robot = _model.CreateRobot(4, 1);
            _robot2 = _model.CreateRobot(3, 3);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.AreEqual(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 4]);
        }
    }
}

