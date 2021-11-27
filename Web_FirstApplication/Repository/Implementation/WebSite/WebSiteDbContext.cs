using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.WebSite;
using Web_FirstApplication.Repository.Declaration.IWebSite;
using Web_FirstApplication.Repository.Declaration.Shared;
using Web_FirstApplication.Repository.Implementation.Shared;

namespace Web_FirstApplication.Repository.Implementation.WebSite
{
    public class WebSiteDbContext : IWebSiteDbContext
    {
        private readonly WebSiteDbBase _DbContext;
        public IDbFunctions<DownloadLink> DownloadLinks { get; set; }
        public IDbFunctions<DownloadRequiredment> DownloadRequiredments { get; set; }
        public IDbFunctions<HomeNew> HomeNews { get; set; }
        public IDbFunctions<Activity> Activites { get; set; }
        public IDbFunctions<Donation> Donations { get; set; }
        public IDbFunctions<Donation.Coffe> Coffes { get; set; }
        public WebSiteDbContext(WebSiteDbBase DbContext)
        {
            _DbContext = DbContext;
            DownloadLinks = new DbFunctions<DownloadLink>(_DbContext);
            DownloadRequiredments = new DbFunctions<DownloadRequiredment>(_DbContext);
            HomeNews = new DbFunctions<HomeNew>(_DbContext);
            Activites = new DbFunctions<Activity>(_DbContext);
            Donations = new DbFunctions<Donation>(_DbContext);
            Coffes = new DbFunctions<Donation.Coffe>(_DbContext);
        }

        public int Complete()
        {
            return _DbContext.SaveChanges();
        }
    }
}
