using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKGoldenAPI.Types;
using HKGoldenAPI.Exceptions;
using HtmlAgilityPack;
using System.Net;

namespace HKGoldenAPI
{
    /// <summary>
    /// Provides access to the API
    /// </summary>
    public class Manager
    {
        public List<GalleryItem> homepageGallery { get; set; }
        public List<Article> homepageArticles { get; set; }
        public bool LoggedIn { get; set; }
        string loginSessionId { get; set; }
        public enum APIMode
        {
            Official = 0,
            HTMLParsing = 1
        }

        NetworkHandler netHandler;

        public Manager()
        {
            netHandler = new NetworkHandler();
        }

        /// <summary>
        /// Load the terms of service of the HKGolden
        /// </summary>
        /// <returns>TermsOfService object containing the website TOS</returns>
        public async Task<TermsOfService> GetTermsOfServiceAsync()
        {
            try
            {
                HtmlDocument document = await netHandler.GETRequestAsDocumentAsync(Constants.URL_TERMSOFSERVICE);
                TermsOfService tos = new TermsOfService(document);
                return tos;
            }
            catch
            {
                throw new NetworkError();
            }
        }

        /// <summary>
        /// Load the homepageGallery and homepageArticles
        /// </summary>
        /// <returns></returns>
        public async Task LoadHomePage()
        {
            homepageGallery = new List<GalleryItem>();
            homepageArticles = new List<Article>();
            HtmlDocument homepage = await netHandler.GETRequestAsDocumentAsync(Constants.URL_HOMEPAGE);
            HtmlNode galleryTable = homepage.DocumentNode.Descendants("table").Where(t => t.Attributes.Contains("width") && t.Attributes["width"].Value == "668px").ElementAt(0);
            foreach (HtmlNode td in galleryTable.Descendants("td").Where(t => t.Attributes.Contains("width") && (t.Attributes["width"].Value == "445px" || t.Attributes["width"].Value == "222px")))
                if (td.Descendants("table").Count() != 0)
                    foreach (HtmlNode subtd in td.Descendants("td").Where(t => t.Descendants("a").Count() != 0))
                        homepageGallery.Add(new GalleryItem(subtd));
                else
                    homepageGallery.Add(new GalleryItem(td));
            HtmlNode articleTable = homepage.DocumentNode.Descendants("table").Where(t => t.Attributes.Contains("width") && t.Attributes["width"].Value == "668px" && t.Attributes.Contains("border")).ElementAt(1);
            foreach (HtmlNode tr in articleTable.ChildNodes.Where(cc => cc.Name == "tr"))
            {
                if (tr.Descendants("iframe").Count() != 0 || tr.Descendants("table").Count() == 0 || tr.Descendants("img").Where(i => i.Attributes.Contains("height") && i.Attributes["height"].Value == "20px").Count() == 0) continue;
                homepageArticles.Add(new Article(tr.Descendants("table").ElementAt(0)));
            }
        }

        /// <summary>
        /// Load the meta info and the first page of a post.
        /// </summary>
        /// <param name="forumID">Forum ID</param>
        /// <param name="messageID">Message ID of the post</param>
        /// <returns>The Post object with meta and the first page loaded</returns>
        public async Task<Post> LoadPost(string forumID, string messageID)
        {
            Post post = new Post();
            await post.LoadPost(forumID, messageID);
            return post;
        }

