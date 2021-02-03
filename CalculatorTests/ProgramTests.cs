using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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
            Assert.ThrowsException<ArgumentException>(() => Program.Compute(2, 2, "g"));
            Assert.ThrowsException<DivideByZeroException>(() => Program.Compute(2, 0, "/"));
        }

        [TestMethod()]
        public void AskNumberTest()
        {
            var input = new StringReader("1,23");
            Console.SetIn(input);
            Assert.AreEqual(Program.AskNumber("Testing numbers."), 1.23);
            //input = new StringReader("Testing");
            //Console.SetIn(input);
            //Assert.ThrowsException<System.OutOfMemoryException>(() => Program.AskNumber("Testing letters."));
        }
    }
}