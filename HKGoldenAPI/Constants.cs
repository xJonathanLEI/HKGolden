using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKGoldenAPI
{
    static class Constants
    {
        public const string URL_HOMEPAGE = "http://www.hkgolden.com";
        public const string URL_TERMSOFSERVICE = "http://www.hkgolden.com/members/tos.aspx";
        public const string URL_REGISTER = "http://www.hkgolden.com/members/join2015.aspx";

        public static string URL_POST(string forumID, string messageID, string pageID = "1")
        {
            return "http://forum" + forumID + ".hkgolden.com/view.aspx?message=" + messageID + "?page=" + pageID;
        }
    }
}
