using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKGoldenAPI
{
    internal static class Utilities
    {
        public static string ReadUntil(ref string Str, string xStr)
        {
            if (!Str.Contains(xStr)) throw new Exception("xStr is not contained in Str.");
            int SP = Str.IndexOf(xStr);
            string RET = Str.Substring(0, SP);
            Str = Str.Remove(0, SP + xStr.Length);
            return RET;
        }

        public static string ClearHTML(string html)
        {
            return html.Replace("&nbsp;", "");
        }
    }
}
