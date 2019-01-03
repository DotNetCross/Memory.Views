using System;

namespace DotNetCross.Memory.Views
{
    public readonly struct View0D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;

        public View0D(T[] array, int index)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            //int arrayLength = array.Length;
            //if ((uint)index > (uint)arrayLength)
            //    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);

            //_pinnable = Unsafe.As<Pinnable<T>>(array);
            //_byteOffsetOrPointer = SpanHelpers.PerTypeValues<T>
            //    .ArrayAdjustment.Add<T>(index);

            _objectOrNull = array;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref array[index]);
        }

        internal View0D(object obj, ref T objectData)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);

            _objectOrNull = obj;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref objectData);
        }

        public View0D(IntPtr pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = pointer;
        }

        public unsafe View0D(void* pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
        }

        public static View0D<T> DangerousCreate(object obj, ref T objectData)
        {
            return new View0D<T>(obj, ref objectData);
        }

        public ref T Element => ref GetPinnableReference();

        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
}
