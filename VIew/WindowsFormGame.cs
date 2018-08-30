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
namespace ScalingSpoon.View
{
    public partial class WindowsFormGame : Form
    {
        public WindowsFormGame()
        {
            InitializeComponent();
        }

        private void WindowsFormGame_Load(object sender, EventArgs e)
        {
            ScalingSpoon.Model.Engine engine = new Model.Engine();
            engine.ConstructBoard(16, 16, 16, 4);
            foreach (Cell c in engine.Board)
                this.Controls.Add(new ButtonCell(c));

            /*
            this.Controls.Add(new ButtonCell(new Cell(0, true, false, false, false, 0, 0)));
            this.Controls.Add(new ButtonCell(new Cell(1, false, true, false, false, 1, 0)));
            this.Controls.Add(new ButtonCell(new Cell(2, false, false, true, false, 2, 0)));
            this.Controls.Add(new ButtonCell(new Cell(3, false, false, false, true, 3, 0)));
            this.Controls.Add(new ButtonCell(new Cell(4, true, true, false, false, 4, 1)));
            this.Controls.Add(new ButtonCell(new Cell(5, false, true, true, false, 5, 2)));
            this.Controls.Add(new ButtonCell(new Cell(6, false, false, true, true, 6, 3)));
            this.Controls.Add(new ButtonCell(new Cell(7, true, false, false, true, 7, 4)));

            Cell c = new Cell(8, true, true, false, false, 10, 10);
            c.RobotID = 0;
            this.Controls.Add(new ButtonCell(c));
            */
        }
    }
}
