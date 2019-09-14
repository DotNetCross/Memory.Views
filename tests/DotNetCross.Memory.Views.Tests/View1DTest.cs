using System;
using System.Runtime.InteropServices;
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

        [Fact]
        public unsafe void View1DTest_Ctor_MultidimensionalArray()
        {
            var array = new int[,] { { 17, 18, 19, 20 }, { 21, 22, 23, 24 }, };
            var oldview = OldView1D<int>.DangerousCreate(array, ref array[0, 2], 4);
            var view = View1D<int>.DangerousCreate(array, ref array[0, 2], 4);
            Assert.Equal(4,  view.Length);
            Assert.Equal(19, view[0]);
            Assert.Equal(20, view[1]);
            Assert.Equal(21, view[2]);
            Assert.Equal(22, view[3]);
        }

#if HASSPAN
        [Fact]
        public unsafe void View1DTest_AsSpan_MultidimensionalArray()
        {
            var array = new int[,] { { 17, 18, 19, 20 }, { 21, 22, 23, 24}, };
            var view = View1D<int>.DangerousCreate(array, ref array[0, 2], 4);
            var span = view.AsSpan();
            Assert.Equal(4, span.Length);
            Assert.Equal(19, span[0]);
            Assert.Equal(20, span[1]);
            Assert.Equal(21, span[2]);
            Assert.Equal(22, span[3]);
        }
#else
        [Fact]
        public unsafe void View1DTest_AsSpan_MultidimensionalArray()
        {
            var array = new int[,] { { 17, 18, 19, 20 }, { 21, 22, 23, 24}, };
            var view = View1D<int>.DangerousCreate(array, ref array[0, 2], 4);
            var e = Assert.Throws<NotSupportedException>(() => { var span = view.AsSpan(); });
            Assert.Equal("View not supported by Span e.g. Span cannot be created for a multi-dimensional array, because Span constructors suck!", 
                e.Message);
        }
#endif
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
    }
}
