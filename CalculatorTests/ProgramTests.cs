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
        
            // I noticed that the AskNumber would cause an OutOfMemoryException if I called it as a test with the wrong parameter
            // since it would just keep asking forever. I tried to develop a test for this but didn't get it to work.
            //input = new StringReader("Testing");
            //Console.SetIn(input);
            //Assert.ThrowsException<System.OutOfMemoryException>(() => Program.AskNumber("Testing letters."));
        }

        [TestMethod()]
        public void HandleSpecialTest()
        {
            Assert.AreEqual(Program.HandleSpecial(42, ""), "42");
            Assert.AreEqual(Program.HandleSpecial(4, ""), "4");
            Assert.AreEqual(Program.HandleSpecial(42, "MARCUS"), "MARCUS");
            Assert.AreEqual(Program.HandleSpecial(84, "RICHARD"), "RICHARD");
            Assert.AreEqual(Program.HandleSpecial(666, "DEVIL"), "DEVIL");
        }

        [TestMethod()]
        public void AskCommandTest()
        {
            var input = new StringReader("TESTING");
            Console.SetIn(input);
            Assert.AreEqual(Program.AskCommand(), "TESTING");
        }
    }
}