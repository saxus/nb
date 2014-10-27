using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBouncer.Tools
{

    public static class StringEx
    {
        // Biztos, h van erre vmi a .NET FW-ben
        public static string Repeat(this string str, int count)
        {
            var sb = new StringBuilder(count * str.Length);

            for (int i = 0; i < count; i++)
            {
                sb.Append(str);
            }

            return sb.ToString();
        }


        private static UTF8Encoding s_utf8Encoding = new UTF8Encoding();


        public static byte[] ToByteArray(this string str)
        {
            if (str == null)
                return new byte[1] { 0 };

            var lg = s_utf8Encoding.GetByteCount(str);
            var b = new byte[lg + 1];

            b[0] = 1;
            s_utf8Encoding.GetBytes(str, 0, str.Length, b, 1);

            return b;
        }


        public static string RemoveWhitespaces(this string s)
        {
            var str = s
                .Replace('\u00a0', ' ')
                .Replace('\t', ' ')
                .Replace("\r", "")
                .Replace('\n', ' ').Trim();

            while (str.IndexOf("  ") != -1)
            {
                str = str.Replace("  ", " ");
            }

            return str;
        }


        public static int ContainsCount(this string s, char ch)
        {
            if (s.Length == 0)
                return 0;

            int count = 0;

            foreach (var c in s)
            {
                if (c == ch)
                    count++;
            }

            return count;
        }


        public static string[] Split(this string str, char c)
        {
            return str.Split(new char[] { c }, StringSplitOptions.None);
        }

        public static string[] Split(this string str, char c, int count)
        {
            return str.Split(new char[] { c }, count, StringSplitOptions.None);
        }


        public static string[] Split(this string str, string s)
        {
            return str.Split(new string[] { s }, StringSplitOptions.None);
        }


        public static string[] Split(this string str, string s, int count)
        {
            return str.Split(new string[] { s }, count, StringSplitOptions.None);
        }


        public static string Maximize(this string t, int maxlength)
        {
            if (t.Length > maxlength)
                return t.Substring(0, maxlength - 2) + "..";

            return t;
        }


        public static string F(this string t, object arg)
        {
            return string.Format(t, arg);
        }

        public static string F(this string t, object arg1, object arg2)
        {
            return string.Format(t, arg1, arg2);
        }

        public static string F(this string t, object arg1, object arg2, object arg3)
        {
            return string.Format(t, arg1, arg2, arg3);
        }



        public static string FirstLine(this string t)
        {
            if (t == null)
                return null;

            if (t.Contains("\n"))
            {
                return t.Substring(0, t.IndexOf("\n") - 1);
            }
            else
            {
                return t;
            }
        }
    }
}
