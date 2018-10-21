using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScalingSpoon.View.Bus
{
    public class ButtonCell : Button
    {
        private Cell _cell = new Cell();
        private Dictionary<int, Color> _robotColors = new Dictionary<int, Color>();
        public ButtonCell()
        {
            
        }

        public ButtonCell(Cell c)
        {
            _cell = c;
            this.Width = 32;
            this.Height = 32;
            this.Location = new Point(c.X * 32, c.Y * 32);
            _robotColors.Add(0, Color.Red);
            _robotColors.Add(1, Color.Green);
            _robotColors.Add(2, Color.Purple);
            _robotColors.Add(3, Color.Blue);
        }

        public Cell GetCell()
        {
            return _cell;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Draw solid box in the middle
            if (_cell.Walls.HasFlag(CellWalls.Up) && _cell.Walls.HasFlag(CellWalls.Down) && _cell.Walls.HasFlag(CellWalls.Right) && _cell.Walls.HasFlag(CellWalls.Left))
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 32, 32));
                return;
            }

            // -  x
            // y
            if (_cell.Walls.HasFlag(CellWalls.Up)) //Up = left wall
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 3, 32));
            if (_cell.Walls.HasFlag(CellWalls.Down)) //Down = right wall
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(29, 0, 3, 32));
            if (_cell.Walls.HasFlag(CellWalls.Right)) //Right = down wall
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 29, 32, 3));
            if (_cell.Walls.HasFlag(CellWalls.Left)) //Left = Up wall
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 32, 3));

            if (_cell.RobotID != -1)
                e.Graphics.FillEllipse(new SolidBrush(_robotColors[_cell.RobotID]), 6, 6, 20, 20);

            DestinationCell dc = _cell as DestinationCell;
            if (dc != null && dc.CurrentWinningCell)
            {
                //This is the same SalesPad Promotions star character i believe.
                using (Font f = new Font("Tahoma", 16, FontStyle.Regular))
                using (Brush b = new SolidBrush(_robotColors[dc.WinningRobotId]))
                    e.Graphics.DrawString("★", f, b, 3, 3);
            }

            //Draw Solved Path onto the board.
            //Solved Path Lines.png - interesections are separated by a pixel, so they don't overlap on top of each other.
            // For some reason, the pixels seem to be 1 off when drawing onto the buttons? Not sure. Adding 1 cause them to line up.
            int tempRobotPath = _cell.RobotPath;
            for (int i = 0; i < 4; i++)
            {
                int xyCord = 12 + (i * 2);
                int leftLength = xyCord + 1;
                int upLength = xyCord + 1;
                int rightLength = 32 - leftLength + 1;
                int downLength = 32 - upLength + 1;

                if ((tempRobotPath & 0x1) == 0x1) //Up = left
                    e.Graphics.FillRectangle(new SolidBrush(_robotColors[i]), new Rectangle(0, xyCord, leftLength, 1));
                tempRobotPath = tempRobotPath >> 1;

                if ((tempRobotPath & 0x1) == 0x1) //Down = right
                    e.Graphics.FillRectangle(new SolidBrush(_robotColors[i]), new Rectangle(xyCord, xyCord, rightLength, 1));
                tempRobotPath = tempRobotPath >> 1;

                if ((tempRobotPath & 0x1) == 0x1) //Right = down
                    e.Graphics.FillRectangle(new SolidBrush(_robotColors[i]), new Rectangle(xyCord, xyCord, 1, downLength));
                tempRobotPath = tempRobotPath >> 1;

                if ((tempRobotPath & 0x1) == 0x1) //Left = up
                    e.Graphics.FillRectangle(new SolidBrush(_robotColors[i]), new Rectangle(xyCord, 0, 1, upLength));
                tempRobotPath = tempRobotPath >> 1;
            }

            if (_cell.Deflector != null)
            {
                switch (_cell.Deflector.DeflectorType)
                {
                    case DeflectorType.Backward:
                        e.Graphics.DrawLine(new Pen(_robotColors[_cell.Deflector.RobotID]), 0, 0, 32, 32);
                        break;
                    case DeflectorType.Forward:
                        e.Graphics.DrawLine(new Pen(_robotColors[_cell.Deflector.RobotID]), 0, 32, 32, 0);
                        break;
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left)
                return true;
            else
                return base.IsInputKey(keyData);
        }
    }
}
