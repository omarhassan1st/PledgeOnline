using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Web_FirstApplication.Models.DbModel.Account;
using Web_FirstApplication.Models.DbModel.WebSite;
using Web_FirstApplication.Models.Donation;
using Web_FirstApplication.Repository.Declaration.IAccount;
using Web_FirstApplication.Repository.Declaration.IWebSite;

namespace Web_FirstApplication.Controllers
{
    public class DonationController : Controller
    {
        private readonly IWebSiteDbContext _webSiteDbContext;
        private readonly IAccountDbContext _accountDbContext;
        public DonationController(IWebSiteDbContext webDbContext, IAccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
            _webSiteDbContext = webDbContext;
        }
        public IActionResult Methods(Donation donation)
        {
            donation.coffe = _webSiteDbContext.Coffes.Find(C => C.Id == 2);
            return View(donation);
        }

        [HttpGet]
        [ActionName("PaymentWall")]
        public IActionResult OnGetPaymentWall(NameValueCollection parameters)
        {
            return RedirectToAction(nameof(OnPostPaymentWall));
        }

        [HttpPost]
        [ActionName("PaymentWall")]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostPaymentWall(NameValueCollection parameters)
        {
            #region ( Keys )
            PaymentWall.Paymentwall_Base.setApiType(PaymentWall.Paymentwall_Base.API_GOODS);
            PaymentWall.Paymentwall_Base.setAppKey("13cf3b6e4f2a2784bebd7e94f2241d1d");
            PaymentWall.Paymentwall_Base.setSecretKey("10356e134564c53a8db8692f5f332730");
            #endregion

            PaymentWall.Paymentwall_Pingback pingback =
                new(parameters, HttpContext.Request.Query["REMOTE_ADDR"]);

            if (pingback.validate())
            {
                float productAmount = pingback.getProduct().getAmount();
                string pingUserId = pingback.getUserId();

                int tableUserId = _accountDbContext.TB_Users.Find(u => u.StrUserID == pingUserId).JID;

                if (tableUserId != 0)
                {
                    SK_Silk userSilk = _accountDbContext.SK_Silks.Find(u => u.JID == tableUserId);
                    if (pingback.isDeliverable())
                    {
                        if (userSilk == null)
                        {
                            _accountDbContext.SK_Silks.Add(new SK_Silk
                            {
                                JID = tableUserId,
                                silk_own = Convert.ToInt32(productAmount),
                                silk_point = 1
                            });
                        }
                        else
                        {
                            userSilk.silk_own += Convert.ToInt32(productAmount);
                            userSilk.silk_point = 1;
                            _accountDbContext.SK_Silks.Update(userSilk);
                        }
                    }
                    else if (pingback.isCancelable())
                    {
                        if (userSilk != null)
                        {
                            userSilk.silk_own -= Convert.ToInt32(productAmount);
                            userSilk.silk_point = 1;
                            _accountDbContext.SK_Silks.Update(userSilk);
                        }
                    }
                }
                return Ok();
            }
            else
            {
                ModelState.AddModelError(string.Empty, pingback.getErrorSummary());
                return View();
            }
        }
    }
}
