using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shop.Models;
using Shop.Models.AccountViewModels;
using Shop.Models.Domain;
using MimeKit;
using System.IO;
using System.Net.Mail;
using Shop.Models.Domain.Enum;
using Shop.Models.Domain.Interface;
using Shop.Services;

namespace Shop.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISellerRepository _sellerRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            ISellerRepository sellerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _sellerRepository = sellerRepository;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (returnUrl != null && returnUrl.StartsWith("/admin/", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Người dùng đăng nhập.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản người dùng đã bị khóa.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Địa chỉ e-mail hoặc mật khẩu của bạn không chính xác. Vui lòng thử lại");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Không thể xác thực hai yếu tố.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("Người dùng có ID {UserId} đã đăng nhập bằng 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("Người dùng có tài khoản ID {UserId} bị khóa.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Mã xác thực không hợp lệ được nhập cho người dùng  {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Mã xác thực không hợp lệ.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Không thể xác thực hai yếu tố.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("Người dùng  {UserId} đã đăng nhập bằng mã khôi phục.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("Người dùng có tài khoản {UserId} bị khóa.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Mã khôi phục không hợp lệ được nhập cho người dùng  {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Mã khôi phục không hợp lệ.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Sex"] = Geslachten();
            return View();
        }

        private SelectList Geslachten()
        {
            var geslachten = new List<Sex>();
            foreach (Sex geslacht in Enum.GetValues(typeof(Sex)))
            {
                geslachten.Add(geslacht);
            }
            return new SelectList(geslachten);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var gebruiker = new User
                    {
                        FirstName = model.FirstName,
                        FamilyName = model.FamilyName,
                        EmailAddress = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        Sex = model.Sex ?? Sex.Different
                    };
                    _userRepository.Add(gebruiker);
                    _userRepository.SaveChanges();
                    _logger.LogInformation("Người dùng đã tạo một tài khoản mới bằng mật khẩu.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl, model.FirstName);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("Người dùng đã tạo một tài khoản mới bằng mật khẩu.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            ViewData["Sex"] = Geslachten();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterSeller(string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["Seller"] = returnUrl;
            ViewData["categorie"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSeller(RegisterSellerViewModel model, string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString());
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "seller"));

                    var message = new MailMessage();
                    message.From = new MailAddress("3bros.shop.suport@gmail.com");
                    message.To.Add("3bros.shop.suport@gmail.com");
                    message.Subject = "Một người bán mới vừa đăng ký thông qua hình thức đại lý";
                    message.Body = String.Format("Tên cửa hàng: {0}\n" +
                                            "Địa chỉ email: {1}\n" +
                                            "Số điện thoại: {2}" + 
                                            "Tên đường: {3}\n" +
                                            "Số nhà: {4}\n" +
                                            "Postcode: {5}\n" +
                                            "Thành phố: {6}\n" +
                                            "Mô tả: {7}\n",
                                            model.NameOfBusiness, model.Email,model.PhoneNumber, model.Street, model.ApartmentNumber, model.Postcode, model.City, model.Description);

                    Seller nieuweSeller = new Seller(model.NameOfBusiness, model.Email, model.PhoneNumber,  model.Description, model.Street, model.ApartmentNumber, model.Postcode, model.City, false);
                    _userRepository.Add(new User
                    {
                        EmailAddress = model.Email,
                        FirstName = model.NameOfBusiness,
                        FamilyName = "",
                        Sex = Shop.Models.Domain.Enum.Sex.Different
                    });

                    _sellerRepository.Add(nieuweSeller);
                    _userRepository.SaveChanges();
                    _sellerRepository.SaveChanges();


                    var filePath = @"wwwroot/images/seller/" + nieuweSeller.SellerId + "/logo.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Logo.CopyToAsync(fileStream);
                    fileStream.Close();

                    var attachment = new Attachment(@"wwwroot/images/seller/" + nieuweSeller.SellerId + "/logo.jpg");
                    attachment.Name = "logo.jpg";
                    message.Attachments.Add(attachment);
                    var SmtpServer = new SmtpClient("smtp.gmail.com");
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(message);
                    message.Attachments.Remove(attachment);
                    attachment.Dispose();

                    message.From = new MailAddress("3bros.shop.suport@gmail.com");
                    message.To.Clear();
                    message.To.Add(model.Email);
                    message.Subject = "Yêu cầu của bạn để trở thành người bán trên 3BrosShop";
                    message.Body = String.Format("Kính gửi {0}, \n" +
                                            "yêu cầu của bạn để trở thành người bán tại 3brosShop đang được xác nhận \n\n" +
                                            "Thông tin dưới đây sẽ được xem xét bởi quản trị viên. vui lòng bạn đợi một email xác nhận trong một thời gian ngắn." +
                                            "Tên cửa hàng: {1}\n" +
                                            "Địa chỉ Email: {2}\n" +
                                            "Tên đường: {3}\n" +
                                            "Số nhà: {4}\n" +
                                            "Postcode: {5}\n" +
                                            "Thành Phố: {6}\n" +
                                            "Mô tả: {7}\n",
                                            model.NameOfBusiness, model.NameOfBusiness, model.Email, model.Street, model.ApartmentNumber, model.Postcode, model.City, model.Description);
                    SmtpServer.Send(message);

                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            ViewData["categorie"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Đăng xuất người dùng.");
            if (Url.IsLocalUrl(returnUrl) && returnUrl != null)
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Lỗi từ nhà cung cấp khác: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("tài khoản người dùng đăng nhập {FirstName} ", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
                ViewData["Sex"] = Geslachten();
                ViewData["LoginProvider"] = info.LoginProvider;

                var email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? "Email của bạn";
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "John";
                var familyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "Doe";
                var sexCase = info.Principal.FindFirstValue(ClaimTypes.Gender) ?? "Different";
                var sex = Sex.Different;

                switch (sexCase.ToLower())
                {
                    case "male":
                        sex = Sex.Male;
                        break;
                    case "female":
                        sex = Sex.Female;
                        break;
                    default:
                        sex = Sex.Different;
                        break;
                }

                return View("ExternalLogin", new ExternalLoginViewModel
                {
                    Email = email,
                    FirstName = firstName,
                    FamilyName = familyName,
                    Sex = sex
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Lỗi đăng nhập trong khi thông tin đang xác thực");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var _user = new User
                    {
                        FirstName = model.FirstName,
                        FamilyName = model.FamilyName,
                        Sex = model.Sex,
                        PhoneNumber = "",
                        Address = "",
                        EmailAddress = model.Email
                    };

                    _userRepository.Add(_user);
                    _userRepository.SaveChanges();

                    var password = Guid.NewGuid().ToString();
                    await _userManager.AddPasswordAsync(user, password);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, token);

                    var message = new MailMessage();
                    message.From = new MailAddress("3bros.shop.suport@gmail.com");
                    message.To.Add(model.Email);
                    message.Subject = "Bạn đã tạo tài khoản tại 3BrosShop";

                    message.Body = String.Format("Kính gửi, \n" +
                   "Gần đây bạn đã tạo tài khoản trên 3BrosShop thông qua facebook hoặc google.\n\n" +
                   "Thông tin chi tiết: \n" +
                   "Địa chỉ Email: " + model.Email + "\n" +
                   "Mật khẩu: " + password + "\n\n" +
                   "Bạn nên đổi mật khẩu khi đăng nhập lần đầu tiên \n\n" +
                   "Trân trọng, \n" +
                   "3Bros team");

                    var SmtpServer = new SmtpClient("smtp.gmail.com");
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(message);

                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("Người đã tạo tài khoản {FirstName}", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Đặt lại mật khẩu",
                   $"Vui lòng đặt lại mật khẩu của bạn bằng cách nhấp vào đây: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (code == null)
            {
                throw new ApplicationException("bạn phải nhập mã code để lập lại mật khẩu.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Description.StartsWith("User name") && error.Description.EndsWith("đã được thực hiện"))
                    ModelState.AddModelError(string.Empty, "Địa chỉ e-mail bạn đã chọn đã được sử dụng. Vui lòng nhập một địa chỉ email khác.");
                else
                    ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion
    }
}
