using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DotNetCross.Memory.Views
{
    // inspiration
    // Fast/Portable Span Factoring PR
    // https://github.com/dotnet/corefx/pull/26667
    //
    // This class exists solely so that arbitrary objects can be Unsafe-casted to it to get a ref to the start of the user data.
    //

    // Span API
    // https://github.com/dotnet/corefx/blob/7a8b3b9e837aefcf60ad52588e773870325fcf45/src/System.Runtime/ref/System.Runtime.cs#L2221

    // For when from object member
    // public View(object obj, ref T memberRef)
    // https://stackoverflow.com/questions/1128315/find-size-of-object-instance-in-bytes-in-c-sharp
    // https://github.com/CyberSaving/MemoryUsage/blob/master/Main/Program.cs

    // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7-3


    public readonly struct ViewND<T>
    {
        readonly Pinnable<T> _pinnable;
        readonly IntPtr _byteOffsetOrPointerToData;
        readonly IntPtr _byteOffsetOrPointerToElementStrides;
        readonly int _rank;
    }


    // https://github.com/nietras/corefx/blob/63f9e6d1c42d31d5ce6af978a29e518ee43dc811/src/System.Memory/src/System/Span.cs

    // http://mattwarren.org/2017/05/08/Arrays-and-the-CLR-a-Very-Special-Relationship/
    // https://windowsdebugging.wordpress.com/2012/04/07/memorylayoutofarrays/
    // https://windowsdebugging.wordpress.com/2012/04/24/memorylayoutofarraysx64/
    // https://blogs.msdn.microsoft.com/seteplia/2017/09/12/managed-object-internals-part-3-the-layout-of-a-managed-array-3/
    // https://blogs.msdn.microsoft.com/seteplia/2017/09/21/managed-object-internals-part-4-fields-layout/
    // Part 4 shows how to get the layout using reflection and IL emit!
    // https://github.com/SergeyTeplyakov/ObjectLayoutInspector
}
