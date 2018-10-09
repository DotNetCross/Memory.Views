using System;
using System.Runtime.CompilerServices;

namespace DotNetCross.Memory.Views
{
    // inspiration

    public readonly struct View<T>
    {
        readonly object _object;
        readonly UIntPtr _offsetOrPointer;

        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7-3
        public ref T GetPinnableReference() => Unsafe.AddByteOffset(); // Need version that takes object and UIntPtr
    }

    // http://mattwarren.org/2017/05/08/Arrays-and-the-CLR-a-Very-Special-Relationship/
    // https://windowsdebugging.wordpress.com/2012/04/07/memorylayoutofarrays/
    // https://windowsdebugging.wordpress.com/2012/04/24/memorylayoutofarraysx64/
    // https://blogs.msdn.microsoft.com/seteplia/2017/09/12/managed-object-internals-part-3-the-layout-of-a-managed-array-3/
    // Seems the size of the array is stored as a set of int32s, not want
    // I wanted, wanted intptr.
}
