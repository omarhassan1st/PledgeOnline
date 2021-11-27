using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Web_FirstApplication.Const;
using Web_FirstApplication.Models.DbModel.Account;
using Web_FirstApplication.Models.DbModel.Shard;
using Web_FirstApplication.Models.DbModel.WebSite;
using Web_FirstApplication.Repository.Declaration.IAccount;
using Web_FirstApplication.Repository.Declaration.IShard;
using Web_FirstApplication.Repository.Declaration.IWebSite;

namespace Web_FirstApplication.Models.ViewModel
{
    public class LayoutViewModel
    {
        public int Capacity { get; set; }
        public string ServerTime { get; set; }
        public Activity Activites { get; set; }
        public string HotanFortress { get; set; }
        public LayoutViewModel(IAccountDbContext accountDbContext, IShardDbContext shardDbContext, IWebSiteDbContext webSiteDbContext)
        {
            ServerTime = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            Capacity = accountDbContext.ShardCurrentUsers.Find(c => c.nID == 1).nUserCount;
            Activites = webSiteDbContext.Activites.FirstOrDeafult();
            if (Activites.HonorUpdate.Contains(","))
            {
                string time = Activites.HonorUpdate.Split(',')
                    .Where(t => Convert.ToDateTime(t) > DateTime.Now).FirstOrDefault().ToString();
                Activites.HonorUpdate = DateTime.Now.Subtract(Convert.ToDateTime(time)).ToString()
                    .FixFormate();
            }
            else
            {
                Activites.HonorUpdate = DateTime.Now.Subtract(Convert.ToDateTime(Activites.HonorUpdate))
                    .ToString().FixFormate();
            }
            if (Activites.SelketNeith.Contains(","))
            {
                string time = Activites.SelketNeith.Split(',')
                    .Where(t => Convert.ToDateTime(t) > DateTime.Now).FirstOrDefault().ToString();
                Activites.SelketNeith = DateTime.Now.Subtract(Convert.ToDateTime(time)).ToString()
                    .FixFormate();
            }
            else
            {
                Activites.SelketNeith = DateTime.Now.Subtract(Convert.ToDateTime(Activites.SelketNeith))
                    .ToString().FixFormate();
            }
            if (Activites.AnbiusAsis.Contains(","))
            {
                string time = Activites.AnbiusAsis.Split(',')
                    .Where(t => Convert.ToDateTime(t) > DateTime.Now).FirstOrDefault().ToString();
                Activites.AnbiusAsis = DateTime.Now.Subtract(Convert.ToDateTime(time)).ToString()
                    .FixFormate();
            }
            else
            {
                Activites.AnbiusAsis = DateTime.Now.Subtract(Convert.ToDateTime(Activites.AnbiusAsis))
                    .ToString().FixFormate();
            }
            if (Activites.BattleArena.Contains(","))
            {
                string time = Activites.BattleArena.Split(',')
                    .Where(t => Convert.ToDateTime(t) > DateTime.Now).FirstOrDefault().ToString();
                Activites.BattleArena = DateTime.Now.Subtract(Convert.ToDateTime(time)).ToString()
                    .FixFormate();
            }
            else
            {
                Activites.BattleArena = DateTime.Now.Subtract(Convert.ToDateTime(Activites.BattleArena))
                    .ToString().FixFormate();
            }
            if (Activites.AlchemyEvent.Contains(","))
            {
                string time = Activites.AlchemyEvent.Split(',')
                    .Where(t => Convert.ToDateTime(t) > DateTime.Now).FirstOrDefault().ToString();
                Activites.AlchemyEvent = DateTime.Now.Subtract(Convert.ToDateTime(time)).ToString()
                    .FixFormate();
            }
            else
            {
                Activites.AlchemyEvent = DateTime.Now.Subtract(Convert.ToDateTime(Activites.AlchemyEvent))
                    .ToString().FixFormate();
            }
            if (Activites.LpnEvent.Contains(","))
            {
                string time = Activites.LpnEvent.Split(',')
                    .Where(t => Convert.ToDateTime(t) > DateTime.Now).FirstOrDefault().ToString();
                Activites.LpnEvent = DateTime.Now.Subtract(Convert.ToDateTime(time)).ToString()
                    .FixFormate();
            }
            else
            {
                Activites.LpnEvent = DateTime.Now.Subtract(Convert.ToDateTime(Activites.LpnEvent))
                    .ToString().FixFormate();
            }
            if (Activites.UniqueMaster.Contains(","))
            {
                string dayString = Activites.UniqueMaster.Split(',')[0];
                int dayInt = 0;
                switch (dayString.ToLower())
                {
                    case "saturday":
                        dayInt = 1;
                        break;
                    case "Sunday":
                        dayInt = 2;
                        break;
                    case "Monday":
                        dayInt = 3;
                        break;
                    case "Tuesday":
                        dayInt = 4;
                        break;
                    case "Wednesday":
                        dayInt = 5;
                        break;
                    case "Thursday":
                        dayInt = 6;
                        break;
                    case "Friday":
                        dayInt = 7;
                        break;
                    default:
                        dayInt = 0;
                        break;
                }

                Activites.UniqueMaster =
                    $"{(dayInt - (int)DateTime.Now.DayOfWeek).ToString().Replace("-",string.Empty)}d," +
                    $" {DateTime.Now.Subtract(Convert.ToDateTime(Activites.UniqueMaster.Split(',')[1])).ToString().FixFormate()}";
            }
            if (Activites.PvpMaster.Contains(","))
            {
                string dayString = Activites.PvpMaster.Split(',')[0];
                int dayInt = 0;
                switch (dayString.ToLower())
                {
                    case "saturday":
                        dayInt = 1;
                        break;
                    case "Sunday":
                        dayInt = 2;
                        break;
                    case "Monday":
                        dayInt = 3;
                        break;
                    case "Tuesday":
                        dayInt = 4;
                        break;
                    case "Wednesday":
                        dayInt = 5;
                        break;
                    case "Thursday":
                        dayInt = 6;
                        break;
                    case "Friday":
                        dayInt = 7;
                        break;
                    default:
                        dayInt = 0;
                        break;
                }

                Activites.PvpMaster =
                    $"{(dayInt - (int)DateTime.Now.DayOfWeek).ToString().Replace("-", string.Empty)}d," +
                    $" {DateTime.Now.Subtract(Convert.ToDateTime(Activites.PvpMaster.Split(',')[1])).ToString().FixFormate()}";
            }

            int GuildId = shardDbContext._SiegeFortress.Find(SF => SF.FortressID == 3).GuildID;
            HotanFortress = shardDbContext._Guilds.Find(Guild => Guild.ID == GuildId).Name;
        }
    }
}
