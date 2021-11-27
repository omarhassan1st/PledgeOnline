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
    public class HomeNewsController : Controller
    {
        private readonly IWebSiteDbContext _DbContext;
        public HomeNewsController(IWebSiteDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public ActionResult Index()
        {
            var links = _DbContext.HomeNews.ToList();
            return View(links);
        }

        public ActionResult Details(int id)
        {
            var links = _DbContext.HomeNews.Find(L => L.Id == id);
            return View(links);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HomeNew model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Date = DateTime.Now;
                    _DbContext.HomeNews.Add(model);
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
            var link = _DbContext.HomeNews.Find(L => L.Id == id);
            return View(link);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HomeNew model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _DbContext.HomeNews.Update(model);
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
            var link = _DbContext.HomeNews.Find(L => L.Id == id);
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
                    _DbContext.HomeNews.Remove(id);
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
