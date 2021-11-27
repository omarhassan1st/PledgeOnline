using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.WebSite
{
    public class HomeNew
    {
        public int Id { get; set; }
        [Required]
        public string Msg { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Paragraph { get; set; }
    }
}
