﻿using System;
using System.Runtime.CompilerServices;

namespace DotNetCross.Memory.Views
{
    public readonly struct ReadOnlyView0D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyView0D(IntPtr pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = pointer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlyView0D(void* pointer)
        {
            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyView0D(object obj, in T objectData)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);

            _objectOrNull = obj;
            _byteOffsetOrPointer = Unsafe.ByteOffset(_objectOrNull, ref Unsafe.AsRef<T>(objectData));
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
        public static ReadOnlyView0D<T> DangerousCreate(object obj, in T objectData)
        {
            return new ReadOnlyView0D<T>(obj, objectData);
        }

        public ref readonly T Element
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetPinnableReference();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
}
