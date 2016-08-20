using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKGoldenAPI.Types
{
    public class Header
    {
        public string name { get; set; }
        public string value { get; set; }

        public Header() { }
        public Header(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
