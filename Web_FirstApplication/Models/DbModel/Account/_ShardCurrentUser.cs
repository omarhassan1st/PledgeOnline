using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.Account
{
    [Table("_ShardCurrentUser")]
    public class _ShardCurrentUser
    {
        [Key]
        public int nID { get; set; }
        public int nShardID { get; set; }

        public int nUserCount { get; set; }

        public DateTime dLogDate { get; set; }

    }
}
