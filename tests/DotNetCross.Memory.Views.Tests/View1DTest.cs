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
#if HASSPAN
            var span = view.AsSpan();
            ref int s0 = ref span[0];
#endif
        }
    }
}
