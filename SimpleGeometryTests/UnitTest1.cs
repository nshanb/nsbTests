using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleGeometry;
using simpleGeometry;

namespace SimpleGeometryTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void TestMethodX()
        {
            PlayWithHiding1 x = new PlayWithHiding1() { X = 1 };
            Console.WriteLine($"PlayWithHiding1.X: {x.X}");
            PlayWithHiding y = x as PlayWithHiding;
            Console.WriteLine($"PlayWithHiding.X: {y.X}");

        }
        [TestMethod]
        public void TestMethodY()
        {
            PlayWithHiding1 x = new PlayWithHiding1() { Y = 1 };
            Console.WriteLine($"PlayWithHiding1.Y: {x.Y}");
            PlayWithHiding y = x as PlayWithHiding;
            Console.WriteLine($"PlayWithHiding.Y: {y.Y}");

        }
        [TestMethod]
        public void TestMethodZ()
        {
            PlayWithHiding1 x = new PlayWithHiding1();
            x.Z = 4;
            Console.WriteLine($"PlayWithHiding1.Z: {4}");
            PlayWithHiding y = x as PlayWithHiding;
            Console.WriteLine($"PlayWithHiding.Z: {y.Z}");

        }
    }
}
