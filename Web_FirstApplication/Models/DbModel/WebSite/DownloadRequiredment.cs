using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.WebSite
{
    public class DownloadRequiredment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MINIMUM { get; set; }
        public string RECOMMENDED { get; set; }
    }
}
