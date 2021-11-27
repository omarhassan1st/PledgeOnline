using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_FirstApplication.Repository.Declaration.IWebSite;

namespace Web_FirstApplication.Controllers
{
    [AllowAnonymous]
    public class DownloadController : Controller
    {
        private readonly IWebSiteDbContext _DbContext;
        public DownloadController(IWebSiteDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet]
        public IActionResult Links()
        {
            ViewData["DownloadLinks"] = _DbContext.DownloadLinks.ToList();
            ViewData["DownloadRequiredments"] = _DbContext.DownloadRequiredments.ToList();
            return View();
        }
    }
}
