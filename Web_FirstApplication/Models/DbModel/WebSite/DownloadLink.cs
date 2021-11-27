using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.WebSite
{
    public class DownloadLink
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Size { get; set; }
        public string ImgPath { get; set; }
    }
}
