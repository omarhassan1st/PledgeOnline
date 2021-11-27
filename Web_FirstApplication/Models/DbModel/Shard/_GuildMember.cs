using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.Shard
{
    [Table("_GuildMember")]
    public class _GuildMember
    {
        [Key]
        public int GuildID { get; set; }

        public int CharID { get; set; }

        public string CharName { get; set; }

        public byte MemberClass { get; set; }

        public byte CharLevel { get; set; }

        public int GP_Donation { get; set; }

        public DateTime JoinDate { get; set; }

        public int? Permission { get; set; }

        public int? Contribution { get; set; }

        public int? GuildWarKill { get; set; }

        public int? GuildWarKilled { get; set; }

        public string Nickname { get; set; }

        public int? RefObjID { get; set; }

        public byte? SiegeAuthority { get; set; }

    }

}
