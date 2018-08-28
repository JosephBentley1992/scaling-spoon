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
        private Engine _model = new Engine();
        private Robot _robot;

        [ClassInitialize]
        public void Setup()
        {
            _model.Board = new Cell[3, 3];
            _robot = new Robot(1);
            _model.RobotInitialLocations.Add(_robot.Id, _model.Board[0, 0]);
            _model.RobotCurrentLocations.Add(_robot.Id, _model.Board[0, 0]);
        }

        [TestMethod]
        public void ColidingWithRightBounds()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 2]);
        }

        [TestMethod]
        public void ColidingWithBottomBounds()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 2]);
        }

        [TestMethod]
        public void ColidingWithLeftBounds()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 0]);
        }

        [TestMethod]
        public void ColidingWithTopBounds()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Left);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 0]);
        }
    }

    [TestClass]
    public class MovementTestsWalls
    {
        private Engine _model = new Engine();
        private Robot _robot;

        [ClassInitialize]
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
            _robot = new Robot(1);
            _model.RobotInitialLocations.Add(_robot.Id, _model.Board[0, 0]);
            _model.RobotCurrentLocations.Add(_robot.Id, _model.Board[0, 0]);

            _model.Board[0, 1].HasEastWall = true;
            _model.Board[0, 2].HasWestWall = true;

            _model.Board[1, 0].HasSouthWall = true;
            _model.Board[2, 0].HasNorthWall = true;
        }

        [TestMethod]
        public void ColidesWithEastWall()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 1]);
        }

        [TestMethod]
        public void ColidesWithWestWall()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Up);
            _model.MoveRobot(_robot.Id, Direction.Left);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[0, 2]);
        }

        [TestMethod]
        public void ColidesWithSouthWall()
        {
            _model.MoveRobot(_robot.Id, Direction.Down);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[1, 0]);
        }

        [TestMethod]
        public void ColidesWithNorthWall()
        {
            _model.MoveRobot(_robot.Id, Direction.Right);
            _model.MoveRobot(_robot.Id, Direction.Down);
            _model.MoveRobot(_robot.Id, Direction.Left);
            _model.MoveRobot(_robot.Id, Direction.Up);
            Assert.Equals(_model.RobotCurrentLocations[_robot.Id], _model.Board[2, 0]);
        }
    }

    [TestClass]
    public class MovementTestsRobots
    {
        private Engine _model = new Engine();
        private Robot _robotRed;
        private Robot _robotBlue;

        [ClassInitialize]
        public void Setup()
        {
            /* R a a B
             * b b b b
             * c c c c
             * d d d d
             */
            _model.Board = new Cell[4, 4];
            _robotRed = new Robot(1);
            _robotBlue = new Robot(2);
            _model.RobotInitialLocations.Add(_robotRed.Id, _model.Board[0, 0]);
            _model.RobotCurrentLocations.Add(_robotRed.Id, _model.Board[0, 0]);
            _model.RobotInitialLocations.Add(_robotBlue.Id, _model.Board[0, 3]);
            _model.RobotCurrentLocations.Add(_robotBlue.Id, _model.Board[0, 3]);
        }

        [TestMethod]
        public void ColidesMovingRight()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            Assert.Equals(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[0, 2]);
            Assert.Equals(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 3]);
        }

        [TestMethod]
        public void ColidesMovingLeft()
        {
            _model.MoveRobot(_robotBlue.Id, Direction.Left);
            Assert.Equals(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[0, 0]);
            Assert.Equals(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 1]);
        }

        [TestMethod]
        public void ColidesMovingUp()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Down);
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            _model.MoveRobot(_robotRed.Id, Direction.Up);
            Assert.Equals(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[1, 3]);
            Assert.Equals(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[0, 3]);
        }

        [TestMethod]
        public void ColidesMovingDown()
        {
            _model.MoveRobot(_robotRed.Id, Direction.Down);
            _model.MoveRobot(_robotRed.Id, Direction.Right);
            _model.MoveRobot(_robotBlue.Id, Direction.Down);
            Assert.Equals(_model.RobotCurrentLocations[_robotRed.Id], _model.Board[3, 3]);
            Assert.Equals(_model.RobotCurrentLocations[_robotBlue.Id], _model.Board[2, 3]);
        }
    }
}

