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
        public CookieContainer cookieContainer;

        public NetworkHandler()
        {
            cookieContainer = new CookieContainer();
        }

        public async Task<string> GETRequestAsync(string URL)
        {
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            HWR.Method = "GET";
            using (Stream s = (await HWR.GetResponseAsync()).GetResponseStream())
            using (StreamReader SR = new StreamReader(s))
                return await SR.ReadToEndAsync();
        }

        public async Task<HtmlDocument> GETRequestAsDocumentAsync(string URL)
        {
            HtmlDocument doc = new HtmlDocument();
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            HWR.Method = "GET";
            using (Stream s = (await HWR.GetResponseAsync()).GetResponseStream())
                doc.Load(s);
            return doc;
        }

        public async Task<string> POSTRequestAsync(string URL, string param, bool useCookieContainer = false, List<Header> headers = null, bool presentUserAgent = false)
        {
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            if (useCookieContainer)
                HWR.CookieContainer = cookieContainer;
            HWR.Method = "POST";
            if (presentUserAgent) HWR.Headers["User-Agent"] = Constants.USER_AGENT;
            if (headers != null)
                foreach (Header header in headers)
                    HWR.Headers[header.name] = header.value;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(param);
            HWR.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            await (await HWR.GetRequestStreamAsync()).WriteAsync(bytes, 0, bytes.Length);
            using (Stream s = (await HWR.GetResponseAsync()).GetResponseStream())
            using (StreamReader SR = new StreamReader(s))
                return await SR.ReadToEndAsync();
        }

        public CookieCollection GetAllCookies()
        {
            return null;
        }

        public async Task<HtmlDocument> POSTRequestAsDocumentAsync(string URL, string param, bool useCookieContainer = false, List<Header> headers = null, bool presentUserAgent = false)
        {
            HtmlDocument doc = new HtmlDocument();
            HttpWebRequest HWR = (HttpWebRequest)HttpWebRequest.Create(URL);
            if (useCookieContainer)
                HWR.CookieContainer = cookieContainer;
            HWR.Method = "POST";
            if (presentUserAgent) HWR.Headers["User-Agent"] = Constants.USER_AGENT;
            if (headers != null)
                foreach (Header header in headers)
                    HWR.Headers[header.name] = header.value;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(param);
            HWR.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            await (await HWR.GetRequestStreamAsync()).WriteAsync(bytes, 0, bytes.Length);
            using (Stream s = (await HWR.GetResponseAsync()).GetResponseStream())
                doc.Load(s);
            return doc;
        }
    }
}