        /// <summary>
        /// Login with username and password only.
        /// </summary>
        /// <param name="username">Username/email</param>
        /// <param name="password">Password in plain text</param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password, APIMode apiMode = APIMode.Official)
        {
            if (apiMode == APIMode.Official)
            {

                string param = "username=" + WebUtility.UrlEncode(username) + "&returntype=json&pass=" + Utilities.GetMD5Hash(password);
                string postResult = await netHandler.POSTRequestAsync(Constants.OFFICIAL_LOGIN, param);

                return false;
            }else
            {
                string param = "ctl00%24ContentPlaceHolder1%24ScriptManager1=ctl00%24ContentPlaceHolder1%24ScriptManager1%7Cctl00%24ContentPlaceHolder1%24linkb_login&__VIEWSTATE=%2FwEPDwUJMTc5Njc0NzU1D2QWAmYPZBYCZg9kFgJmDxYCHgRUZXh0BbsFPGxpbmsgcmVsPSJjYW5vbmljYWwiIGhyZWY9Imh0dHA6Ly9tMS5oa2dvbGRlbi5jb20vL2xvZ2luLmFzcHgiIC8%2BDQo8bWV0YSBwcm9wZXJ0eT0ib2c6aW1hZ2UiIGNvbnRlbnQ9Imh0dHA6Ly9tMS5oa2dvbGRlbi5jb20vaW1hZ2VzL2luZGV4X2ltYWdlcy9mYl9sb2dvLmpwZyIgLz4NCjxtZXRhIHByb3BlcnR5PSJvZzpsb2NhbGUiIGNvbnRlbnQ9InpoX0hLIiAvPg0KPG1ldGEgcHJvcGVydHk9Im9nOnR5cGUiIGNvbnRlbnQ9IndlYnNpdGUiIC8%2BDQo8bWV0YSBwcm9wZXJ0eT0ib2c6dGl0bGUiIGNvbnRlbnQ9Iummmea4r%2BmrmOeZuyAtIOWFqOa4r%2BacgOWPl%2Batoei%2Fjm%2Fml6Lpm7vohabos4foqIrntrLnq5kiLz4NCjxtZXRhIHByb3BlcnR5PSJvZzpkZXNjcmlwdGlvbiIgY29udGVudD0i6aaZ5riv6auY55m7IC0g5YWo5riv5pyA5Y%2BX5q2h6L%2BOb%2BaXoumbu%2BiFpuizh%2Bioiue2suermSIgLz4NCjxtZXRhIHByb3BlcnR5PSJvZzp1cmwiIGNvbnRlbnQ9Imh0dHA6Ly9tMS5oa2dvbGRlbi5jb20vL2xvZ2luLmFzcHgiIC8%2BDQo8bWV0YSBwcm9wZXJ0eT0ib2c6c2l0ZV9uYW1lIiBjb250ZW50PSJoa2dvbGRlbi5jb20iIC8%2BDQo8bWV0YSBwcm9wZXJ0eT0iZmI6YWRtaW5zIiBjb250ZW50PSIxMDAwMDExNjAwNTQzODYiIC8%2BDQo8bWV0YSBwcm9wZXJ0eT0iZmI6YXBwX2lkIiBjb250ZW50PSIxNTEyNTc0ODcyMzE2ODc1IiAvPg0KZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAQUrY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRjYl9yZW1lbWJlcl9sb2dpbrpkjBL1imHGCSpG91a5gj2QJCvq&__VIEWSTATEGENERATOR=C2EE9ABB&__EVENTVALIDATION=%2FwEWBQKnsOmMCwLIgaDXBQK%2F7a%2F7AgKtjfOnCgL6vZ%2FmCGLMHLW121jNYn3bTEMkE9o3wlGr&ctl00%24ContentPlaceHolder1%24txt_email=" + WebUtility.UrlEncode(username) + "&ctl00%24ContentPlaceHolder1%24txt_pass=" + WebUtility.UrlEncode(password) + "&ctl00%24ContentPlaceHolder1%24cb_remember_login=on&__ASYNCPOST=true&ctl00%24ContentPlaceHolder1%24linkb_login=%E7%99%BB%E5%85%A5";
                string postResult = await netHandler.POSTRequestAsync(Constants.URL_LOGIN, param, true, null, true);
                if (postResult.Contains("BW"))
                {
                    loginSessionId = netHandler.Cookies.Where(c => c.name == "ASP.NET_SessionId").ElementAt(0).value;
                    LoggedIn = true;
                    return true;
                }
                return false;
            }
        }
    }
}
