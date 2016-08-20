using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HKGoldenAPI.Types;
using System.Collections;
using System.Reflection;

namespace HKGoldenAPI
{
    internal class NetworkHandler
    {
        public List<NameValuePair> Cookies { get; set; }

        public NetworkHandler()
        {
            Cookies = new List<NameValuePair>();
        }

        public async Task<string> GETRequestAsync(string URL, bool useCookieContainer = false, List<NameValuePair> headers = null, bool presentUserAgent = false)
        {
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            if (useCookieContainer)
            {
                HWR.Headers["Cookie"] = "";
                foreach (NameValuePair cookie in Cookies)
                    HWR.Headers["Cookie"] += cookie.name + "=" + cookie.value + ";";
            }
            HWR.Method = "GET";
            if (presentUserAgent) HWR.Headers["User-Agent"] = Constants.USER_AGENT;
            if (headers != null)
                foreach (NameValuePair header in headers)
                    HWR.Headers[header.name] = header.value;
            using (WebResponse res = await HWR.GetResponseAsync())
            {
                if (!string.IsNullOrEmpty(res.Headers["Set-Cookie"]))
                    AddSetCookieToCookies(res.Headers["Set-Cookie"]);
                using (Stream s = res.GetResponseStream())
                using (StreamReader SR = new StreamReader(s))
                    return await SR.ReadToEndAsync();
            }
        }

        public async Task<HtmlDocument> GETRequestAsDocumentAsync(string URL, bool useCookieContainer = false, List<NameValuePair> headers = null, bool presentUserAgent = false)
        {
            HtmlDocument doc = new HtmlDocument();
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            if (useCookieContainer)
            {
                HWR.Headers["Cookie"] = "";
                foreach (NameValuePair cookie in Cookies)
                    HWR.Headers["Cookie"] += cookie.name + "=" + cookie.value + ";";
            }
            HWR.Method = "GET";
            if (presentUserAgent) HWR.Headers["User-Agent"] = Constants.USER_AGENT;
            if (headers != null)
                foreach (NameValuePair header in headers)
                    HWR.Headers[header.name] = header.value;
            using (WebResponse res = await HWR.GetResponseAsync())
            {
                if (!string.IsNullOrEmpty(res.Headers["Set-Cookie"]))
                    AddSetCookieToCookies(res.Headers["Set-Cookie"]);
                using (Stream s = res.GetResponseStream())
                    doc.Load(s);
            }
            return doc;
        }

        public async Task<string> POSTRequestAsync(string URL, string param, bool useCookieContainer = false, List<NameValuePair> headers = null, bool presentUserAgent = false)
        {
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            if (useCookieContainer)
            {
                HWR.Headers["Cookie"] = "";
                foreach (NameValuePair cookie in Cookies)
                    HWR.Headers["Cookie"] += cookie.name + "=" + cookie.value + ";";
            }
            HWR.Method = "POST";
            if (presentUserAgent) HWR.Headers["User-Agent"] = Constants.USER_AGENT;
            if (headers != null)
                foreach (NameValuePair header in headers)
                    HWR.Headers[header.name] = header.value;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(param);
            HWR.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            await (await HWR.GetRequestStreamAsync()).WriteAsync(bytes, 0, bytes.Length);
            using (WebResponse res = await HWR.GetResponseAsync())
            {
                if (!string.IsNullOrEmpty(res.Headers["Set-Cookie"]))
                    AddSetCookieToCookies(res.Headers["Set-Cookie"]);
                using (Stream s = res.GetResponseStream())
                using (StreamReader SR = new StreamReader(s))
                    return await SR.ReadToEndAsync();
            }
        }

        public async Task<HtmlDocument> POSTRequestAsDocumentAsync(string URL, string param, bool useCookieContainer = false, List<NameValuePair> headers = null, bool presentUserAgent = false)
        {
            HtmlDocument doc = new HtmlDocument();
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            if (useCookieContainer)
            {
                HWR.Headers["Cookie"] = "";
                foreach (NameValuePair cookie in Cookies)
                    HWR.Headers["Cookie"] += cookie.name + "=" + cookie.value + ";";
            }
            HWR.Method = "POST";
            if (presentUserAgent) HWR.Headers["User-Agent"] = Constants.USER_AGENT;
            if (headers != null)
                foreach (NameValuePair header in headers)
                    HWR.Headers[header.name] = header.value;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(param);
            HWR.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            await (await HWR.GetRequestStreamAsync()).WriteAsync(bytes, 0, bytes.Length);
            using (WebResponse res = await HWR.GetResponseAsync())
            {
                if (!string.IsNullOrEmpty(res.Headers["Set-Cookie"]))
                    AddSetCookieToCookies(res.Headers["Set-Cookie"]);
                using (Stream s = res.GetResponseStream())
                    doc.Load(s);
            }
            return doc;
        }

        private void AddSetCookieToCookies(string setCookie)
        {
            List<string> namesToExclude = new List<string>() { "domain", "expires", "path" };
            while (setCookie != "")
            {
                while (setCookie.StartsWith(" ")) setCookie = setCookie.Remove(0, 1);
                string currentElement;
                if (setCookie.IndexOf(",") == -1 && setCookie.IndexOf(";") == -1)
                {
                    currentElement = setCookie;
                    setCookie = "";
                } else if (setCookie.IndexOf(",") == -1)
                    currentElement = Utilities.ReadUntil(ref setCookie, ";");
                else if (setCookie.IndexOf(";") == -1)
                    currentElement = Utilities.ReadUntil(ref setCookie, ",");
                else if (setCookie.IndexOf(",") < setCookie.IndexOf(";"))
                    currentElement = Utilities.ReadUntil(ref setCookie, ",");
                else
                    currentElement = Utilities.ReadUntil(ref setCookie, ";");
                if (currentElement.IndexOf("=") == -1) continue;
                string currentName = Utilities.ReadUntil(ref currentElement, "=");
                if (namesToExclude.Contains(currentName)) continue;
                Cookies.Add(new NameValuePair(currentName, currentElement));
            }
        }
    }
}
