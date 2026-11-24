using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Models;
using TARge25_Shop.Models.Account;

namespace TARge25_Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailServices;

        public AccountController
            (
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IEmailServices emailServices
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    City = vm.City,
                };

                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
                    newsignup.Subject = "SHOP Confirm your email";
                    newsignup.To = user.Email;

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administrations");
                    }

                    _emailServices.SendEmailToken(newsignup, token);
                    List<string> errordatas =
                        [
                        "Area", "Account",
                        "Issue", "Success",
                        "StatusMessage", "Registration Sucesss",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.City}\n[password hidden]\n[password hidden]"
                        ];
                    ViewBag.ErrorDatas = errordatas;
                    ViewBag.ErrorTitle = "Successfully registered";
                    ViewBag.ErrorMessage = "Please check your email to confirm your account.";

                    return View("ConfirmationEmailMessage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // otsib ülesse kasutajaid läbi Id
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id of {userId} is not valid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            List<string> errordatas =
                [
                "Area", "Account",
                "Issue", "Success",
                "StatusMessage", "Registration Success",
                "ActedOn", $"{user.Email}",
                "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]"
                ];

            if (result.Succeeded)
            {
                errordatas =
                [
                "Area", "Account",
                "Issue", "Success",
                "StatusMessage", "Registration Success",
                "ActedOn", $"{user.Email}",
                "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]"
                ];
                ViewBag.ErrorMessage = errordatas;
                return View();
            }

            ViewBag.ErrorDatas = errordatas;
            ViewBag.ErrorTitles = "Email cannot be confirmed";
            ViewBag.ErrorMessage = $"The user email, with userid of {userId}, cannot be confirmed";
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string ? returnUrl = null)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            ///////
            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ///////

            if (ModelState.IsValid)
            {
                // Otsib emaili DB'st üles
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return View("Login", loginViewModel);
            }

            // Proovi sisse logida, kui kasutajal on juba väline login seotud
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // Kui kontot veel pole, otsi või loo see Google'ist saadud e-posti alusel
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);

            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true // Vajalik, kuna RequireConfirmedAccount = true
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        foreach (var error in createResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View("Login", loginViewModel);
                    }
                }

                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Email claim not received from Google.");
            return View("Login", loginViewModel);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    //Muutuja token tuleb uus token genereerida
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Muutuja password reset link viib resetpassword vaatele
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                    var emailDto = new EmailDto
                    {
                        // Email sisu
                        To = model.Email,
                        Subject = "SHOP Password reset request",
                        Body = $"Reset your account password by clicking this link: <a href='{passwordResetLink}'>Reset Password</a>"
                    };
                    _emailServices.SendEmail(emailDto);

                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ResetPassword(string email, string token)
        {
            // kui token või email on null, siis tuleb sõnum Invalid password reset token
            if (email == null || token == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // Tulemuses kasutame ResetPasswordAsync ja see annab kaasa user, tokeni ja salasõna
                    var result = await _userManager.ResetPasswordAsync(
                        user,
                        model.Token,
                        model.Password
                        );

                    // Kui õnnestub, siis returnib vaate resetPasswordConfirmation
                    if (result.Succeeded)
                    {
                        // Kontrollib kas reset link on aegunud?
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                // siia võib panna eraldi returin, kui kasutaja nimi on vale
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                // Kui user ib bykk, siis suuna login vaatesse
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // var result muutuja peab kasutama meetotit muudaPassword
                var result = await _userManager.ChangePasswordAsync(
                    user,
                    model.Password,
                    model.NewPassword
                    );

                if (result.Succeeded)
                {
                    // tuleb kasutada värskenda sisse kirjutamist meetotit
                    await _signInManager.RefreshSignInAsync(user);
                    //Suunab vaatele muutsid salasõna edukalt
                    return View("PasswordChangeConfirmation");
                }
                // Kui ei ole edukas, siis näita veateade
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
