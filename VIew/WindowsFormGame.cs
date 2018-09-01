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
            NewGame();
        }

        private void NewGame()
        {
            if (_model != null)
            {
                List< System.Windows.Forms.Control> controlsToRemove = new List<System.Windows.Forms.Control>();
                foreach (System.Windows.Forms.Control c in this.Controls)
                {
                    if (c is ButtonCell)
                        controlsToRemove.Add(c);
                }
                foreach (System.Windows.Forms.Control c in controlsToRemove)
                    this.Controls.Remove(c);
            }

            _model = new Model.Engine();
            _model.ConstructBoard(16, 16, 18, 4);
            foreach (Cell c in _model.Board)
            {
                ButtonCell bc = new ButtonCell(c);
                bc.Click += ButtonCell_Click;
                bc.KeyDown += ButtonCell_KeyDown;
                this.Controls.Add(bc);
            }

            StringBuilder sb = new StringBuilder();
            int x1 = _model.WinningDestinations.GroupBy(c => c.X).Select(c => c.Count()).Count(c => c == 1);
            int x2 = _model.WinningDestinations.GroupBy(c => c.X).Select(c => c.Count()).Count(c => c == 2);
            int y1 = _model.WinningDestinations.GroupBy(c => c.Y).Select(c => c.Count()).Count(c => c == 1);
            int y2 = _model.WinningDestinations.GroupBy(c => c.Y).Select(c => c.Count()).Count(c => c == 2);
            int q1 = _model.WinningDestinations.Count(c => c.X > 1 && c.X < 8 && c.Y > 1 && c.Y < 8);
            int q2 = _model.WinningDestinations.Count(c => c.X >= 8 && c.X < 15 && c.Y > 1 && c.Y < 8);
            int q3 = _model.WinningDestinations.Count(c => c.X > 1 && c.X < 8 && c.Y >= 8 && c.Y < 15);
            int q4 = _model.WinningDestinations.Count(c => c.X >= 8 && c.X < 15 && c.Y >= 8 && c.Y < 15);

            sb.AppendLine(String.Format("Blank rows: {0}", 16 - x2 - x1 - 2));
            sb.AppendLine(String.Format("2 rows: {0}", x2));
            sb.AppendLine(String.Format("Blank cols: {0}", 16 - y2 - y1 - 2));
            sb.AppendLine(String.Format("2 cols: {0}", y2));
            sb.AppendLine(String.Format("Q1: {0}", q1));
            sb.AppendLine(String.Format("Q2: {0}", q2));
            sb.AppendLine(String.Format("Q3: {0}", q3));
            sb.AppendLine(String.Format("Q4: {0}", q4));
            txtGameDescription.Text = sb.ToString();
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

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            NewGame();
            Refresh();
        }
    }
}
