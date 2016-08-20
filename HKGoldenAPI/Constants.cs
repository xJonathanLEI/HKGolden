using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKGoldenAPI
{
    public static class Constants
    {
        public const string URL_HOMEPAGE = "http://www.hkgolden.com";
        public const string URL_TERMSOFSERVICE = "http://www.hkgolden.com/members/tos.aspx";
        public const string URL_REGISTER = "http://www.hkgolden.com/members/join2015.aspx";
        public const string URL_LOGIN = "http://m2.hkgolden.com/login.aspx";
        public const string OFFICIAL_LOGIN = "http://android-1-2.hkgolden.com/login.aspx";
        public const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393";

        public static string URL_POST(string forumID, string messageID, string pageID = "1")
        {
            return "http://forum" + forumID + ".hkgolden.com/view.aspx?message=" + messageID + "&page=" + pageID;
        }
    }
}
