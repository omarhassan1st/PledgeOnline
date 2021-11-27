using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.WebSite;
using Web_FirstApplication.Repository.Declaration.Shared;

namespace Web_FirstApplication.Repository.Declaration.IWebSite
{
    public interface IWebSiteDbContext
    {
        IDbFunctions<DownloadLink> DownloadLinks { get; set; }
        IDbFunctions<DownloadRequiredment> DownloadRequiredments { get; set; }
        IDbFunctions<HomeNew> HomeNews { get; set; }
        IDbFunctions<Activity> Activites { get; set; }
        IDbFunctions<Donation> Donations { get; set; }
        IDbFunctions<Donation.Coffe> Coffes { get; set; }
        int Complete();
    }
}
