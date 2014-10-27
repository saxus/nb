using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBouncer.Tools
{
    public static class StringHelper
    {
        private static UTF8Encoding s_utf8Encoding = new UTF8Encoding();

        public static byte[] StringToByteArray(string str)
        {
            if (str == null)
                return new byte[1] { 0 };

            var lg = s_utf8Encoding.GetByteCount(str);
            var b = new byte[lg + 1];

            b[0] = 1;
            s_utf8Encoding.GetBytes(str, 0, str.Length, b, 1);

            return b;
        }


        public static string ByteArrayToString(byte[] array)
        {
            return ByteArrayToString(array, 0);
        }


        public static string ByteArrayToString(byte[] array, int offset)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Length - offset < 1)
                throw new Exception("Array length < 1");

            if (array[offset] == 0)
                return null;

            return s_utf8Encoding.GetString(array, offset + 1, array.Length - 1 - offset);
        }

    }
}
