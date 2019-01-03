using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCross.Memory.Views.Tests
{
    [TestClass]
    public class ReadOnlyView0DTest
    {
        [TestMethod]
        public void ReadOnlyView0DTest_Ctor_ArrayIndex()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var view = new ReadOnlyView0D<int>(array, 2);
            Assert.AreEqual(19, view.Element);
        }

        [TestMethod]
        public void ReadOnlyView0DTest_Ctor_ArrayIndex_Outside_Throws()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var exception = Assert.ThrowsException<IndexOutOfRangeException>(
                () => new ReadOnlyView0D<int>(array, 4));
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void ReadOnlyView0DTest_DangerousCreate_ObjectRef()
        {
            var obj = new TestObject(17, "ABC");
            var view = ReadOnlyView0D<int>.DangerousCreate(obj, obj.ReadOnlyInt);
            Assert.AreEqual(17, view.Element);
            obj.Int = 42;
            Assert.AreEqual(42, view.Element);
        }
    }
}
