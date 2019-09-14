using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetCross.Memory.Views
{
    // Slow Span source
    // https://github.com/nietras/corefx/blob/63f9e6d1c42d31d5ce6af978a29e518ee43dc811/src/System.Memory/src/System/OldView1D.cs
    // Fast Span
    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/Span.Fast.cs

    public readonly partial struct View1D<T>
    {
        public int Length => _length0;

        public unsafe Span<T> AsSpan()
        {
#if HASSPAN
            // Unfortunately only netcoreapp2.1 supports creating spans over any ref
            return MemoryMarshal.CreateSpan<T>(ref GetPinnableReference(), _length0);
#else
            if (_objectOrNull == null)
            {
                return new Span<T>(_byteOffsetOrPointer.ToPointer(), _length0);
            }
            else if (_objectOrNull is T[] array)
            {
                // Span ctors are sorely lacking for other than the default stuff
                // perhaps use reflection/IL emit or similar to construct directly
                var start = ViewHelper.ArrayIndexOfByteOffset<T>(_byteOffsetOrPointer);
                return new Span<T>(array, start, _length0);
            }
            else
            {
                // Span ctors are sorely lacking for other than the default stuff
                // perhaps use reflection/IL emit or similar to construct directly
                // We cannot create a span via normal ctors for e.g. 
                // multi -dimensional arrays.
                // TODO: For now throw exception
                return ThrowHelper.ThrowNotSupportedException_ViewNotSupportedBySpan<T>();
            }
#endif
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct OldView1D<T>
    {
        readonly object _objectOrNull;
        readonly IntPtr _byteOffsetOrPointer;
        // .NET has for good chosen int as size, would have preferred IntPtr e.g. nint
        // for length but this would give issues with interoperating with BCL
        readonly int _length;

        /// <summary>
        /// Creates a new view over the entirety of the target array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="array"/> is a null
        /// reference (Nothing in Visual Basic).</exception>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OldView1D(T[] array)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment1D;
            _length = array.Length;
        }

        /// <summary>
        /// Creates a new view over the portion of the target array beginning
        /// at 'start' index and covering the remainder of the array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the view.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="array"/> is a null
        /// reference (Nothing in Visual Basic).</exception>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> is not in the range (&lt;0 or &gt;=Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OldView1D(T[] array, int start)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));

            int arrayLength = array.Length;
            if ((uint)start > (uint)arrayLength)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment1D.Add<T>(start);
            _length = array.Length - start;
        }

        /// <summary>
        /// Creates a new view over the portion of the target array beginning
        /// at 'start' index and ending at 'end' index (exclusive).
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the view.</param>
        /// <param name="length">The number of items in the view.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="array"/> is a null
        /// reference (Nothing in Visual Basic).</exception>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in the range (&lt;0 or &gt;=Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OldView1D(T[] array, int start, int length)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            if (default(T) == null && array.GetType() != typeof(T[]))
                ThrowHelper.ThrowArrayTypeMismatchException_ArrayTypeMustBeExactMatch(typeof(T));
            if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _length = length;
            _objectOrNull = array;
            _byteOffsetOrPointer = ViewHelper.PerTypeValues<T>.ArrayAdjustment1D.Add<T>(start);
        }

        /// <summary>
        /// Creates a new view over the target unmanaged buffer.  Clearly this
        /// is quite dangerous, because we are creating arbitrarily typed T's
        /// out of a void*-typed block of memory.  And the length is not checked.
        /// But if this creation is correct, then all subsequent uses are correct.
        /// </summary>
        /// <param name="pointer">An unmanaged pointer to memory.</param>
        /// <param name="length">The number of <typeparamref name="T"/> elements the memory contains.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <typeparamref name="T"/> is reference type or contains pointers and hence cannot be stored in unmanaged memory.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="length"/> is negative.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe OldView1D(void* pointer, int length)
        {
            if (!ViewHelper.IsReferenceFree<T>())
                ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _length = length;
            _objectOrNull = null;
            _byteOffsetOrPointer = new IntPtr(pointer);
        }

        /// <summary>
        /// Create a new view over a portion of a regular managed object. This can be useful
        /// if part of a managed object represents a "fixed array." This is dangerous because
        /// "length" is not checked, nor is the fact that "rawPointer" actually lies within the object.
        /// </summary>
        /// <param name="obj">The managed object that contains the data to view over.</param>
        /// <param name="objectData">A reference to data within that object.</param>
        /// <param name="length">The number of <typeparamref name="T"/> elements the memory contains.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the specified object is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="length"/> is negative.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OldView1D<T> DangerousCreate(object obj, ref T objectData, int length)
        {
            if (obj == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);

            IntPtr byteOffset = Unsafe.ByteOffset(obj, ref objectData);
            return new OldView1D<T>(obj, byteOffset, length);
        }

        // Constructor for internal use only.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal OldView1D(object obj, IntPtr byteOffset, int length)
        {
            Debug.Assert(length >= 0);

            _objectOrNull = obj;
            _byteOffsetOrPointer = byteOffset;
            _length = length;
        }

        /// <summary>
        /// The number of items in the view.
        /// </summary>
        public int Length => _length;

        /// <summary>
        /// Returns true if Length is 0.
        /// </summary>
        public bool IsEmpty => _length == 0;

        /// <summary>
        /// Returns a reference to specified element of the OldView1D.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Thrown when index less than 0 or index greater than or equal to Length
        /// </exception>
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            //[NonVersionable]
            get
            {
                if ((uint)index >= ((uint)_length))
                    ThrowHelper.ThrowIndexOutOfRangeException();

                // TODO: Consider using nint via DotNetCross.NativeInts
                return ref Unsafe.RefAtByteOffset<T>(_objectOrNull,
                    _byteOffsetOrPointer.Add<T>(index));
            }
        }

        public unsafe Span<T> AsSpan()
        {
#if HASSPAN
            // Unfortunately only netcoreapp2.1 supports creating spans over any ref
            return MemoryMarshal.CreateSpan<T>(ref GetPinnableReference(), _length);
#else
            if (_objectOrNull == null)
            {
                return new Span<T>(_byteOffsetOrPointer.ToPointer(), _length);
            }
            else if (_objectOrNull is T[] array)
            {
                // Span ctors are sorely lacking for other than the default stuff
                // perhaps use reflection/IL emit or similar to construct directly
                var start = ViewHelper.ArrayIndexOfByteOffset<T>(_byteOffsetOrPointer);
                return new Span<T>(array, start, _length);
            }
            else
            {
                // Span ctors are sorely lacking for other than the default stuff
                // perhaps use reflection/IL emit or similar to construct directly
                // We cannot create a span via normal ctors for e.g. 
                // multi -dimensional arrays.
                // TODO: For now throw exception
                return ThrowHelper.ThrowNotSupportedException_ViewNotSupportedBySpan<T>();
            }
#endif
        }
    
        /// <summary>
        /// Clears the contents of this view.
        /// </summary>
        //public unsafe void Clear()
        //{
        //    int length = _length;

        //    if (length == 0)
        //        return;

        //    var byteLength = (UIntPtr)((uint)length * Unsafe.SizeOf<T>());

        //    if ((Unsafe.SizeOf<T>() & (sizeof(IntPtr) - 1)) != 0)
        //    {
        //        if (_objectOrNull == null)
        //        {
        //            var ptr = (byte*)_byteOffsetOrPointer.ToPointer();

        //            ViewHelper.ClearLessThanPointerSized(ptr, byteLength);
        //        }
        //        else
        //        {
        //            ref byte b = ref Unsafe.As<T, byte>(ref Unsafe.AddByteOffset<T>(ref _objectOrNull.Data, _byteOffsetOrPointer));

        //            ViewHelper.ClearLessThanPointerSized(ref b, byteLength);
        //        }
        //    }
        //    else
        //    {
        //        if (ViewHelper.IsReferenceFree<T>())
        //        {
        //            ref byte b = ref Unsafe.As<T, byte>(ref DangerousGetPinnableReference());

        //            ViewHelper.ClearPointerSizedWithoutReferences(ref b, byteLength);
        //        }
        //        else
        //        {
        //            UIntPtr pointerSizedLength = (UIntPtr)((length * Unsafe.SizeOf<T>()) / sizeof(IntPtr));

        //            ref IntPtr ip = ref Unsafe.As<T, IntPtr>(ref DangerousGetPinnableReference());

        //            ViewHelper.ClearPointerSizedWithReferences(ref ip, pointerSizedLength);
        //        }
        //    }
        //}

        /// <summary>
        /// Fills the contents of this view with the given value.
        /// </summary>
        //public unsafe void Fill(T value)
        //{
        //    int length = _length;

        //    if (length == 0)
        //        return;

        //    if (Unsafe.SizeOf<T>() == 1)
        //    {
        //        byte fill = Unsafe.As<T, byte>(ref value);
        //        if (_objectOrNull == null)
        //        {
        //            Unsafe.InitBlockUnaligned(_byteOffsetOrPointer.ToPointer(), fill, (uint)length);
        //        }
        //        else
        //        {
        //            ref byte r = ref Unsafe.As<T, byte>(ref Unsafe.AddByteOffset<T>(ref _objectOrNull.Data, _byteOffsetOrPointer));
        //            Unsafe.InitBlockUnaligned(ref r, fill, (uint)length);
        //        }
        //    }
        //    else
        //    {
        //        ref T r = ref DangerousGetPinnableReference();

        //        // TODO: Create block fill for value types of power of two sizes e.g. 2,4,8,16

        //        // Simple loop unrolling
        //        int i = 0;
        //        for (; i < (length & ~7); i += 8)
        //        {
        //            Unsafe.Add<T>(ref r, i + 0) = value;
        //            Unsafe.Add<T>(ref r, i + 1) = value;
        //            Unsafe.Add<T>(ref r, i + 2) = value;
        //            Unsafe.Add<T>(ref r, i + 3) = value;
        //            Unsafe.Add<T>(ref r, i + 4) = value;
        //            Unsafe.Add<T>(ref r, i + 5) = value;
        //            Unsafe.Add<T>(ref r, i + 6) = value;
        //            Unsafe.Add<T>(ref r, i + 7) = value;
        //        }
        //        if (i < (length & ~3))
        //        {
        //            Unsafe.Add<T>(ref r, i + 0) = value;
        //            Unsafe.Add<T>(ref r, i + 1) = value;
        //            Unsafe.Add<T>(ref r, i + 2) = value;
        //            Unsafe.Add<T>(ref r, i + 3) = value;
        //            i += 4;
        //        }
        //        for (; i < length; i++)
        //        {
        //            Unsafe.Add<T>(ref r, i) = value;
        //        }
        //    }
        //}

        /// <summary>
        /// Copies the contents of this view into destination view. If the source
        /// and destinations overlap, this method behaves as if the original values in
        /// a temporary location before the destination is overwritten.
        ///
        /// <param name="destination">The view to copy items into.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the destination OldView1D is shorter than the source OldView1D.
        /// </exception>
        /// </summary>
        // TODO: Move to extension method instead
        public void CopyTo(OldView1D<T> destination)
        {
            if (!TryCopyTo(destination))
                ThrowHelper.ThrowArgumentException_DestinationTooShort();
        }


        /// <summary>
        /// Copies the contents of this view into destination view. If the source
        /// and destinations overlap, this method behaves as if the original values in
        /// a temporary location before the destination is overwritten.
        ///
        /// <returns>If the destination view is shorter than the source view, this method
        /// return false and no data is written to the destination.</returns>
        /// </summary>
        /// <param name="destination">The view to copy items into.</param>
        public bool TryCopyTo(OldView1D<T> destination)
        {
            if ((uint)_length > (uint)destination._length)
                return false;

            // TODO: This is a tide-over implementation as we plan to add a overlap-safe cpblk-based api to Unsafe. (https://github.com/dotnet/corefx/issues/13427)
            unsafe
            {
                ref T src = ref GetPinnableReference();
                ref T dst = ref destination.GetPinnableReference();
                IntPtr srcMinusDst = Unsafe.ByteOffset<T>(ref dst, ref src);
                int length = _length;

                bool srcGreaterThanDst = (sizeof(IntPtr) == sizeof(int)) ? srcMinusDst.ToInt32() >= 0 : srcMinusDst.ToInt64() >= 0;
                if (srcGreaterThanDst)
                {
                    // Source address greater than or equal to destination address. Can do normal copy.
                    for (int i = 0; i < length; i++)
                    {
                        Unsafe.Add<T>(ref dst, i) = Unsafe.Add<T>(ref src, i);
                    }
                }
                else
                {
                    // Source address less than destination address. Must do backward copy.
                    int i = length;
                    while (i-- != 0)
                    {
                        Unsafe.Add<T>(ref dst, i) = Unsafe.Add<T>(ref src, i);
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Returns true if left and right point at the same memory and have the same length.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        public static bool operator ==(OldView1D<T> left, OldView1D<T> right)
        {
            return left._length == right._length && Unsafe.AreSame<T>(ref left.GetPinnableReference(), ref right.GetPinnableReference());
        }

        /// <summary>
        /// Returns false if left and right point at the same memory and have the same length.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        public static bool operator !=(OldView1D<T> left, OldView1D<T> right) => !(left == right);

        // TODO: We need to re-enable this scenario since we can use
        //       this in boxed scenarios
#pragma warning disable 0809  //warning CS0809: Obsolete member 'Span<T>.Equals(object)' overrides non-obsolete member 'object.Equals(object)'
        /// <summary>
        /// This method is not supported as spans cannot be boxed. To compare two spans, use operator==.
        /// <exception cref="System.NotSupportedException">
        /// Always thrown by this method.
        /// </exception>
        /// </summary>
        [Obsolete("Equals() on OldView1D will always throw an exception. Use == instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            throw new NotSupportedException("TODO!");//SR.CannotCallEqualsOnView1D);
        }

        /// <summary>
        /// This method is not supported as spans cannot be boxed.
        /// <exception cref="System.NotSupportedException">
        /// Always thrown by this method.
        /// </exception>
        /// </summary>
        [Obsolete("GetHashCode() on OldView1D will always throw an exception.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            throw new NotSupportedException("TODO!");//SR.CannotCallGetHashCodeOnView1D);
        }
#pragma warning restore 0809

        /// <summary>
        /// Defines an implicit conversion of an array to a <see cref="OldView1D{T}"/>
        /// </summary>
        public static implicit operator OldView1D<T>(T[] array) => new OldView1D<T>(array);

        /// <summary>
        /// Defines an implicit conversion of a <see cref="ArraySegment{T}"/> to a <see cref="OldView1D{T}"/>
        /// </summary>
        public static implicit operator OldView1D<T>(ArraySegment<T> arraySegment) => new OldView1D<T>(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

        /// <summary>
        /// Defines an implicit conversion of a <see cref="OldView1D{T}"/> to a <see cref="ReadOnlyView1D{T}"/>
        /// </summary>
        //public static implicit operator ReadOnlyView1D<T>(OldView1D<T> view) => new ReadOnlyView1D<T>(view._objectOrNull, view._byteOffsetOrPointer, view._length);

        /// <summary>
        /// Forms a slice out of the given view, beginning at 'start'.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> index is not in range (&lt;0 or &gt;=Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OldView1D<T> Slice(int start)
        {
            if ((uint)start > (uint)_length)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            IntPtr newOffset = _byteOffsetOrPointer.Add<T>(start);
            int length = _length - start;
            return new OldView1D<T>(_objectOrNull, newOffset, length);
        }

        /// <summary>
        /// Forms a slice out of the given view, beginning at 'start', of given length
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice (exclusive).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in range (&lt;0 or &gt;=Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OldView1D<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            IntPtr newOffset = _byteOffsetOrPointer.Add<T>(start);
            return new OldView1D<T>(_objectOrNull, newOffset, length);
        }

        /// <summary>
        /// Copies the contents of this view into a new array.  This heap
        /// allocates, so should generally be avoided, however it is sometimes
        /// necessary to bridge the gap with APIs written in terms of arrays.
        /// </summary>
        // TODO: Move to extension method instead
        public T[] ToArray()
        {
            if (_length == 0)
                return ViewHelper.PerTypeValues<T>.EmptyArray;

            T[] result = new T[_length];
            CopyTo(result);
            return result;
        }

        /// <summary>
        /// Returns a 0-length view whose base is the null pointer.
        /// </summary>
        public static OldView1D<T> Empty => default;

        /// <summary>
        /// Returns a reference to the 0th element of the OldView1D. If the OldView1D is empty, returns a reference to the location where the 0th element
        /// would have been stored. Such a reference can be used for pinning but must never be dereferenced.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
        {
            return ref Unsafe.RefAtByteOffset<T>(_objectOrNull, _byteOffsetOrPointer);
        }
    }
}
