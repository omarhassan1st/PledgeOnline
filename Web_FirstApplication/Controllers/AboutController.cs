using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Controllers
{
    [AllowAnonymous]
    public class AboutController : Controller
    {
        [HttpGet]
        public IActionResult TermsOfService()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RefundPolicy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ServerInformation()
        {
            return View();
        }
    }
}
