using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace DotNetCross.Memory.Views
{
    internal static partial class ViewHelper
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Multiply(this IntPtr start, int multiplier)
        {
            Debug.Assert(start.ToInt64() >= 0);
            Debug.Assert(multiplier >= 0);

            unsafe
            {
                if (sizeof(IntPtr) == sizeof(int))
                {
                    // 32-bit path.
                    return (IntPtr)((uint)start * (uint)multiplier);
                }
                else
                {
                    // 64-bit path.
                    return (IntPtr)((ulong)start * (ulong)multiplier);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ArrayIndexOfByteOffset<T>(IntPtr byteOffset)
        {
            unsafe
            {
                if (sizeof(IntPtr) == sizeof(int))
                {
                    // 32-bit path.
                    return ((int)byteOffset - (int)ViewHelper.PerTypeValues<T>.ArrayAdjustment1D) 
                        / Unsafe.SizeOf<T>();
                }
                else
                {
                    // 64-bit path.
                    return (int)(((long)byteOffset - (long)ViewHelper.PerTypeValues<T>.ArrayAdjustment1D)
                        / Unsafe.SizeOf<T>());
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

            public static readonly IntPtr ArrayAdjustment1D = MeasureArrayAdjustment1D();
            public static readonly IntPtr ArrayAdjustment2D = MeasureArrayAdjustment2D();
            public static readonly IntPtr ArrayAdjustment3D = MeasureArrayAdjustment3D();
            public static readonly IntPtr ArrayAdjustment4D = MeasureArrayAdjustment4D();
            public static readonly IntPtr ArrayAdjustment5D = MeasureArrayAdjustment5D();

            // Array header sizes are a runtime implementation detail and aren't the same across all runtimes. 
            // (The CLR made a tweak after 4.5, and Mono has an extra Bounds pointer.)
            private static IntPtr MeasureArrayAdjustment1D()
            {
                T[] sampleArray = new T[1];
                return Unsafe.ByteOffset(sampleArray, ref sampleArray[0]);
            }
            private static IntPtr MeasureArrayAdjustment2D()
            {
                T[,] sampleArray = new T[1,1];
                return Unsafe.ByteOffset(sampleArray, ref sampleArray[0,0]);
            }
            private static IntPtr MeasureArrayAdjustment3D()
            {
                T[,,] sampleArray = new T[1, 1, 1];
                return Unsafe.ByteOffset(sampleArray, ref sampleArray[0, 0, 0]);
            }
            private static IntPtr MeasureArrayAdjustment4D()
            {
                T[,,,] sampleArray = new T[1, 1, 1, 1];
                return Unsafe.ByteOffset(sampleArray, ref sampleArray[0, 0, 0, 0]);
            }
            private static IntPtr MeasureArrayAdjustment5D()
            {
                T[,,,,] sampleArray = new T[1, 1, 1, 1, 1];
                return Unsafe.ByteOffset(sampleArray, ref sampleArray[0, 0, 0, 0, 0]);
            }
        }
    }
}
