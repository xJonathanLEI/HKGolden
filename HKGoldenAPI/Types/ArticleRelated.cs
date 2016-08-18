using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HKGoldenAPI.Types
{
    public class GalleryItem
    {
        public string imageURL { get; set; }
        public string altText { get; set; }
        public string URL { get; set; }

        public GalleryItem() { }
        public GalleryItem(HtmlNode td)
        {
            HtmlNode a = td.Descendants("a").ElementAt(0);
            HtmlNode img = td.Descendants("img").ElementAt(0);
            URL = Constants.URL_HOMEPAGE + a.Attributes["href"].Value;
            altText = img.Attributes["alt"].Value;
            imageURL = img.Attributes["src"].Value;
            imageURL = Constants.URL_HOMEPAGE + (imageURL.StartsWith("..") ? imageURL.Remove(0, 2) : imageURL);
        }
    }

    public class Article
    {
        public string title { get; set; }
        public string author { get; set; }
        public string summary { get; set; }
        public string featurePhotoURL { get; set; }
        public string articleURL { get; set; }
        public string catId { get; set; }
        public string time { get; set; }

        public Article() { }
        public Article(HtmlNode table)
        {
            HtmlNode titleTr, authorTr, featureTr, summaryTr;
            titleTr = null; authorTr = null; featureTr = null; summaryTr = null;
            int trIndex = 0;
            foreach (HtmlNode tr in table.ChildNodes.Where(c => c.Name == "tr"))
            {
                switch (trIndex)
                {
                    case 0:
                        titleTr = tr;
                        break;
                    case 1:
                        authorTr = tr;
                        break;
                    case 2:
                        featureTr = tr;
                        break;
                    case 3:
                        summaryTr = tr;
                        break;
                }
                trIndex++;
            }
            string CatID = titleTr.Descendants("a").ElementAt(0).Attributes["href"].Value;
            Utilities.ReadUntil(ref CatID, "CatID=");
            catId = CatID;
            title = titleTr.Descendants("a").ElementAt(1).InnerText;
            articleURL = Constants.URL_HOMEPAGE + titleTr.Descendants("a").ElementAt(1).Attributes["href"].Value;
            author = Utilities.ClearHTML(authorTr.Descendants("font").ElementAt(0).InnerText);
            string Time = authorTr.Descendants("span").ElementAt(1).InnerText;
            Utilities.ReadUntil(ref Time, " ");
            time = Utilities.ReadUntil(ref Time, "發表");
            featurePhotoURL = Constants.URL_HOMEPAGE + featureTr.Descendants("img").ElementAt(0).Attributes["src"].Value;
            summary = summaryTr.Descendants("td").ElementAt(0).InnerText;
        }
    }
}
