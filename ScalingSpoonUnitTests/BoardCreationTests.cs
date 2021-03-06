﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScalingSpoon.Model;
using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
namespace ScalingSpoonTests
{
    [TestClass]
    public class CreateCellWallTests
    {
        private static Engine _model;

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
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);
        }

        [TestMethod]
        public void CanCreateUpWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Up);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.Up);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.Down);
        }

        [TestMethod]
        public void CanCreateDownWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Down);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.Down);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.Up);
        }

        [TestMethod]
        public void CanCreateRightWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Right);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.Right);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.Left);
        }

        [TestMethod]
        public void CanCreateLeftWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Left);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.Left);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.Right);
        }

        [TestMethod]
        public void CanCreateUpAndRight()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Up, Direction.Right);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.UpAndRight);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.Down);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.Left);
        }

        [TestMethod]
        public void CanCreateDownAndLeft()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Down, Direction.Left);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.DownAndLeft);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.Up);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.Right);
        }

        [TestMethod]
        public void CanCreateRightAndDown()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Right, Direction.Down);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.RightAndDown);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.Left);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.Up);
        }

        [TestMethod]
        public void CanCreateLeftAndUp()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Left, Direction.Up);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.LeftAndUp);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.Right);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.Down);
        }

        [TestMethod]
        public void CanCreateAll()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.All);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.Down);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.Left);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.Up);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.Right);
        }
    }

    [TestClass]
    public class RemoveCellWallTests
    {
        private static Engine _model;

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
                    _model.Board[x, y] = new Cell(id++, CellWalls.None, x, y);
        }

        [TestMethod]
        public void CanDeleteUpWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Up);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Up);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteDownWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Down);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Down);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteRightWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Right);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Right);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteLeftWall()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Left);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Left);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteUpAndRight()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Up, Direction.Right);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Up, Direction.Right);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteDownAndLeft()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Down, Direction.Left);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Down, Direction.Left);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteRightAndDown()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Right, Direction.Down);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Right, Direction.Down);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteLeftAndUp()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Left, Direction.Up);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Left, Direction.Up);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.None);
        }

        [TestMethod]
        public void CanDeleteAll()
        {
            _model.CreateCellWall(_model.Board[1, 1], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            _model.RemoveCellWall(_model.Board[1, 1], Direction.Up, Direction.Right, Direction.Left, Direction.Down);
            Assert.AreEqual(_model.Board[1, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[0, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 2].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[2, 1].Walls, CellWalls.None);
            Assert.AreEqual(_model.Board[1, 0].Walls, CellWalls.None);
        }
    }
}