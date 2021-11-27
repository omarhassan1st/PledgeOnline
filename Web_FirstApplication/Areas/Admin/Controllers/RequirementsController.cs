using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_FirstApplication.Conest;
using Web_FirstApplication.Models.DbModel.WebSite;
using Web_FirstApplication.Repository.Declaration.IWebSite;

namespace Web_FirstApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RequirementsController : Controller
    {
        private readonly IWebSiteDbContext _DbContext;
        public RequirementsController(IWebSiteDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public ActionResult Index()
        {
            var links = _DbContext.DownloadRequiredments.ToList();
            return View(links);
        }

        public ActionResult Details(int id)
        {
            var links = _DbContext.DownloadRequiredments.Find(L => L.Id == id);
            return View(links);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DownloadRequiredment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _DbContext.DownloadRequiredments.Add(model);
                    _DbContext.Complete();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var link = _DbContext.DownloadRequiredments.Find(L => L.Id == id);
            return View(link);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DownloadRequiredment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _DbContext.DownloadRequiredments.Update(model);
                    _DbContext.Complete();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult OnGetDelete(int id)
        {
            var link = _DbContext.DownloadRequiredments.Find(L => L.Id == id);
            return View(link);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult OnPostDelete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _DbContext.DownloadRequiredments.Remove(id);
                    _DbContext.Complete();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
