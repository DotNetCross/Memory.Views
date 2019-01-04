using System;
using Xunit;

namespace DotNetCross.Memory.Views.Tests
{
    public class ReadOnlyView0DTest
    {
        [Fact]
        public void ReadOnlyView0DTest_Ctor_ArrayIndex()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var view = new ReadOnlyView0D<int>(array, 2);
            Assert.Equal(19, view.Element);
        }

        [Fact]
        public void ReadOnlyView0DTest_Ctor_ArrayIndex_Outside_Throws()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var exception = Assert.Throws<IndexOutOfRangeException>(
                () => new ReadOnlyView0D<int>(array, 4));
            Assert.NotNull(exception);
        }

        [Fact]
        public void ReadOnlyView0DTest_DangerousCreate_ObjectRef()
        {
            var obj = new TestObject(17, "ABC");
            var view = ReadOnlyView0D<int>.DangerousCreate(obj, obj.ReadOnlyInt);
            Assert.Equal(17, view.Element);
            obj.Int = 42;
            Assert.Equal(42, view.Element);
        }
    }
}
