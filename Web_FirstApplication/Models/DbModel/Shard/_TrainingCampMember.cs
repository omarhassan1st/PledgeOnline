using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.Shard
{
    [Table("_TrainingCampMember")]
    public class _TrainingCampMember
    {
        [Key]
        public int CampID { get; set; }

        public int CharID { get; set; }

        public int RefObjID { get; set; }

        public string CharName { get; set; }

        public DateTime JoinDate { get; set; }

        public byte MemberClass { get; set; }

        public byte CharJoinedLevel { get; set; }

        public byte CharCurLevel { get; set; }

        public byte CharMaxLevel { get; set; }

        public int HonorPoint { get; set; }
        [NotMapped]
        public string GuildName { get; set; }

    }
}
