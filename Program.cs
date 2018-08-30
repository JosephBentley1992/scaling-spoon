﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScalingSpoon.Model;
using ScalingSpoon.View;

namespace ScalingSpoon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            //Engine e = new Engine();
            //e.ConstructBoard(16, 16, 16, 4);
            //e.WriteBoardToConsole();
            //Console.ReadLine();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WindowsFormGame());
        }
    }
}
