 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace DotNetCross.Memory.Views
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct View1D<T>
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
        public unsafe View1D(void* pointer, int length0)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
        }

        // Constructor for internal use only.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal View1D(object obj, IntPtr byteOffset, 
            int length0)
        {
            //Debug.Assert(length >= 0);

            _objectOrNull = obj;
            _byteOffsetOrPointer = byteOffset;
            _length0 = length0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static View1D<T> DangerousCreate(object obj, ref T objectData, 
            int length0)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            if (length0 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length0);

            IntPtr byteOffset = Unsafe.ByteOffset(obj, ref objectData);
            return new View1D<T>(obj, byteOffset, 
                length0);
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
                        .Add<T>(index0)
                    );
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T UnsafeAt(int index0)
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                _byteOffsetOrPointer
                    .Add<T>(index0)
                    );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct View2D<T>
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
        public unsafe View2D(void* pointer, int length0, int length1)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _byteStride0 = new IntPtr(_length1).Multiply(Unsafe.SizeOf<T>());
        }
        public unsafe View2D(void* pointer, int length0, int length1,
            IntPtr byteStride0)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _byteStride0 = byteStride0;
        }

        // Constructor for internal use only.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal View2D(object obj, IntPtr byteOffset, 
            int length0, int length1, IntPtr byteStride0)
        {
            //Debug.Assert(length >= 0);

            _objectOrNull = obj;
            _byteOffsetOrPointer = byteOffset;
            _length0 = length0;
            _length1 = length1;
            _byteStride0 = byteStride0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static View2D<T> DangerousCreate(object obj, ref T objectData, 
            int length0, int length1, IntPtr byteStride0)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            if (length0 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length0);
            if (length1 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length1);

            IntPtr byteOffset = Unsafe.ByteOffset(obj, ref objectData);
            return new View2D<T>(obj, byteOffset, 
                length0, length1, byteStride0);
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
                        .Add(_byteStride0.Multiply(index0))
                        .Add<T>(index1)
                    );
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T UnsafeAt(int index0, int index1)
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                _byteOffsetOrPointer
                    .Add(_byteStride0.Multiply(index0))
                    .Add<T>(index1)
                    );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct View3D<T>
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
        public unsafe View3D(void* pointer, int length0, int length1, int length2)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _byteStride1 = new IntPtr(_length2).Multiply(Unsafe.SizeOf<T>());
            _byteStride0 = _byteStride1.Multiply(_length1);
        }
        public unsafe View3D(void* pointer, int length0, int length1, int length2,
            IntPtr byteStride0, IntPtr byteStride1)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _byteStride0 = byteStride0;
            _byteStride1 = byteStride1;
        }

        // Constructor for internal use only.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal View3D(object obj, IntPtr byteOffset, 
            int length0, int length1, int length2, IntPtr byteStride0, IntPtr byteStride1)
        {
            //Debug.Assert(length >= 0);

            _objectOrNull = obj;
            _byteOffsetOrPointer = byteOffset;
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _byteStride0 = byteStride0;
            _byteStride1 = byteStride1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static View3D<T> DangerousCreate(object obj, ref T objectData, 
            int length0, int length1, int length2, IntPtr byteStride0, IntPtr byteStride1)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            if (length0 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length0);
            if (length1 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length1);
            if (length2 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length2);

            IntPtr byteOffset = Unsafe.ByteOffset(obj, ref objectData);
            return new View3D<T>(obj, byteOffset, 
                length0, length1, length2, byteStride0, byteStride1);
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
                        .Add(_byteStride0.Multiply(index0))
                        .Add(_byteStride1.Multiply(index1))
                        .Add<T>(index2)
                    );
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T UnsafeAt(int index0, int index1, int index2)
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                _byteOffsetOrPointer
                    .Add(_byteStride0.Multiply(index0))
                    .Add(_byteStride1.Multiply(index1))
                    .Add<T>(index2)
                    );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct View4D<T>
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
        public unsafe View4D(void* pointer, int length0, int length1, int length2, int length3)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
            _byteStride2 = new IntPtr(_length3).Multiply(Unsafe.SizeOf<T>());
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
        }
        public unsafe View4D(void* pointer, int length0, int length1, int length2, int length3,
            IntPtr byteStride0, IntPtr byteStride1, IntPtr byteStride2)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
            _byteStride0 = byteStride0;
            _byteStride1 = byteStride1;
            _byteStride2 = byteStride2;
        }

        // Constructor for internal use only.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal View4D(object obj, IntPtr byteOffset, 
            int length0, int length1, int length2, int length3, IntPtr byteStride0, IntPtr byteStride1, IntPtr byteStride2)
        {
            //Debug.Assert(length >= 0);

            _objectOrNull = obj;
            _byteOffsetOrPointer = byteOffset;
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
            _byteStride0 = byteStride0;
            _byteStride1 = byteStride1;
            _byteStride2 = byteStride2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static View4D<T> DangerousCreate(object obj, ref T objectData, 
            int length0, int length1, int length2, int length3, IntPtr byteStride0, IntPtr byteStride1, IntPtr byteStride2)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            if (length0 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length0);
            if (length1 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length1);
            if (length2 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length2);
            if (length3 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length3);

            IntPtr byteOffset = Unsafe.ByteOffset(obj, ref objectData);
            return new View4D<T>(obj, byteOffset, 
                length0, length1, length2, length3, byteStride0, byteStride1, byteStride2);
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
                        .Add(_byteStride0.Multiply(index0))
                        .Add(_byteStride1.Multiply(index1))
                        .Add(_byteStride2.Multiply(index2))
                        .Add<T>(index3)
                    );
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T UnsafeAt(int index0, int index1, int index2, int index3)
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                _byteOffsetOrPointer
                    .Add(_byteStride0.Multiply(index0))
                    .Add(_byteStride1.Multiply(index1))
                    .Add(_byteStride2.Multiply(index2))
                    .Add<T>(index3)
                    );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct View5D<T>
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
        public unsafe View5D(void* pointer, int length0, int length1, int length2, int length3, int length4)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
            _length4 = length4;
            _byteStride3 = new IntPtr(_length4).Multiply(Unsafe.SizeOf<T>());
            _byteStride2 = _byteStride3.Multiply(_length3);
            _byteStride1 = _byteStride2.Multiply(_length2);
            _byteStride0 = _byteStride1.Multiply(_length1);
        }
        public unsafe View5D(void* pointer, int length0, int length1, int length2, int length3, int length4,
            IntPtr byteStride0, IntPtr byteStride1, IntPtr byteStride2, IntPtr byteStride3)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));

            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
            _length4 = length4;
            _byteStride0 = byteStride0;
            _byteStride1 = byteStride1;
            _byteStride2 = byteStride2;
            _byteStride3 = byteStride3;
        }

        // Constructor for internal use only.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal View5D(object obj, IntPtr byteOffset, 
            int length0, int length1, int length2, int length3, int length4, IntPtr byteStride0, IntPtr byteStride1, IntPtr byteStride2, IntPtr byteStride3)
        {
            //Debug.Assert(length >= 0);

            _objectOrNull = obj;
            _byteOffsetOrPointer = byteOffset;
            _length0 = length0;
            _length1 = length1;
            _length2 = length2;
            _length3 = length3;
            _length4 = length4;
            _byteStride0 = byteStride0;
            _byteStride1 = byteStride1;
            _byteStride2 = byteStride2;
            _byteStride3 = byteStride3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static View5D<T> DangerousCreate(object obj, ref T objectData, 
            int length0, int length1, int length2, int length3, int length4, IntPtr byteStride0, IntPtr byteStride1, IntPtr byteStride2, IntPtr byteStride3)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            if (length0 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length0);
            if (length1 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length1);
            if (length2 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length2);
            if (length3 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length3);
            if (length4 < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length4);

            IntPtr byteOffset = Unsafe.ByteOffset(obj, ref objectData);
            return new View5D<T>(obj, byteOffset, 
                length0, length1, length2, length3, length4, byteStride0, byteStride1, byteStride2, byteStride3);
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
                        .Add(_byteStride0.Multiply(index0))
                        .Add(_byteStride1.Multiply(index1))
                        .Add(_byteStride2.Multiply(index2))
                        .Add(_byteStride3.Multiply(index3))
                        .Add<T>(index4)
                    );
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T UnsafeAt(int index0, int index1, int index2, int index3, int index4)
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                _byteOffsetOrPointer
                    .Add(_byteStride0.Multiply(index0))
                    .Add(_byteStride1.Multiply(index1))
                    .Add(_byteStride2.Multiply(index2))
                    .Add(_byteStride3.Multiply(index3))
                    .Add<T>(index4)
                    );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
}