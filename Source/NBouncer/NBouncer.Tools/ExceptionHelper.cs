using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBouncer.Tools
{
    public static class ExceptionHelper
    {
        public static void WriteRecursive(Exception ex)
        {
            var exStr = CreateLogString(ex);

            Console.WriteLine(exStr);
        }

        public static string CreateLogString(Exception ex)
        {
            var e = ex;

            var sb = new StringBuilder();

            while (e != null)
            {
                sb.AppendLine("=".Repeat(79));
                sb.AppendLine("Exception: " + e.GetType().Name);
                sb.AppendLine("-".Repeat(79));
                sb.AppendLine(e.Message);
                sb.AppendLine("-".Repeat(79));
                sb.AppendLine(e.StackTrace);
                sb.AppendLine("-".Repeat(79));
                sb.AppendLine();

                e = e.InnerException;
            }

            return sb.ToString();
        }
    }
}
