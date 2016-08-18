using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKGoldenAPI.Types;

namespace HKGoldenAPI
{
    public class Register
    {
        public string name { get; set; }
        public string nickname { get; set; }
        public Gender gender { get; set; }
        public EmailType emailType { get; set; }
        public string emailName { get; set; }
        public string emailDomain { get; set; }
        public string backupEmail { get; set; }


        public async Task TryRegister()
        {

        }
    }
}
