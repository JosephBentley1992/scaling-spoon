using ScalingSpoon.Model.Bus;
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
            if (_cell.HasNorthWall && _cell.HasSouthWall && _cell.HasEastWall && _cell.HasWestWall)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 32, 32));
                return;
            }

            if (_cell.HasNorthWall)
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 3, 32));
            if (_cell.HasSouthWall)
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(29, 0, 3, 32));
            if (_cell.HasEastWall)
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 29, 32, 3));
            if (_cell.HasWestWall)
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
