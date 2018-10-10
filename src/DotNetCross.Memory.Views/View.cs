using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetCross.Memory.Views
{
    // inspiration
    //
    // This class exists solely so that arbitrary objects can be Unsafe-casted to it to get a ref to the start of the user data.
    //
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class Pinnable<T>
    {
        public T Data;
    }

    public readonly struct View<T>
    {
        readonly Pinnable<T> _pinnable;
        readonly UIntPtr _byteOffsetOrPointer;

        public View(T[] array, int start)
        {
            throw new NotImplementedException();
            //if (array == null)
            //    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            //if (default(T) == null && array.GetType() != typeof(T[]))
            //    ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            //int arrayLength = array.Length;
            //if ((uint)start > (uint)arrayLength)
            //    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            //_pinnable = Unsafe.As<Pinnable<T>>(array);
            //_byteOffsetOrPointer = SpanHelpers.PerTypeValues<T>.ArrayAdjustment.Add<T>(start);
        }

        // For when from object member
        // public View(object obj, ref T memberRef)
        // https://stackoverflow.com/questions/1128315/find-size-of-object-instance-in-bytes-in-c-sharp
        // https://github.com/CyberSaving/MemoryUsage/blob/master/Main/Program.cs


        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7-3
        public ref T GetPinnableReference()
        {
            throw new NotImplementedException();
            //if (_pinnable == null)
            //    unsafe { return ref Unsafe.AsRef<T>(_byteOffsetOrPointer.ToPointer()); }
            //else
            //    return ref Unsafe.AddByteOffset<T>(ref _pinnable.Data, _byteOffsetOrPointer);
        }
        // Unsafe.AddByteOffset(); // Need version that takes object and UIntPtr
    }



    // https://github.com/nietras/corefx/blob/63f9e6d1c42d31d5ce6af978a29e518ee43dc811/src/System.Memory/src/System/Span.cs

    // http://mattwarren.org/2017/05/08/Arrays-and-the-CLR-a-Very-Special-Relationship/
    // https://windowsdebugging.wordpress.com/2012/04/07/memorylayoutofarrays/
    // https://windowsdebugging.wordpress.com/2012/04/24/memorylayoutofarraysx64/
    // https://blogs.msdn.microsoft.com/seteplia/2017/09/12/managed-object-internals-part-3-the-layout-of-a-managed-array-3/
    // Seems the size of the array is stored as a set of int32s, not want
    // I wanted, wanted intptr.
}
