 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace DotNetCross.Memory.Views
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct View1D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;
        // .NET has for good chosen int as size, would have preferred IntPtr e.g. nint
        // for length but this would give issues with interoperating with BCL
        readonly int _length0;

        public View1D(T[] array)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment1D;
            _length0 = array.GetLength(0);
        }

        public View1D(T[] array, int start0)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment1D;
            _length0 = array.GetLength(0);
            if ((uint)start0 > (uint)_length0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add<T>(start0);
        }

        public View1D(T[] array, int start0,
            int length0)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment1D;
            _length0 = array.GetLength(0);
            if ((uint)start0 > (uint)_length0 || (uint)length0 > (uint)(_length0 - start0))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add<T>(start0);
            _length0 = length0;
        }
        public ref T this[int index0]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            //[NonVersionable]
            get
            {
                if ((uint)index0 >= (uint)_length0)
                    ThrowHelper.ThrowIndexOutOfRangeException();

                return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                    _byteOffsetOrPointer
                        .Add<T>(_length0)
                    );
            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
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
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment2D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _byteStride0 = new IntPtr(_length1).Multiply(Unsafe.SizeOf<T>());
        }

        public View2D(T[,] array, int start0, int start1)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment2D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _byteStride0 = new IntPtr(_length1).Multiply(Unsafe.SizeOf<T>());
            if ((uint)start0 > (uint)_length0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add<T>(start1);
        }

        public View2D(T[,] array, int start0, int start1,
            int length0, int length1)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment2D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _byteStride0 = new IntPtr(_length1).Multiply(Unsafe.SizeOf<T>());
            if ((uint)start0 > (uint)_length0 || (uint)length0 > (uint)(_length0 - start0))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1 || (uint)length1 > (uint)(_length1 - start1))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add<T>(start1);
            _length0 = length0;
            _length1 = length1;
        }
        public ref T this[int index0, int index1]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            //[NonVersionable]
            get
            {
                if ((uint)index0 >= (uint)_length0 ||
                    (uint)index1 >= (uint)_length1)
                    ThrowHelper.ThrowIndexOutOfRangeException();

                return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                    _byteOffsetOrPointer
                        .Add(_byteStride0.Multiply(_length0))
                        .Add<T>(_length1)
                    );
            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
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
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment3D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _byteStride1 = new IntPtr(_length2).Multiply(Unsafe.SizeOf<T>());
            _byteStride0 = _byteStride1.Multiply(_length1);
        }

        public View3D(T[,,] array, int start0, int start1, int start2)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment3D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _byteStride1 = new IntPtr(_length2).Multiply(Unsafe.SizeOf<T>());
            _byteStride0 = _byteStride1.Multiply(_length1);
            if ((uint)start0 > (uint)_length0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            if ((uint)start2 > (uint)_length2)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start2);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add(_byteStride1.Multiply(start1))
                .Add<T>(start2);
        }

        public View3D(T[,,] array, int start0, int start1, int start2,
            int length0, int length1, int length2)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment3D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _byteStride1 = new IntPtr(_length2).Multiply(Unsafe.SizeOf<T>());
            _byteStride0 = _byteStride1.Multiply(_length1);
            if ((uint)start0 > (uint)_length0 || (uint)length0 > (uint)(_length0 - start0))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1 || (uint)length1 > (uint)(_length1 - start1))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            if ((uint)start2 > (uint)_length2 || (uint)length2 > (uint)(_length2 - start2))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start2);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add(_byteStride1.Multiply(start1))
                .Add<T>(start2);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
        }
        public ref T this[int index0, int index1, int index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            //[NonVersionable]
            get
            {
                if ((uint)index0 >= (uint)_length0 ||
                    (uint)index1 >= (uint)_length1 ||
                    (uint)index2 >= (uint)_length2)
                    ThrowHelper.ThrowIndexOutOfRangeException();

                return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                    _byteOffsetOrPointer
                        .Add(_byteStride0.Multiply(_length0))
                        .Add(_byteStride1.Multiply(_length1))
                        .Add<T>(_length2)
                    );
            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
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
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment4D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _length3 = array.GetLength(3);
            _byteStride2 = new IntPtr(_length3).Multiply(Unsafe.SizeOf<T>());
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
        }

        public View4D(T[,,,] array, int start0, int start1, int start2, int start3)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment4D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _length3 = array.GetLength(3);
            _byteStride2 = new IntPtr(_length3).Multiply(Unsafe.SizeOf<T>());
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
            if ((uint)start0 > (uint)_length0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            if ((uint)start2 > (uint)_length2)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start2);
            if ((uint)start3 > (uint)_length3)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start3);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add(_byteStride1.Multiply(start1))
                .Add(_byteStride2.Multiply(start2))
                .Add<T>(start3);
        }

        public View4D(T[,,,] array, int start0, int start1, int start2, int start3,
            int length0, int length1, int length2, int length3)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment4D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _length3 = array.GetLength(3);
            _byteStride2 = new IntPtr(_length3).Multiply(Unsafe.SizeOf<T>());
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
            if ((uint)start0 > (uint)_length0 || (uint)length0 > (uint)(_length0 - start0))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1 || (uint)length1 > (uint)(_length1 - start1))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            if ((uint)start2 > (uint)_length2 || (uint)length2 > (uint)(_length2 - start2))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start2);
            if ((uint)start3 > (uint)_length3 || (uint)length3 > (uint)(_length3 - start3))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start3);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add(_byteStride1.Multiply(start1))
                .Add(_byteStride2.Multiply(start2))
                .Add<T>(start3);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
        }
        public ref T this[int index0, int index1, int index2, int index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            //[NonVersionable]
            get
            {
                if ((uint)index0 >= (uint)_length0 ||
                    (uint)index1 >= (uint)_length1 ||
                    (uint)index2 >= (uint)_length2 ||
                    (uint)index3 >= (uint)_length3)
                    ThrowHelper.ThrowIndexOutOfRangeException();

                return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                    _byteOffsetOrPointer
                        .Add(_byteStride0.Multiply(_length0))
                        .Add(_byteStride1.Multiply(_length1))
                        .Add(_byteStride2.Multiply(_length2))
                        .Add<T>(_length3)
                    );
            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
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
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment5D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _length3 = array.GetLength(3);
            _length4 = array.GetLength(4);
            _byteStride3 = new IntPtr(_length4).Multiply(Unsafe.SizeOf<T>());
            _byteStride2 = _byteStride3.Multiply(_length3);
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
        }

        public View5D(T[,,,,] array, int start0, int start1, int start2, int start3, int start4)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment5D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _length3 = array.GetLength(3);
            _length4 = array.GetLength(4);
            _byteStride3 = new IntPtr(_length4).Multiply(Unsafe.SizeOf<T>());
            _byteStride2 = _byteStride3.Multiply(_length3);
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
            if ((uint)start0 > (uint)_length0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            if ((uint)start2 > (uint)_length2)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start2);
            if ((uint)start3 > (uint)_length3)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start3);
            if ((uint)start4 > (uint)_length4)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start4);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add(_byteStride1.Multiply(start1))
                .Add(_byteStride2.Multiply(start2))
                .Add(_byteStride3.Multiply(start3))
                .Add<T>(start4);
        }

        public View5D(T[,,,,] array, int start0, int start1, int start2, int start3, int start4,
            int length0, int length1, int length2, int length3, int length4)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment5D;
            _length0 = array.GetLength(0);
            _length1 = array.GetLength(1);
            _length2 = array.GetLength(2);
            _length3 = array.GetLength(3);
            _length4 = array.GetLength(4);
            _byteStride3 = new IntPtr(_length4).Multiply(Unsafe.SizeOf<T>());
            _byteStride2 = _byteStride3.Multiply(_length3);
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
            if ((uint)start0 > (uint)_length0 || (uint)length0 > (uint)(_length0 - start0))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start0);
            if ((uint)start1 > (uint)_length1 || (uint)length1 > (uint)(_length1 - start1))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start1);
            if ((uint)start2 > (uint)_length2 || (uint)length2 > (uint)(_length2 - start2))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start2);
            if ((uint)start3 > (uint)_length3 || (uint)length3 > (uint)(_length3 - start3))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start3);
            if ((uint)start4 > (uint)_length4 || (uint)length4 > (uint)(_length4 - start4))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start4);
            _byteOffsetOrPointer = _byteOffsetOrPointer
                .Add(_byteStride0.Multiply(start0))
                .Add(_byteStride1.Multiply(start1))
                .Add(_byteStride2.Multiply(start2))
                .Add(_byteStride3.Multiply(start3))
                .Add<T>(start4);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
            _length4 = length4;
        }
        public ref T this[int index0, int index1, int index2, int index3, int index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            //[NonVersionable]
            get
            {
                if ((uint)index0 >= (uint)_length0 ||
                    (uint)index1 >= (uint)_length1 ||
                    (uint)index2 >= (uint)_length2 ||
                    (uint)index3 >= (uint)_length3 ||
                    (uint)index4 >= (uint)_length4)
                    ThrowHelper.ThrowIndexOutOfRangeException();

                return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                    _byteOffsetOrPointer
                        .Add(_byteStride0.Multiply(_length0))
                        .Add(_byteStride1.Multiply(_length1))
                        .Add(_byteStride2.Multiply(_length2))
                        .Add(_byteStride3.Multiply(_length3))
                        .Add<T>(_length4)
                    );
            }
        }
    }
}