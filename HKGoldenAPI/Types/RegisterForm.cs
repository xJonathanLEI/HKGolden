using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKGoldenAPI.Types
{
    public enum Gender
    {
        Female = 0,
        Male = 1
    }

    public enum EmailType
    {
        ISP = 1,
        Telephone_Company = 2,
        College = 3,
        Middle_School =4
    }

    public class Interests
    {
        public bool electronic { get; set; }
        public bool phone { get; set; }
        public bool sport { get; set; }
        public bool computer { get; set; }
        public bool investment { get; set; }
        public bool movie { get; set; }
        public bool videoGame { get; set; }
        public bool travelling { get; set; }
        public bool photography { get; set; }
        public bool mp3 { get; set; }
        public bool car { get; set; }
        public bool other { get; set; }
        public bool beauty { get; set; }
    }
}
