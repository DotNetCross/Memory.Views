 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace IHFood.Numerics
{
    public readonly struct View1D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;
        // .NET has for good chosen int as size, would have preferred IntPtr e.g. nint
        // for length but this would give issues with interoperating with BCL
        readonly int _length0;

        public View1D(T[] array)
        {
            
        }
    }
    public readonly struct View2D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;
        readonly IntPtr _byteStride0;
        // .NET has for good chosen int as size, would have preferred IntPtr e.g. nint
        // for length but this would give issues with interoperating with BCL
        readonly int _length0;
        readonly int _length1;

        public View2D(T[,] array)
        {
            
        }
    }
    public readonly struct View3D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;
        readonly IntPtr _byteStride0;
        readonly IntPtr _byteStride1;
        // .NET has for good chosen int as size, would have preferred IntPtr e.g. nint
        // for length but this would give issues with interoperating with BCL
        readonly int _length0;
        readonly int _length1;
        readonly int _length2;

        public View3D(T[,,] array)
        {
            
        }
    }
    public readonly struct View4D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;
        readonly IntPtr _byteStride0;
        readonly IntPtr _byteStride1;
        readonly IntPtr _byteStride2;
        // .NET has for good chosen int as size, would have preferred IntPtr e.g. nint
        // for length but this would give issues with interoperating with BCL
        readonly int _length0;
        readonly int _length1;
        readonly int _length2;
        readonly int _length3;

        public View4D(T[,,,] array)
        {
            
        }
    }
    public readonly struct View5D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;
        readonly IntPtr _byteStride0;
        readonly IntPtr _byteStride1;
        readonly IntPtr _byteStride2;
        readonly IntPtr _byteStride3;
        // .NET has for good chosen int as size, would have preferred IntPtr e.g. nint
        // for length but this would give issues with interoperating with BCL
        readonly int _length0;
        readonly int _length1;
        readonly int _length2;
        readonly int _length3;
        readonly int _length4;

        public View5D(T[,,,,] array)
        {
            
        }
    }
}