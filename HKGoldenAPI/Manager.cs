using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKGoldenAPI.Types;
using HKGoldenAPI.Exceptions;
using HtmlAgilityPack;

namespace HKGoldenAPI
{
    /// <summary>
    /// Provides access to the API
    /// </summary>
    public class Manager
    {
        public List<GalleryItem> homepageGallery;
        public List<Article> homepageArticles;

        NetworkHandler netHandler;

        public Manager()
        {
            netHandler = new NetworkHandler();
        }

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
            foreach (HtmlNode td in galleryTable.Descendants("td").Where(t=>t.Attributes.Contains("width") && (t.Attributes["width"].Value == "445px" || t.Attributes["width"].Value == "222px")))
                if (td.Descendants("table").Count() != 0)
                    foreach( HtmlNode subtd in td.Descendants("td").Where(t => t.Descendants("a").Count() != 0))
                        homepageGallery.Add(new GalleryItem(subtd));
                else
                    homepageGallery.Add(new GalleryItem(td));
            HtmlNode articleTable = homepage.DocumentNode.Descendants("table").Where(t => t.Attributes.Contains("width") && t.Attributes["width"].Value == "668px" && t.Attributes.Contains("border")).ElementAt(1);
            foreach (HtmlNode tr in articleTable.ChildNodes.Where(cc=>cc.Name == "tr"))
            {
                if (tr.Descendants("iframe").Count() != 0 || tr.Descendants("table").Count() == 0 || tr.Descendants("img").Where(i=>i.Attributes.Contains("height") && i.Attributes["height"].Value == "20px").Count() == 0) continue;
                homepageArticles.Add(new Article(tr.Descendants("table").ElementAt(0)));
            }
         }
    }
}
