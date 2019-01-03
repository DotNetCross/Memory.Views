using System;

namespace DotNetCross.Memory.Views
{
    public readonly struct ReadOnlyView0D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;

        // TODO: Replace with static methods
        public ReadOnlyView0D(T[] array, int index)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            int arrayLength = array.Length;
            if ((uint)index > (uint)arrayLength)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);

            _objectOrNull = array;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref array[index]);
        }

        // TODO: Make internal and create static method DangerousCreate()
        public ReadOnlyView0D(object obj, in T member)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);

            _objectOrNull = obj;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref Unsafe.AsRef<T>(member));
        }

        public ReadOnlyView0D(IntPtr pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = pointer;
        }

        public unsafe ReadOnlyView0D(void* pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
        }

        // TODO: Revise ctor and create `ReadOnlyView0D` static class instead

        public ref readonly T Element => ref GetPinnableReference();

        public ref readonly T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
}
