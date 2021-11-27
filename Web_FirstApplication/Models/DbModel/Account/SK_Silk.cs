using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.Account
{
    [Table("SK_Silk")]
    public class SK_Silk
    {
        [Key]
        public int JID { get; set; }

        public int silk_own { get; set; }

        public int silk_gift { get; set; }

        public int silk_point { get; set; }
    }
}
