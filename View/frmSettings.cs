using ScalingSpoon.View.Bus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScalingSpoon.View
{
    public partial class frmSettings : Form
    {
        private GameSettings _settings = new GameSettings();
        public frmSettings(GameSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            btnRobotColor1.BackColor = _settings.RobotColors[0];
            btnRobotColor2.BackColor = _settings.RobotColors[1];
            btnRobotColor3.BackColor = _settings.RobotColors[2];
            btnRobotColor4.BackColor = _settings.RobotColors[3];
            nudDeflectorCount.Value = _settings.DeflectorCount;
            nudPortalCount.Value = _settings.PortalCount;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _settings.RobotColors[0] = btnRobotColor1.BackColor;
            _settings.RobotColors[1] = btnRobotColor2.BackColor;
            _settings.RobotColors[2] = btnRobotColor3.BackColor;
            _settings.RobotColors[3] = btnRobotColor4.BackColor;
            _settings.DeflectorCount = (int)nudDeflectorCount.Value;
            _settings.PortalCount = (int)nudPortalCount.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRobotColor_Click(object sender, EventArgs e)
        {
            ButtonCell robotButton = sender as ButtonCell;
            if (robotButton == null)
                return;

            ColorDialog colorChooser = new ColorDialog();
            colorChooser.AllowFullOpen = true;
            colorChooser.ShowHelp = true;
            colorChooser.Color = robotButton.BackColor;

            // Update the text box color if the user clicks OK 
            if (colorChooser.ShowDialog() == DialogResult.OK)
                robotButton.BackColor = colorChooser.Color;
        }
    }
}
