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

        [TestMethod]
        public void View0DTest_Ctor_ArrayIndex_Outside_Throws()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var exception = Assert.ThrowsException<IndexOutOfRangeException>(
                () => new View0D<int>(array, 4));
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void View0DTest_DangerousCreate_ObjectRef()
        {
            var obj = new TestObject(17, "ABC");
            var view = View0D<int>.DangerousCreate(obj, ref obj.Int);
            Assert.AreEqual(17, view.Element);
            view.Element = 23;
            Assert.AreEqual(23, obj.Int);
            obj.Int = 42;
            Assert.AreEqual(42, view.Element);
        }
    }
}
