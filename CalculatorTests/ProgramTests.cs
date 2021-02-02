using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void ComputeTest()
        {
            Assert.AreEqual(Program.Compute(2, 4, "+"), 6);
            Assert.AreEqual(Program.Compute(11, 4, "-"), 7);
            Assert.AreEqual(Program.Compute(5, 4, "*"), 20);
            Assert.AreEqual(Program.Compute(15, 3, "/"), 5);
            Assert.IsNull(Program.Compute(2, 2, "g"));
        }
    }
}