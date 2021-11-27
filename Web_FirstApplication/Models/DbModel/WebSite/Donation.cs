using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.WebSite
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        public Coffe coffe { get; set; }
        public class Coffe
        {
            [Key]
            public int Id { get; set; }
            public string Path { get; set; }
        }

    }
}
