using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingSpoon.View.Bus
{
    public class GameSettings
    {
        public Dictionary<int, Color> RobotColors { get; set; }
        public int DeflectorCount { get; set; }
        public int PortalCount { get; set; }
    }
}
