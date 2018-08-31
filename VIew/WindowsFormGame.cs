using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScalingSpoon.View.Bus;
using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;

namespace ScalingSpoon.View
{
    public partial class WindowsFormGame : Form
    {
        private ScalingSpoon.Model.Engine _model;
        private int focusedRobot = -1;
        public WindowsFormGame()
        {
            InitializeComponent();
        }

        private void WindowsFormGame_Load(object sender, EventArgs e)
        {
            _model = new Model.Engine();
            _model.ConstructBoard(16, 16, 18, 4);
            foreach (Cell c in _model.Board)
            {
                ButtonCell bc = new ButtonCell(c);
                bc.Click += ButtonCell_Click;
                bc.KeyDown += ButtonCell_KeyDown;
                this.Controls.Add(bc);
            }
            this.Size = new Size(530, 554);
        }

        private void ButtonCell_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.KeyCode == Keys.Up)
                _model.MoveRobot(focusedRobot, Direction.Left);
            if (e.KeyCode == Keys.Down)
                _model.MoveRobot(focusedRobot, Direction.Right);
            if (e.KeyCode == Keys.Right)
                _model.MoveRobot(focusedRobot, Direction.Down);
            if (e.KeyCode == Keys.Left)
                _model.MoveRobot(focusedRobot, Direction.Up);

            Refresh();
        }

        private void ButtonCell_Click(object sender, EventArgs e)
        {
            ButtonCell cell = sender as ButtonCell;
            if (cell == null)
                return;
            if (cell.GetCell().RobotID != -1)
                focusedRobot = cell.GetCell().RobotID;
        }

        private void WindowsFormGame_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
