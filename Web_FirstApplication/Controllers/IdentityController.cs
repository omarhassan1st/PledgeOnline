using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Web_FirstApplication.Models.IdentityModel;
using Web_FirstApplication.Repository.Declaration.IAccount;
using Web_FirstApplication.Models.DbModel.Account;
using Web_FirstApplication.Conest;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web_FirstApplication.Const;

namespace Web_FirstApplication.Controllers
{
    [AllowAnonymous]
    public class IdentityController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAccountDbContext _DbContext;
        private readonly Services.Mailing _emailSender;
        public IdentityController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IAccountDbContext DbContext,
            Services.Mailing emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _DbContext = DbContext;
            _emailSender = emailSender;
        }

        [HttpGet]
        [ActionName("Login")]
        public async Task<IActionResult> OnGetLogin()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostLogin(LoginModel Input)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View();
        }

        [HttpGet]
        [ActionName("Register")]
        public IActionResult OnGetRegister()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostRegister(RegisterModel Input)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.UserName, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    string userIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    _DbContext.TB_Users.Add(new TB_User
                    {
                        StrUserID = Input.UserName,
                        password = Encryption.Sro.ComputeMD5Hash(Input.Password),
                        Email = Input.Email,
                        regtime = DateTime.Now,
                        reg_ip = userIp,
                        address = Services.Location.GetUserCountryByIp(userIp)
                    });
                    _DbContext.Complete();
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        [ActionName("Logout")]
        [HttpGet]
        public async Task<IActionResult> OnGetLogout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ActionName("ChangePassword")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OnGetChangePassword()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            bool hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        [ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostChangePassword(ChangePasswordModel.InputModel Input)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("Please Login First!");
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    if (!User.IsInRole("Admin"))
                    {
                        var tb_User = _DbContext.TB_Users.Find(u => u.StrUserID == user.UserName).password =
                             Encryption.Sro.ComputeMD5Hash(Input.NewPassword);
                        _DbContext.Complete();
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["StatusMessage"] = "Password Changed Succesfuly";
                }
            }
            return View();
        }

        [HttpGet]
        [ActionName("ForgotPassword")]
        public IActionResult OnGetForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostForgotPassword(ForgotPasswordModel.InputModel Input)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ResetPassword", "Identity",
                    new { Input.Email, code }, Request.Scheme);

                _emailSender.SendEmail(Input.Email, "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [ActionName("ResetPassword")]
        public IActionResult OnGetResetPassword(string code = null, string Email = null)
        {

            if (code == null || Email == null)
            {
                return BadRequest("A data must be supplied for password reset.");
            }
            else
            {
                var Model = new ResetPasswordModel
                {
                    Input = new ResetPasswordModel.InputModel
                    {
                        Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                        Email = Email
                    }
                };
                return View(Model);
            }
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostResetPassword(ResetPasswordModel.InputModel Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
