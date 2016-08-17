using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HKGoldenAPI.Types
{
    /// <summary>
    /// The structure for the terms of serivce
    /// </summary>
    public class TermsOfService
    {
        public string title { get; set; }
        public List<TermsOfServiceSection> sections { get; set; }

        public TermsOfService() { }
        public TermsOfService(HtmlDocument document)
        {
            title = ClearText(document.DocumentNode.Descendants("h2").ElementAt(0).Descendants("font").ElementAt(0).InnerText);
            HtmlNode table = document.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("style") && d.Attributes["style"].Value == "margin-left:30px; margin-right:30px;").ElementAt(0).Descendants("table").ElementAt(0);
            bool addingTitle = true;
            sections = new List<TermsOfServiceSection>();
            foreach (HtmlNode tr in table.Descendants("tr").Where(t => t.Descendants("font").Count() != 0))
            {
                if (addingTitle)
                    sections.Add(new TermsOfServiceSection() { sectionTitle = ClearText(tr.Descendants("font").ElementAt(0).InnerText), sectionContent = "" });
                else
                    foreach (HtmlNode font in tr.Descendants("font"))
                        sections.Last().sectionContent += ClearText(font.InnerText);
                addingTitle = !addingTitle;
            }
        }

        private string ClearText(string text)
        {
            return text.Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }
    }

    public class TermsOfServiceSection
    {
        public string sectionTitle { get; set; }
        public string sectionContent { get; set; }
    }
}
