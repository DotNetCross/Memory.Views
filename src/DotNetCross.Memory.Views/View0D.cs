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

            int arrayLength = array.Length;
            if ((uint)index > (uint)arrayLength)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);

            //_pinnable = Unsafe.As<Pinnable<T>>(array);
            //_byteOffsetOrPointer = SpanHelpers.PerTypeValues<T>
            //    .ArrayAdjustment.Add<T>(index);

            _objectOrNull = array;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref array[index]);
        }

        public View0D(object obj, ref T member)
        {
            _objectOrNull = obj;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref member);
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

        public ref T Element => ref GetPinnableReference();

        // For when from object member
        // public View(object obj, ref T memberRef)
        // https://stackoverflow.com/questions/1128315/find-size-of-object-instance-in-bytes-in-c-sharp
        // https://github.com/CyberSaving/MemoryUsage/blob/master/Main/Program.cs


        // https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7-3
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
}
