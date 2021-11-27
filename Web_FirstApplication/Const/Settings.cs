using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Conest
{
    public class Settings
    {
        public class Email
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string ID { get; set; }
            public string PW { get; set; }
        }
    }
}
