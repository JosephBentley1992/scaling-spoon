using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingSpoon.Model.Bus
{
    public class Portal
    {
        public int RobotID;
        public Cell Exit;
        
        public Portal(int robotId, Cell exit)
        {
            RobotID = robotId;
            Exit = exit;
        }
    }
}
