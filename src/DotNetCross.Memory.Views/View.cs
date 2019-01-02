using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetCross.Memory.Views
{
    // inspiration
    // Fast/Portable Span Factoring PR
    // https://github.com/dotnet/corefx/pull/26667
    //
    // This class exists solely so that arbitrary objects can be Unsafe-casted to it to get a ref to the start of the user data.
    //
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class Pinnable<T>
    {
        public T Data;
    }

    // Span API
    // https://github.com/dotnet/corefx/blob/7a8b3b9e837aefcf60ad52588e773870325fcf45/src/System.Runtime/ref/System.Runtime.cs#L2221


    public readonly struct View1D<T>
    {
        readonly Pinnable<T> _pinnable;
        readonly IntPtr _byteOffsetOrPointer;
        readonly IntPtr _length;
    }


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

    internal static partial class SpanHelpers
    {
        /// <summary>
        /// Computes "start + index * sizeof(T)", using the unsigned IntPtr-sized multiplication for 32 and 64 bits.
        ///
        /// Assumptions:
        ///     Start and index are non-negative, and already pre-validated to be within the valid range of their containing Span.
        ///
        ///     If the byte length (Span.Length * sizeof(T)) does an unsigned overflow (i.e. the buffer wraps or is too big to fit within the address space),
        ///     the behavior is undefined.
        ///
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Add<T>(this IntPtr start, int index)
        {
            Debug.Assert(start.ToInt64() >= 0);
            Debug.Assert(index >= 0);

            unsafe
            {
                if (sizeof(IntPtr) == sizeof(int))
                {
                    // 32-bit path.
                    uint byteLength = (uint)index * (uint)Unsafe.SizeOf<T>();
                    return (IntPtr)(((byte*)start) + byteLength);
                }
                else
                {
                    // 64-bit path.
                    ulong byteLength = (ulong)index * (ulong)Unsafe.SizeOf<T>();
                    return (IntPtr)(((byte*)start) + byteLength);
                }
            }
        }

        /// <summary>
        /// Determine if a type is eligible for storage in unmanaged memory. TODO: To be replaced by a ContainsReference() api.
        /// </summary>
        public static bool IsReferenceFree<T>() => PerTypeValues<T>.IsReferenceFree;

        private static bool IsReferenceFreeCore<T>()
        {
            // Under the JIT, these become constant-folded.
            if (typeof(T) == typeof(byte))
                return true;
            if (typeof(T) == typeof(sbyte))
                return true;
            if (typeof(T) == typeof(bool))
                return true;
            if (typeof(T) == typeof(char))
                return true;
            if (typeof(T) == typeof(short))
                return true;
            if (typeof(T) == typeof(ushort))
                return true;
            if (typeof(T) == typeof(int))
                return true;
            if (typeof(T) == typeof(uint))
                return true;
            if (typeof(T) == typeof(long))
                return true;
            if (typeof(T) == typeof(ulong))
                return true;
            if (typeof(T) == typeof(IntPtr))
                return true;
            if (typeof(T) == typeof(UIntPtr))
                return true;

            return IsReferenceFreeCoreSlow(typeof(T));
        }

        private static bool IsReferenceFreeCoreSlow(Type type)
        {
            if (type.GetTypeInfo().IsPrimitive) // This is hopefully the common case. All types that return true for this are value types w/out embedded references.
                return true;

            if (!type.GetTypeInfo().IsValueType)
                return false;

            // If type is a Nullable<> of something, unwrap it first.
            Type underlyingNullable = Nullable.GetUnderlyingType(type);
            if (underlyingNullable != null)
                type = underlyingNullable;

            if (type.GetTypeInfo().IsEnum)
                return true;

            foreach (FieldInfo field in type.GetTypeInfo().DeclaredFields)
            {
                if (field.IsStatic)
                    continue;
                if (!IsReferenceFreeCoreSlow(field.FieldType))
                    return false;
            }
            return true;
        }

        // https://github.com/nietras/corefx/blob/63f9e6d1c42d31d5ce6af978a29e518ee43dc811/src/System.Memory/src/System/SpanHelpers.cs
        public static class PerTypeValues<T>
        {
            //
            // Latch to ensure that excruciatingly expensive validation check for constructing a Span around a raw pointer is done
            // only once per type (unless of course, the validation fails.)
            //
            // false == not yet computed or found to be not reference free.
            // true == confirmed reference free
            //
            public static readonly bool IsReferenceFree = IsReferenceFreeCore<T>();

            public static readonly T[] EmptyArray = new T[0];

            public static readonly IntPtr ArrayAdjustment = MeasureArrayAdjustment();

            // Array header sizes are a runtime implementation detail and aren't the same across all runtimes. (The CLR made a tweak after 4.5, and Mono has an extra Bounds pointer.)
            private static IntPtr MeasureArrayAdjustment()
            {
                T[] sampleArray = new T[1];
                return Unsafe.ByteOffset<T>(ref Unsafe.As<Pinnable<T>>(sampleArray).Data, ref sampleArray[0]);
            }
        }
    }
}
