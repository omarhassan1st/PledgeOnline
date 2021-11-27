using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel;
using Web_FirstApplication.Models.DbModel.WebSite;
using Web_FirstApplication.Models.ViewModel;
using Web_FirstApplication.Repository.Declaration.IWebSite;

namespace Web_FirstApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IWebSiteDbContext _DbContext;

        public HomeController(IWebSiteDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] byte PageId = 0)
        {
            IEnumerable<HomeNew> homeNews = _DbContext.HomeNews
                .Skip((PageId == 0 ? 0 : PageId - 1) * 4).Take(4).ToList();
            TempData["Count"] =
                Convert.ToInt32(Math.Ceiling((double)_DbContext.HomeNews.Count() / 4));

            return View(homeNews);
        }

        [HttpGet]

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return base.View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
