using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCross.Memory.Views.Tests
{
    [TestClass]
    public class View0DTest
    {
        [TestMethod]
        public void View0DTest_Ctor_ArrayIndex()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var view = new View0D<int>(array, 2);
            Assert.AreEqual(19, view.Element);
            view.Element = 42;
            Assert.AreEqual(42, view.Element);
        }
    }
}
