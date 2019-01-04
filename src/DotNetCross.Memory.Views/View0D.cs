using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetCross.Memory.Views
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct View0D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public View0D(IntPtr pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = pointer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe View0D(void* pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal View0D(object obj, ref T objectData)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);

            _objectOrNull = obj;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref objectData);
        }

        /// <summary>
        /// Create a new read-only view over a portion of a regular managed object.
        /// This is dangerous because it is not checked if <paramref name="objectData"/>
        /// actually lies within the object <paramref name="obj"/>.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the specified object is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static View0D<T> DangerousCreate(object obj, ref T objectData)
        {
            return new View0D<T>(obj, ref objectData);
        }

        public ref T Element
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetPinnableReference();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }

    // https://github.com/nietras/corefx/blob/63f9e6d1c42d31d5ce6af978a29e518ee43dc811/src/System.Memory/src/System/Span.cs

}
