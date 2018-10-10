using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetCross.Memory.Views.Tests
{
    [TestClass]
    public class ObjectMemorySizeTest
    {
        [TestMethod]
        public void ObjectMemorySizeTest_()
        {
            Assert.AreEqual(1, ObjectMemorySize<byte>(1));
            Assert.AreEqual(2, ObjectMemorySize<short>(2));
            Assert.AreEqual(4, ObjectMemorySize<int>(3));
            Assert.AreEqual(8, ObjectMemorySize<long>(4));
            Assert.AreEqual(10, ObjectMemorySize<string>("abcde"));
            Assert.AreEqual(7, ObjectMemorySize<byte[]>(new byte[] { 1,2,3,4,5,6,7 }));
        }

        // shallow, just the single object itself
        static int ObjectMemorySize<T>(in T obj)
        {
            if (typeof(T).IsValueType)
            {
                return Unsafe.SizeOf<T>();
            }
            else
            {
                //return ObjectSizeFromTypeHandleUnsafe(typeof(T));
                return TestSize<T>.SizeOf(obj);
            }
        }

        static int ObjectSizeFromTypeHandleMarshal(Type type)
        {
            return Marshal.ReadInt32(type.TypeHandle.Value, 4);
        }
        static unsafe int ObjectSizeFromTypeHandleUnsafe(Type type)
        {
            var typeHandle = type.TypeHandle;
            int size = *(*(int**)&typeHandle + 1);
            return size;
        }

        class TestSize<T>
        {

            static private int SizeOfObj(Type T, object thevalue)
            {
                var type = T;
                int returnval = 0;
                if (type.IsValueType)
                {
                    var nulltype = Nullable.GetUnderlyingType(type);
                    returnval = System.Runtime.InteropServices.Marshal.SizeOf(nulltype ?? type);
                }
                else if (thevalue == null)
                    return 0;
                else if (thevalue is string)
                    returnval = (thevalue as string).Length * sizeof(char); // What about length field
                else if (type.IsArray && type.GetElementType().IsValueType)
                {
                    // What about multidimensional?
                    returnval = ((Array)thevalue).GetLength(0) * Marshal.SizeOf(type.GetElementType());
                }
                //else if (thevalue is Stream)
                //{
                //    Stream thestram = thevalue as Stream;
                //    returnval = (int)thestram.Length;
                //}
                //else if (type.IsSerializable)
                //{
                //    try
                //    {
                //        using (Stream s = new MemoryStream())
                //        {
                //            BinaryFormatter formatter = new BinaryFormatter();
                //            formatter.Serialize(s, thevalue);
                //            returnval = (int)s.Length;
                //        }
                //    }
                //    catch { }
                //}
                else
                {
                    var fields = type.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        Type t = fields[i].FieldType;
                        Object v = fields[i].GetValue(thevalue);
                        returnval += 4 + SizeOfObj(t, v);
                    }
                }
                if (returnval == 0)
                    try
                    {
                        returnval = Marshal.SizeOf(thevalue);
                    }
                    catch { }
                return returnval;
            }
            static public int SizeOf(T value)
            {
                // This will box value type
                return SizeOfObj(typeof(T), value);
            }
        }

    }
}
