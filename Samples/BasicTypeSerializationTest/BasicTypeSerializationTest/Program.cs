using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.Netduino;
using netduino.helpers.Helpers;
namespace BasicTypeSerializationTest {
    public class Program {
        public static void Main() {
            var serializer = new BasicTypeSerializer();
            serializer.Put((byte)byte.MaxValue);
            serializer.Put((UInt16)UInt16.MaxValue);
            serializer.Put((Int16)Int16.MinValue);
            serializer.Put((UInt32)UInt32.MaxValue);
            serializer.Put((Int32)Int32.MinValue);
            serializer.Put((UInt64)UInt64.MaxValue);
            serializer.Put((Int64)Int64.MinValue);
            serializer.Put("Unicode String");
            serializer.Put("ASCII String",true);
            serializer.Put(new byte[] { byte.MinValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, byte.MaxValue });
            serializer.Put(new ushort[] { ushort.MinValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, UInt16.MaxValue });
            serializer.Put(new UInt32[] { UInt32.MinValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, UInt32.MaxValue });
            serializer.Put(new UInt64[] { UInt64.MinValue, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, UInt64.MaxValue });

            byte byteValue = 0;
            UInt16 UInt16Value = 0;
            Int16 Int16Value = 0;
            UInt32 UInt32Value = 0;
            Int32 Int32Value = 0;
            UInt64 UInt64Value = 0;
            Int64 Int64Value = 0;
            string UnicodeString = null;
            string AsciiString = null;
            byte[] byteArray = null;
            ushort[] ushortArray = null;
            UInt32[] UInt32Array = null;
            UInt64[] UInt64Array = null;

            var deserializer = new BasicTypeDeSerializer(serializer.GetBuffer(), serializer.ContentSize);

            byteValue = deserializer.Get();
            if (byteValue != byte.MaxValue) throw new ApplicationException("byteValue");

            UInt16Value = deserializer.Get(UInt16Value);
            if (UInt16Value != UInt16.MaxValue) throw new ApplicationException("UInt16Value");

            Int16Value = deserializer.Get(Int16Value);
            if (Int16Value != Int16.MinValue) throw new ApplicationException("Int16Value");

            UInt32Value = deserializer.Get(UInt32Value);
            if (UInt32Value != UInt32.MaxValue) throw new ApplicationException("UInt32Value");

            Int32Value = deserializer.Get(Int32Value);
            if (Int32Value != Int32.MinValue) throw new ApplicationException("Int32Value");

            UInt64Value = deserializer.Get(UInt64Value);
            if (UInt64Value != UInt64.MaxValue) throw new ApplicationException("UInt64Value");

            Int64Value = deserializer.Get(Int64Value);
            if (Int64Value != Int64.MinValue) throw new ApplicationException("Int64Value");

            UnicodeString = deserializer.Get("");
            if (UnicodeString != "Unicode String") throw new ApplicationException("UnicodeString");

            AsciiString = deserializer.Get("");
            if (AsciiString != "ASCII String") throw new ApplicationException("AsciiString");

            byteArray = deserializer.Get(byteArray);
            if (byteArray[0] != byte.MinValue || byteArray[15] != byte.MaxValue) throw new ApplicationException("byteArray");

            ushortArray = deserializer.Get(ushortArray);
            if (ushortArray[0] != ushort.MinValue || ushortArray[15] != ushort.MaxValue) throw new ApplicationException("ushortArray");

            UInt32Array = deserializer.Get(UInt32Array);
            if (UInt32Array[0] != UInt32.MinValue || UInt32Array[15] != UInt32.MaxValue) throw new ApplicationException("UInt32Array");

            UInt64Array = deserializer.Get(UInt64Array);
            if (UInt64Array[0] != UInt64.MinValue || UInt64Array[15] != UInt64.MaxValue) throw new ApplicationException("UInt64Array");
        }
    }
}
