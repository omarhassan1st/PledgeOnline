using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.DbModel.WebSite
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string FortressWar { get; set; }
        public string Register { get; set; }
        public string BattleArena { get; set; }
        public string AlchemyEvent { get; set; }
        public string LpnEvent { get; set; }
        public string UniqueMaster { get; set; }
        public string PvpMaster { get; set; }
        public string AnbiusAsis { get; set; }
        public string SelketNeith { get; set; }
        public string HonorUpdate { get; set; }
    }
}
