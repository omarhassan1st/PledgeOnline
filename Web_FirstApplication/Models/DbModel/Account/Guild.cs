using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.Account
{
    public class Guild
    {
        [Key]
        public int No { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
    }
}
