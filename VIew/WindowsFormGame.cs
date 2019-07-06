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
using ScalingSpoon.Model;

namespace ScalingSpoon.View
{
    public partial class WindowsFormGame : Form
    {
        private Engine _model;
        private int focusedRobot = -1;
        private GameSettings _settings = new GameSettings();

        public WindowsFormGame()
        {
            InitializeComponent();
            _settings = new GameSettings();
            _settings.DeflectorCount = 0;
            _settings.PortalCount = 0;
            _settings.RobotColors = new Dictionary<int, Color>()
            {
                { 0, Color.Red },
                { 1, Color.Green },
                { 2, Color.Purple },
                { 3, Color.Blue }
            };
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
            _model.ConstructBoard(16, 16, 16, 4, _settings.DeflectorCount, _settings.PortalCount / 2);
            _model.AutoSetNextWinningDestination = false;
            foreach (Cell c in _model.Board)
            {
                ButtonCell bc = new ButtonCell(c, boardLocation, _settings);
                bc.Click += ButtonCell_Click;
                bc.KeyDown += ButtonCell_KeyDown;
                this.Controls.Add(bc);
            }

            StringBuilder sb = new StringBuilder();
            int x1 = _model.WinningDestinations.GroupBy(c => c.X).Select(c => c.Count()).Count(c => c == 1);
            int x2 = _model.WinningDestinations.GroupBy(c => c.X).Select(c => c.Count()).Count(c => c == 2);
            int y1 = _model.WinningDestinations.GroupBy(c => c.Y).Select(c => c.Count()).Count(c => c == 1);
            int y2 = _model.WinningDestinations.GroupBy(c => c.Y).Select(c => c.Count()).Count(c => c == 2);
            int q1 = _model.WinningDestinations.Count(c => c.X > 0 && c.X < 8 && c.Y > 0 && c.Y < 8);
            int q2 = _model.WinningDestinations.Count(c => c.X >= 8 && c.X < 15 && c.Y > 0 && c.Y < 8);
            int q3 = _model.WinningDestinations.Count(c => c.X > 0 && c.X < 8 && c.Y >= 8 && c.Y < 15);
            int q4 = _model.WinningDestinations.Count(c => c.X >= 8 && c.X < 15 && c.Y >= 8 && c.Y < 15);

            sb.AppendLine(String.Format("Blank rows: {0}", 16 - x2 - x1 - 2));
            sb.AppendLine(String.Format("2 rows: {0}", x2));
            sb.AppendLine(String.Format("Blank cols: {0}", 16 - y2 - y1 - 2));
            sb.AppendLine(String.Format("2 cols: {0}", y2));
            sb.AppendLine(String.Format("Q1: {0}", q1));
            sb.AppendLine(String.Format("Q2: {0}", q2));
            sb.AppendLine(String.Format("Q3: {0}", q3));
            sb.AppendLine(String.Format("Q4: {0}", q4));

            txtSolutionPath.Font = new Font(txtSolutionPath.Font.FontFamily, (float)8.75);
            txtSolutionPath.Text = sb.ToString();

            btnRobot0.BackColor = _settings.RobotColors[0];
            btnRobot0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRobot0.FlatAppearance.BorderSize = 0;

            btnRobot1.BackColor = _settings.RobotColors[1];
            btnRobot1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRobot1.FlatAppearance.BorderSize = 0;

            btnRobot2.BackColor = _settings.RobotColors[2];
            btnRobot2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRobot2.FlatAppearance.BorderSize = 0;

            btnRobot3.BackColor = _settings.RobotColors[3];
            btnRobot3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRobot3.FlatAppearance.BorderSize = 0;
        }

        public override void Refresh()
        {
            btnNextPosition.Visible = _model.AtWinningDestiation();
            txtNumberOfMoves.Text = _model.CurrentWinningDestination.MoveHistory.Count().ToString();
            base.Refresh();
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


        private void btnSolve_Click(object sender, EventArgs e)
        {
            txtSolutionPath.Clear();
            foreach (Cell c in _model.Board)
                c.RobotPath = 0;

            txtSolutionPath.Font = new Font(txtSolutionPath.Font.FontFamily, (float)42.0);
            List<RobotMove> movesToWin = new GameSolverBreadthFirst(_model).FindSolution();
            if (movesToWin == null || movesToWin.Count == 0)
            {
                txtSolvedNumberOfMoves.Text = ">12";
                movesToWin = new GameSolverDepthFirst(_model).FindSolution();
                if (movesToWin == null || movesToWin.Count == 0)
                    txtSolvedNumberOfMoves.Text = "???";
            }
            txtSolvedNumberOfMoves.Text = movesToWin.Count.ToString();
            foreach (RobotMove move in movesToWin)
            {
                if (move.Direction == Direction.Up)
                    txtSolutionPath.AppendText("⇦", _settings.RobotColors[move.RobotId]);
                if (move.Direction == Direction.Down)
                    txtSolutionPath.AppendText("⇨", _settings.RobotColors[move.RobotId]);
                if (move.Direction == Direction.Right)
                    txtSolutionPath.AppendText("⇩", _settings.RobotColors[move.RobotId]);
                if (move.Direction == Direction.Left)
                    txtSolutionPath.AppendText("⇧", _settings.RobotColors[move.RobotId]);
            }

            _model.AutoSetRobotPath = true;
            foreach (RobotMove move in movesToWin)
                _model.MoveRobot(move.RobotId, move.Direction);
            _model.AutoSetRobotPath = false;

            foreach (RobotMove move in movesToWin)
                _model.UndoMove();

            Refresh();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _model.ResetCurrentWinningPosition();
            Refresh();
        }

        private void btnUndoMove_Click(object sender, EventArgs e)
        {
            _model.UndoMove();
            Refresh();
        }

        private void btnNextPosition_Click(object sender, EventArgs e)
        {
            _model.SetNextWinningDestination();
            txtSolvedNumberOfMoves.Text = String.Empty;
            txtSolutionPath.Text = String.Empty;

            foreach (Cell c in _model.Board)
                c.RobotPath = 0;

            Refresh();
        }

        private void btnRobot0_Click(object sender, EventArgs e)
        {
            focusedRobot = 0;
        }

        private void btnRobot1_Click(object sender, EventArgs e)
        {
            focusedRobot = 1;
        }

        private void btnRobot2_Click(object sender, EventArgs e)
        {
            focusedRobot = 2;
        }

        private void btnRobot3_Click(object sender, EventArgs e)
        {
            focusedRobot = 3;
        }
        

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            Refresh();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new frmSettings(_settings))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                    this.NewGame();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
