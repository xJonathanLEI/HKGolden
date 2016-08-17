using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HKGoldenAPI
{
    internal class NetworkHandler
    {

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
    }
}
