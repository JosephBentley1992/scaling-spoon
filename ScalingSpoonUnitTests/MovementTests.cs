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
            /* a a a
             * b b b
             * c c c
             */
            _model = new Engine();
            _model.Board = new Cell[3, 3];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            _robot = new Robot(1);

            _model.RobotInitialLocations = new System.Collections.Generic.Dictionary<int, Cell>();
            _model.RobotCurrentLocations = new System.Collections.Generic.Dictionary<int, Cell>();
            _model.RobotInitialLocations.Add(_robot.Id, _model.Board[0, 0]);
            _model.RobotCurrentLocations.Add(_robot.Id, _model.Board[0, 0]);
            _model.Board[0, 0].RobotID = _robot.Id;

            _model.CurrentWinningDestination = new DestinationCell();
        }

        [TestMethod]
        public void RobotCollidesWithRightOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 2);
        }

        [TestMethod]
        public void RobotCollidesWithBottomOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 2);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 2);
        }

        [TestMethod]
        public void RobotCollidesWithLeftOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 2);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 0);
        }

        [TestMethod]
        public void RobotCollidesWithTopOfBoard()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Left);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 0);
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
            _model.Board = new Cell[4, 4];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            _robot = new Robot(1);
            _model.RobotInitialLocations = new System.Collections.Generic.Dictionary<int, Cell>();
            _model.RobotCurrentLocations = new System.Collections.Generic.Dictionary<int, Cell>();
            _model.RobotInitialLocations.Add(_robot.Id, _model.Board[0, 0]);
            _model.RobotCurrentLocations.Add(_robot.Id, _model.Board[0, 0]);
            _model.Board[0, 0].RobotID = _robot.Id;

            _model.CurrentWinningDestination = new DestinationCell();

            _model.Board[0, 1].HasEastWall = true;
            _model.Board[0, 2].HasWestWall = true;

            _model.Board[1, 0].HasSouthWall = true;
            _model.Board[2, 0].HasNorthWall = true;
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingRight()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 1);
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingLeft()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Up);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 2);
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingDown()
        {
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 1);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 0);
        }

        [TestMethod]
        public void RobotCollidesWithWallMovingUp()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Left);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].X == 2);
            Assert.IsTrue(_model.RobotCurrentLocations[_robot.Id].Y == 0);
        }
    }

    [TestClass]
    public class MovementTestsRobots
    {
        private static Engine _model = new Engine();
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
            _model.Board = new Cell[4, 4];

            int id = 0;
            for (int x = 0; x <= _model.Board.GetLength(0) - 1; x++)
                for (int y = 0; y <= _model.Board.GetLength(1) - 1; y++)
                    _model.Board[x, y] = new Cell(id++, false, false, false, false, x, y);

            _robotRed = new Robot(1);
            _robotBlue = new Robot(2);
            _model.RobotInitialLocations = new System.Collections.Generic.Dictionary<int, Cell>();
            _model.RobotCurrentLocations = new System.Collections.Generic.Dictionary<int, Cell>();
            _model.RobotInitialLocations.Add(_robotRed.Id, _model.Board[0, 0]);
            _model.RobotCurrentLocations.Add(_robotRed.Id, _model.Board[0, 0]);
            _model.RobotInitialLocations.Add(_robotBlue.Id, _model.Board[0, 3]);
            _model.RobotCurrentLocations.Add(_robotBlue.Id, _model.Board[0, 3]);
            _model.Board[0, 0].RobotID = _robotRed.Id;
            _model.Board[0, 3].RobotID = _robotBlue.Id;

            _model.CurrentWinningDestination = new DestinationCell();
        }

        [TestMethod]
        public void RobotsCollideMovingRight()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].Y == 2);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].Y == 3);
        }

        [TestMethod]
        public void RobotsCollideMovingLeft()
        {
            _model.MoveRobot(_robotBlue.Id, Direction.Left);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].Y == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].Y == 1);
        }

        [TestMethod]
        public void RobotsCollideMovingUp()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Down);
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            _model.MoveRobot(_robotRed.Id, Direction.Up);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].X == 1);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].Y == 3);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].X == 0);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].Y == 3);
        }

        [TestMethod]
        public void RobotsCollideMovingDown()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Down);
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            _model.MoveRobot(_robotBlue.Id, Direction.Down);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].X == 3);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotRed.Id].Y == 3);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].X == 2);
            Assert.IsTrue(_model.RobotCurrentLocations[_robotBlue.Id].Y == 3);
        }
    }
}

