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
        public ButtonCell()
        {
            
        }

        public ButtonCell(Cell c)
        {
            _cell = c;
            this.Width = 32;
            this.Height = 32;
            this.Location = new Point(c.X * 32, c.Y * 32);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            SolidBrush b = new SolidBrush(Color.Black);

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

            if (_cell.RobotID == 0)
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), 6, 6, 20, 20);
            if (_cell.RobotID == 1)
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue), 6, 6, 20, 20);
            if (_cell.RobotID == 2)
                e.Graphics.FillEllipse(new SolidBrush(Color.Yellow), 6, 6, 20, 20);
            if (_cell.RobotID == 3)
                e.Graphics.FillEllipse(new SolidBrush(Color.Green), 6, 6, 20, 20);
        }
    }
}
