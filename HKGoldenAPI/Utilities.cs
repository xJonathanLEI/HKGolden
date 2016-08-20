using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace HKGoldenAPI
{
    public static class Utilities
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

        public static string ClearPostContent(string html)
        {
            List<string> stringsToClear = new List<string>() { "\r", "\n", "\t", " ", "<br>" };
            int clearIndex = startsWithAny(html, stringsToClear);
            while (clearIndex != -1)
            {
                html = html.Remove(0, stringsToClear[clearIndex].Length);
                clearIndex = startsWithAny(html, stringsToClear);
            }
            clearIndex = endsWithAny(html, stringsToClear);
            while (clearIndex != -1)
            {
                html = html.Remove(html.Length - stringsToClear[clearIndex].Length);
                clearIndex = endsWithAny(html, stringsToClear);
            }
            return html;
        }

        public static string FixPostCharacters(string html)
        {
            string ret = "";
            while (html.Contains("&#"))
            {
                ret += ReadUntil(ref html, "&#");
                if (html.IndexOf(";") != -1)
                    if (html.IndexOf ("&#") == -1 || html.IndexOf(";") < html.IndexOf("&#"))
                    {
                        string unicode = ReadUntil(ref html, ";");
                        unicode = System.Text.Encoding.Unicode.GetString(new byte[] { Convert.ToByte(Convert.ToInt32(unicode) - Math.Floor(Convert.ToInt32(unicode) / 256.0) * 256), Convert.ToByte(Math.Floor(Convert.ToInt32(unicode) / 256.0)) });
                        ret += unicode;
                        continue;
                    }
                //Fails to convert. Add back &#
                ret += "&#";
            }
            return ret + html;
        }

        private static int startsWithAny(string str, List<string> pres)
        {
            for (int i = 0; i < pres.Count; i++)
                if (str.StartsWith(pres[i])) return i;
            return -1;
        }

        private static int endsWithAny(string str, List<string> sufs)
        {
            for (int i = 0; i < sufs.Count; i++)
                if (str.EndsWith(sufs[i])) return i;
            return -1;
        }
        
        public static string GetMD5Hash(string source, bool lowerCase = true)
        {
            HashAlgorithmProvider pro = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(source, BinaryStringEncoding.Utf8);
            IBuffer buffHash = pro.HashData(buffUtf8Msg);
            byte[] data = buffHash.ToArray();
            if (buffHash.Length != pro.HashLength)
                throw new Exception("There was an error creating the hash");
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            if (!lowerCase) return sBuilder.ToString().ToUpper();
            return sBuilder.ToString();
        }
    }
}
