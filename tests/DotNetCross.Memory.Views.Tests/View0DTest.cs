using System;
using Xunit;

namespace DotNetCross.Memory.Views.Tests
{
    public class View0DTest
    {
        [Fact]
        public void View0DTest_Ctor_ArrayIndex()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var view = new View0D<int>(array, 2);
            Assert.Equal(19, view.Element);
            view.Element = 42;
            Assert.Equal(42, view.Element);
        }

        [Fact]
        public void View0DTest_Ctor_ArrayIndex_Outside_Throws()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var exception = Assert.Throws<IndexOutOfRangeException>(
                () => new View0D<int>(array, 4));
            Assert.NotNull(exception);
        }

        [Fact]
        public void View0DTest_DangerousCreate_ObjectRef()
        {
            var obj = new TestObject(17, "ABC");
            var view = View0D<int>.DangerousCreate(obj, ref obj.Int);
            Assert.Equal(17, view.Element);
            view.Element = 23;
            Assert.Equal(23, obj.Int);
            obj.Int = 42;
            Assert.Equal(42, view.Element);
        }
    }
}
