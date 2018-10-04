using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScalingSpoon.Model;
using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System.Collections.Generic;
using System.IO;

namespace ScalingSpoonTests
{
    public abstract class BaseTest
    {
        public FileStream ostrm;
        public StreamWriter writer;
        public TextWriter oldOut = Console.Out;

        [TestInitialize()]
        public void TestInitialize()
        {
            try
            {
                ostrm = new FileStream("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }

            Console.SetOut(writer);
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Done");
        }
    }
}
