using System;
using Xunit;

namespace DotNetCross.Memory.Views.Tests
{
    public class View1DTest
    {
        [Fact]
        public void View1DTest_Ctor_ArraySlice()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var view = new View1D<int>(array, 1, 2);
            Assert.Equal(2, view.Length);
            ref int v0 = ref view[0];
            ref int v1 = ref view[1];
            Assert.Equal(18, v0);
            Assert.Equal(19, v1);
            var span = view.AsSpan();
            ref int s0 = ref span[0];
        }

#if HASSPAN
        [Fact]
        public void View1DTest_AsSpan_Array()
        {
            var array = new int[] { 17, 18, 19, 20 };
            var view = new View1D<int>(array, 1, 2);
            var span = view.AsSpan();
            Assert.Equal(2, span.Length);
            Assert.Equal(18, span[0]);
            Assert.Equal(19, span[1]);
        }

        [Fact]
        public unsafe void View1DTest_AsSpan_Pointer()
        {
            var array = new int[] { 17, 18, 19, 20 };
            fixed (int* ptr = array)
            {
                var view = new View1D<int>(ptr + 1, 2);
                var span = view.AsSpan();
                Assert.Equal(2, span.Length);
                Assert.Equal(18, span[0]);
                Assert.Equal(19, span[1]);
            }
        }
#endif
    }
}
