using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.Shard
{
    public class _SiegeFortress
    {
        [Key]
        public int FortressID { get; set; }

        public int GuildID { get; set; }

        public short TaxRatio { get; set; }

        public long Tax { get; set; }

        public byte NPCHired { get; set; }

        public int TempGuildID { get; set; }

        public string Introduction { get; set; }

        public DateTime? CreatedDungeonTime { get; set; }

        public byte? CreatedDungeonCount { get; set; }

        public byte IntroductionModificationPermission { get; set; }
    }
}
