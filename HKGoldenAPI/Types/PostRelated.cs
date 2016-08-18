using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKGoldenAPI.Types
{
    public class Post
    {
        public string messageID { get; set; }
        public string postTitle { get; set; }
        public List<PostPage> pages { get; set; }
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
