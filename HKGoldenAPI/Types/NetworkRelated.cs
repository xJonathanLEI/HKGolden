using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKGoldenAPI.Types
{
    public class NameValuePair
    {
        public string name { get; set; }
        public string value { get; set; }

        public NameValuePair() { }
        public NameValuePair(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }


}
