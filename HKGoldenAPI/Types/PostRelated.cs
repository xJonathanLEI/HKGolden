using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HKGoldenAPI.Types
{
    public class Post
    {
        public string forumID { get; set; }
        public string messageID { get; set; }
        public string postTitle { get; set; }
        public List<PostPage> pages { get; set; }
        NetworkHandler netHandler;

        public Post()
        {
            netHandler = new NetworkHandler();
        }

        public async Task LoadPost()
        {
            await LoadPost(forumID, messageID);
        }

        public async Task LoadPost(string forumID, string messageID, int pageID = 1)
        {
            this.forumID = forumID; this.messageID = messageID;
            HtmlDocument postDocument = await netHandler.GETRequestAsDocumentAsync(Constants.URL_POST(forumID, messageID, pageID.ToString()));
            if (pages == null)
            {
                int numberOfPages = postDocument.DocumentNode.Descendants("select").Where(s => s.Attributes.Contains("name") && s.Attributes["name"].Value == "page").ElementAt(0).Descendants("option").Count();
                pages = new List<PostPage>();
                for (int i = 0; i < numberOfPages; i++)
                    pages.Add(new PostPage());
            }
            LoadPageFromDocument(postDocument, pageID - 1);
        }

        private void LoadPageFromDocument(HtmlDocument document, int pageIndex)
        {
            HtmlNode postDiv = document.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("style") && d.Attributes["style"].Value == "width: 945px").ElementAt(0);
            if (string.IsNullOrEmpty(postTitle))
                postTitle = postDiv.ChildNodes.Where(c => c.Name == "table").ElementAt(0).Descendants("td").Where(t => t.Attributes.Contains("valign")).ElementAt(0).Descendants("div").ElementAt(0).InnerText;
            pages[pageIndex].postEntries = new List<PostEntry>();
            foreach (HtmlNode tr in postDiv.Descendants("tr").Where(t => t.Attributes.Contains("username")))
            {
                pages[pageIndex].postEntries.Add(new PostEntry() { entryAuthor = new PostAuthor() { userID = tr.Attributes["userid"].Value, nickname = tr.Attributes["username"].Value } });
                HtmlNode contentGrid = tr.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "ContentGrid").ElementAt(0);
                pages[pageIndex].postEntries.Last().entryContent = Utilities.FixPostCharacters(Utilities.ClearPostContent(contentGrid.InnerHtml));
                string Time = tr.Descendants("span").Last().InnerText;
                Utilities.ReadUntil(ref Time, ">");
                int day = Convert.ToInt32(Utilities.ReadUntil(ref Time, "/"));
                int month = Convert.ToInt32(Utilities.ReadUntil(ref Time, "/"));
                int year = Convert.ToInt32(Utilities.ReadUntil(ref Time, " "));
                int hour = Convert.ToInt32(Utilities.ReadUntil(ref Time, ":"));
                int minute = Convert.ToInt32(Time);
                pages[pageIndex].postEntries.Last().postTime = new DateTime(year, month, day, hour, minute, 0);
            }
        }
    }

    public class PostPage
    {
        public bool pageLoaded { get; set; }
        public List<PostEntry> postEntries { get; set; }
    }

    public class PostEntry
    {
        public PostAuthor entryAuthor { get; set; }
        public string entryContent { get; set; }
        public DateTime postTime { get; set; }
    }

    public class PostAuthor
    {
        public string userID { get; set; }
        public string nickname { get; set; }
    }
}
