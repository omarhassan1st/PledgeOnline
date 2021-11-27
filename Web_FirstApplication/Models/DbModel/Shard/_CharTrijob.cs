using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.Shard
{
    [Table("_CharTrijob")]
    public class _CharTrijob
    {
        [Key]
        public int CharID { get; set; }

        public byte JobType { get; set; }

        public byte Level { get; set; }

        public int Exp { get; set; }

        public int Contribution { get; set; }

        public int Reward { get; set; }

        [NotMapped]
        public string CharName { get; set; }


    }
}
