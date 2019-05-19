using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Models;
using Shop.Models.AdminViewModels;
using Shop.Models.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Shop.Data;
using Shop.Models.Domain.Interface;
using Shop.Models.Domain.Enum;
using Shop.Services;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ISellerRepository _sellerRepository;
        private readonly IItemsRepository _itemsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ILogger<AdminController> logger,
            SignInManager<ApplicationUser> signInManager,
            ISellerRepository sellerRepository,
            IItemsRepository itemsRepository,
            IUserRepository userRepository,
            IOrderItemRepository orderItemRepository,
            ICategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _itemsRepository = itemsRepository;
            _orderItemRepository = orderItemRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Địa chỉ email hoặc mật khẩu của bạn không chính xác. Vui lòng thử lại");
                    return View(model);
                }
                else
                {
                    var claims = await _userManager.GetClaimsAsync(user);

                    if (!claims.Any(claimpje => claimpje.Value == "admin"))
                    {
                        ModelState.AddModelError(string.Empty, "Bạn không có quyền để đăng nhập vào phần này.");
                        return View(model);
                    }
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Người dùng đăng nhập.");
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Địa chỉ email hoặc mật khẩu của bạn không chính xác. Vui lòng thử lại");
                        return View(model);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Đăng xuất người dùng.");

            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Dashboard()
        {
            return View(new DashboardViewModel(_sellerRepository.getNumberOfSeller(), _itemsRepository.getNumberofItemsRequests(), _orderItemRepository.getSoldThisMonth(), _orderItemRepository.getUsedThisMonth()));
        }

        [HttpGet]
        public IActionResult SellerRequest()
        {
            return View(new SellerReviewsViewModel(_sellerRepository.GetSellerYetNotApproved(_sellerRepository.GetAll())));
        }

        [HttpGet]
        public IActionResult SellerRequestEvaluation(int Id)
        {
            Seller geselecteerdeSellerEvaluatie = _sellerRepository.GetBySellerIdNotAccepted(Id);
            if (geselecteerdeSellerEvaluatie == null)
            {
                return RedirectToAction("SellerRequest");
            }
            return View(new SellerEditViewModel(geselecteerdeSellerEvaluatie));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSellerRequest(SellerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                await _userManager.DeleteAsync(user);

                _sellerRepository.Remove(model.SellerId);
                _sellerRepository.SaveChanges();

                var filePath = @"wwwroot/images/seller/" + model.SellerId;
                Directory.Delete(filePath, true);

                var message = new MailMessage();
                message.From = new MailAddress("3bros.shop.suport@gmail.com");
                message.To.Add(model.Email);
                message.Subject = "Yêu cầu của bạn để trở thành người bán trên 3BrosShop đã bị từ chối.";

                if (model.Note != null)
                {
                    message.Body = String.Format("Kính gửi " + model.Name + ", \n\n" +
                   "Yêu cầu của bạn trở thành người bán trên 3BrosShop đã bị từ chối. \n\n" +
                   model.Note + "\n\n" +
                   "Nếu bạn nghĩ rằng bạn vẫn có quyền trở thành người bán tại 3BrosShop, chúng tôi khuyên bạn nên gửi yêu cầu mới. \n\n" +
                   "Trân trọng, \n" +
                  "3Bros team");
                }
                else
                {
                    message.Body = String.Format("Kính gửi " + model.Name + ", \n\n" +
                  "Yêu cầu của bạn trở thành người bán trên 3BrosShop đã bị từ chối. \n\n" +
                  "Nếu bạn nghĩ rằng bạn vẫn có quyền trở thành người bán tại 3BrosShop, chúng tôi khuyên bạn nên gửi yêu cầu mới. \n\n"  +
                  "Trân trọng, \n" +
                  "3Bros team");
                }

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(message);

                return RedirectToAction("SellerRequest");
            }
            return View(nameof(SellerRequestEvaluation), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptSellerRequest(SellerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Seller sellerInDb = _sellerRepository.GetBySellerIdNotAccepted(model.SellerId);

                if (sellerInDb.Name != model.Name)
                {
                    sellerInDb.Name = model.Name;
                }

                if (sellerInDb.EmailAddress != model.Email)
                {
                    sellerInDb.EmailAddress = model.Email;
                }

                if (sellerInDb.Description != model.Description)
                {
                    sellerInDb.Description = model.Description;
                }

                if (sellerInDb.Street != model.Street)
                {
                    sellerInDb.Street = model.Street;
                }

                if (sellerInDb.ApartmentNumber != model.ApartmentNumber)
                {
                    sellerInDb.ApartmentNumber = model.ApartmentNumber;
                }

                if (sellerInDb.Postcode != model.Postcode)
                {
                    sellerInDb.Postcode = model.Postcode;
                }

                if (sellerInDb.City != model.City)
                {
                    sellerInDb.City = model.City;
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                user.EmailConfirmed = true;

                var password = Guid.NewGuid().ToString();
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, password);


                sellerInDb.Approved = true;
                _sellerRepository.SaveChanges();

                if (model.Image != null)
                {
                    var filePath = @"wwwroot/images/seller/" + model.SellerId + "/logo.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Image.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                var message = new MailMessage();
                message.From = new MailAddress("3bros.shop.suport@gmail.com");
                message.To.Add(model.Email);
                message.Subject = "Yêu cầu của bạn để trở thành người bán trên 3BrosShop được chấp nhận!";

                if (model.Note != null)
                {
                    message.Body = String.Format("Kính gửi " + model.Name + ", \n\n" +
                   "Yêu cầu gần đây của bạn để trở thành người bán tại 3BrosShop đã được chấp nhận! \n\n" +
                   model.Note + "\n\n" +
                   "Thông tin chi tiết: \n" +
                   "Địa chỉ Email: " + model.Email + "\n" +
                   "Mật khẩu: " + password + "\n\n" +
                   "Chúng tôi khuyên bạn nên đổi mật khẩu khi đăng nhập lần đầu tiên. \n\n" +
                   "Trân trọng, \n" +
                   "3Bros team");
                }
                else
                {
                    message.Body = String.Format("Kính gửi " + model.Name + ", \n\n" +
                   "Yêu cầu gần đây của bạn để trở thành người bán tại 3BrosShop đã được chấp nhận! \n\n" +
                   "Thông tin chi tiết: \n" +
                   "Địa chỉ Email: " + model.Email + "\n" +
                   "Mật khẩu: " + password + "\n\n" +
                   "Chúng tôi khuyên bạn nên đổi mật khẩu khi đăng nhập lần đầu tiên. \n\n" +
                   "Trân trọng, \n" +
                  "3Bros team");
                }

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(message);

                return RedirectToAction("SellerRequest");
            }
            return View(nameof(SellerRequestEvaluation), model);
        }

        private async Task CreateUser(string userName, string email, string password, string role)
        {
            var user = new ApplicationUser { UserName = userName, Email = email, EmailConfirmed = true };
            await _userManager.CreateAsync(user, password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            ListIdentityUser list = new ListIdentityUser();
            List<ApplicationUser> listUser = await _userManager.Users.ToListAsync();
            foreach (var item in listUser)
            {

                IList<Claim> tmp = await _userManager.GetClaimsAsync(item);
                User userInfo = _userRepository.GetBy(item.UserName);
                list.ListUser.Add(
                    new UsersViewModel()
                    {
                        User = item,
                        UserInfo = userInfo,
                        Password = "",
                        RoleName = tmp[0].Value
                    });

            }

            return View(list);
        }

        [HttpGet]
        public IActionResult UsersAdd()
        {
            ViewData["Role"] = Role();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersAdd(UsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.UserName == model.User.Email);
                if (user == null)
                {
                    await CreateUser(model.User.Email, model.User.Email, model.Password, model.RoleName);
                    _userRepository.Add(new User
                    {
                        EmailAddress = model.User.Email,
                        FirstName = model.UserInfo.FirstName,
                        FamilyName = model.UserInfo.FamilyName,
                        PhoneNumber = "#",
                        Address = "#"
                    });

                    if (model.RoleName == "seller")
                    {
                        Seller newSeller = new Seller(model.UserInfo.FamilyName + " "+ model.UserInfo.FirstName , model.User.UserName, "#","#", "#", "#", "#", "#", true);
                        _sellerRepository.Add(newSeller);
                    }

                    var message = new MailMessage();
                    message.From = new MailAddress("3bros.shop.suport@gmail.com");
                    message.To.Add(model.User.Email);
                    message.Subject = "Tài khoản được tạo trên 3BrosShop";

                    message.Body = String.Format("Kính gửi, \n" +
                                                 "Tài khoản được tạo bởi Admin\n\n" +
                                                 "Thông tin chi tiết: \n" +
                                                 "Địa chỉ Email: " + model.User.Email + "\n" +
                                                 "Mật khẩu: " + model.Password + "\n" +
                                                 "Quyền: "+ model.RoleName + "\n\n" + 
                                                 "Bạn nên đổi mật khẩu khi đăng nhập và cập nhật thông tin cá nhân\n\n" +
                                                 "Trân trọng, \n" +
                                                 "3Bros team");

                    var SmtpServer = new SmtpClient("smtp.gmail.com");
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(message);

                    _userRepository.SaveChanges();
                    _sellerRepository.SaveChanges();
                    return RedirectToAction("Users");
                }
            }
            return View(nameof(UsersAdd), model);
        }

        private SelectList Sex()
        {
            var sexList = new List<Sex>();
            foreach (Sex sex in Enum.GetValues(typeof(Sex)))
            {
                sexList.Add(sex);
            }
            return new SelectList(sexList);
        }

        private SelectList Role()
        {
            return new SelectList(new List<string>(new string[] {"admin", "seller", "customers"}));
        }

        [HttpGet]
        public async Task<IActionResult> UsersEdit(string id)
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.UserName == id);
            User userInfo = _userRepository.GetBy(id);
            IList<Claim> tmp = await  _userManager.GetClaimsAsync(user);

            ViewData["Sex"] = Sex();
            ViewData["Role"] = Role();
            return View(new UsersViewModel()
            {
                User = user,
                UserInfo = userInfo,
                Password = "",
                RoleName = tmp[0].Value
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersEdit(UsersViewModel model)
        {
            if (ModelState.IsValid)
            {

                List<ApplicationUser> user = await _userManager.Users.ToListAsync();
                ApplicationUser u = user.Find(x => x.UserName == model.User.UserName);

                IList<Claim> tmp = await _userManager.GetClaimsAsync(u);
                
                if (model.Password != "" && model.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(u);
                    await _userManager.ResetPasswordAsync(u, token, model.Password);
                    var message = new MailMessage();
                    message.From = new MailAddress("3bros.shop.suport@gmail.com");
                    message.To.Add(model.User.UserName);
                    message.Subject = "Tài khoản của bạn đã được thay đổi mật khẩu";

                    message.Body = String.Format("Kính gửi, \n" +
                                                 "Mật khẩu của bạn đã được thay đổi bởi Admin\n\n" +
                                                 "Thông tin chi tiết: \n" +
                                                 "Địa chỉ Email: " + model.User.UserName + "\n" +
                                                 "Mật khẩu: " + model.Password + "\n\n" +
                                                 "Bạn nên đổi mật khẩu khi đăng nhập \n\n" +
                                                 "Trân trọng, \n" +
                                                 "3Bros team");

                    var SmtpServer = new SmtpClient("smtp.gmail.com");
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(message);
                }

                await _userManager.ReplaceClaimAsync(u, tmp[0], new Claim(ClaimTypes.Role, model.RoleName));
                User userInfo = _userRepository.GetBy(model.User.UserName);
                userInfo.FirstName = model.UserInfo.FirstName;
                userInfo.FamilyName = model.UserInfo.FamilyName;
                userInfo.Sex = model.UserInfo.Sex;

                if (tmp[0].Value != model.RoleName && model.RoleName == "seller")
                {
                    Seller seller = _sellerRepository.GetByEmail(model.User.UserName);
                    if (seller == null)
                    {
                        Seller newSeller = new Seller(userInfo.FamilyName +" "+userInfo.FirstName , model.User.UserName, "#","#", "#", "#", "#", "#", true);
                        _sellerRepository.Add(newSeller);
                    }
                }

                _sellerRepository.SaveChanges();
                _userRepository.SaveChanges();
                return RedirectToAction("Users");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ItemsRequest(int Id)
        {
            Items selectedItemsEvaluation = _itemsRepository.GetByItemsIdNotAccepted(Id);
            if ( selectedItemsEvaluation == null)
            {
                return RedirectToAction("ItemsRequests");
            }
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offer();
            return View(new ItemsProcessViewModel(selectedItemsEvaluation));
        }

        [HttpGet]
        public IActionResult SellerAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerAdd(ManuallyNewSellerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var password = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "seller"));

                    Seller nieuweSeller = new Seller(model.Name, model.Email, model.PhoneNumber, model.Description, model.StreetName, model.ApartmentNumber, model.Postcode, model.City, true);
                    _userRepository.Add(new User{
                        EmailAddress = model.Email,
                        FirstName = model.Name,
                        FamilyName = "",
                        PhoneNumber = "#",
                        Address = "#",
                        Sex = Shop.Models.Domain.Enum.Sex.Different
                    });
                    _sellerRepository.Add(nieuweSeller);
                    _userRepository.SaveChanges();
                    _sellerRepository.SaveChanges();

                    var filePath = @"wwwroot/images/seller/" + nieuweSeller.SellerId + "/logo.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Image.CopyToAsync(fileStream);
                    fileStream.Close();


                    var message = new MailMessage();
                    message.From = new MailAddress("3bros.shop.suport@gmail.com");
                    message.To.Add(model.Email);
                    message.Subject = "Yêu cầu của bạn để trở thành người bán trên 3BrosShop được chấp nhận!";

                    if (model.Note != null)
                    {
                        message.Body = String.Format("Kính gửi " + model.Name + ", \n" +
                       "Bạn đã được thêm làm người bán trên 3BrosShop!\n\n" +
                       model.Note + "\n\n" +
                       "Thông tin chi tiết: \n" +
                       "Địa chỉ Email: " + model.Email + "\n" +
                       "Mật Khẩu: " + password + "\n\n" +
                       "Chúng tôi khuyên bạn nên thay đổi mật khẩu với lần đăng nhập đầu tiên. \n\n" +
                       "Trân trọng, \n" +
                      "3Bros team");
                    }
                    else
                    {
                        message.Body = String.Format("Kính gửi " + model.Name + ", \n" +
                        "Bạn đã được thêm làm người bán trên 3BrosShop!\n\n" +
                        "Thông tin chi tiết: \n" +
                        "Địa chỉ Email: " + model.Email + "\n" +
                        "Mật Khẩu: " + password + "\n\n" +
                        "Chúng tôi khuyên bạn nên thay đổi mật khẩu với lần đăng nhập đầu tiên. \n\n" +
                        "Trân trọng, \n" +
                        "3Bros team");
                    }

                    var SmtpServer = new SmtpClient("smtp.gmail.com");
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(message);

                    return RedirectToAction("Dashboard");
                }
            }
            return View(nameof(SellerAdd), model);
        }

        [HttpGet]
        public IActionResult SellerOverview()
        {
            return View(new SellerOverviewViewModel(_sellerRepository.GetSellerApproved(_sellerRepository.GetAll())));
        }

        [HttpGet]
        public IActionResult SellerEdit(int Id)
        {
            Seller selectedSeller = _sellerRepository.GetBySellerId(Id);
            if (selectedSeller == null)
            {
                return RedirectToAction("SellerOverview");
            }
            return View(new SellerEditViewModel(selectedSeller));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerEdit(SellerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Seller sellerInDb = _sellerRepository.GetBySellerId(model.SellerId);

                if (sellerInDb.Name != model.Name)
                {
                    sellerInDb.Name = model.Name;
                }

                if (sellerInDb.EmailAddress != model.Email)
                {
                    sellerInDb.EmailAddress = model.Email;
                }

                if (sellerInDb.Description != model.Description)
                {
                    sellerInDb.Description = model.Description;
                }

                if (sellerInDb.Street != model.Street)
                {
                    sellerInDb.Street = model.Street;
                }

                if (sellerInDb.ApartmentNumber != model.ApartmentNumber)
                {
                    sellerInDb.ApartmentNumber = model.ApartmentNumber;
                }

                if (sellerInDb.Postcode != model.Postcode)
                {
                    sellerInDb.Postcode = model.Postcode;
                }

                if (sellerInDb.City != model.City)
                {
                    sellerInDb.City = model.City;
                }

                if (sellerInDb.PhoneNumber != model.PhoneNumber)
                {
                    sellerInDb.PhoneNumber = model.PhoneNumber;
                }

                _sellerRepository.SaveChanges();

                if (model.Image != null)
                {
                    var filePath = @"wwwroot/images/seller/" + model.SellerId + "/logo.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Image.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                return RedirectToAction("SellerOverview");
            }
            return View(nameof(SellerEdit), model);
        }

        [HttpGet]
        public IActionResult ItemsRequests()
        {
            //implement
            return View(new ItemsRequestsViewModel(_itemsRepository.GetItemsNotyetApproved(_itemsRepository.GetAll())));
        }

        [HttpGet]
        public IActionResult AddItems()
        {
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offer();
            return View();
        }

        private SelectList Offer()
        {
            var offers = new List<Offer>();
            foreach (Offer offerItem in Enum.GetValues(typeof(Offer)))
            {
                offers.Add(offerItem);
            }
            return new SelectList(offers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItems(ManuallyNewItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                Items newItems = new Items(model.Name, model.Price ,model.Description, 0, @"temp", _categoryRepository.GetByName(model.Category), model.Street, model.ApartmentNumber, model.Postcode, model.City, _sellerRepository.GetBySellerId(model.SellerId), model.Offer, true);
                _itemsRepository.Add(newItems);
                _itemsRepository.SaveChanges();

                newItems.Image = @"images\items\" + newItems.ItemsId + @"\";
                _itemsRepository.SaveChanges();

                var filePath = @"wwwroot/images/items/" + newItems.ItemsId + "/thumb.jpg";
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                var fileStream = new FileStream(filePath, FileMode.Create);
                await model.Thumbnail.CopyToAsync(fileStream);
                fileStream.Close();

                for (int i = 0; i < model.Image.Count; i++)
                {
                    filePath = @"wwwroot/images/items/" + newItems.ItemsId + "/Image/" + (i + 1) + ".jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Image[i].CopyToAsync(fileStream);
                    fileStream.Close();
                }


                return RedirectToAction("ItemsOverview");

            }
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offer();
            return View(nameof(AddItems), model);
        }


        [HttpGet]
        public IActionResult ItemsOverview()
        {
            return View(new ItemsOverviewViewModel(_itemsRepository.GetAllApproved()));
        }

        [HttpGet]
        public async Task<IActionResult> ItemsDelete(string id)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Index", "Home");
            var itemsList = _itemsRepository.GetAllApproved().Where(item => item.ItemsId == Convert.ToInt32(id)).ToList();

            if (itemsList.Count > 0)
            {
                var items = itemsList[0];
                _itemsRepository.Remove(items.ItemsId);
                _itemsRepository.SaveChanges();
            }
            return RedirectToAction("ItemsOverview");
        }

        [HttpGet]
        public IActionResult ItemsEdit(int Id)
        {
            Items geselecteerdeItems = _itemsRepository.GetByItemsId(Id);
            if (geselecteerdeItems == null)
            {
                return RedirectToAction("ItemsOverview");
            }
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offer();
            return View(new ItemsProcessViewModel(geselecteerdeItems));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemsEdit(ItemsProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                Items itemsInDb = _itemsRepository.GetByItemsId(model.ItemsId);

                if (itemsInDb.Name != model.Name)
                {
                    itemsInDb.Name = model.Name;
                }

                if (itemsInDb.Description != model.Description)
                {
                    itemsInDb.Description = model.Description;
                }

                if (itemsInDb.Price != model.Price)
                {
                    itemsInDb.Price = model.Price;
                }


                if (itemsInDb.Category.Name != model.Category)
                {
                    itemsInDb.Category = _categoryRepository.GetByName(model.Category);
                }

                if (itemsInDb.Street != model.Street)
                {
                    itemsInDb.Street = model.Street;
                }

                if (itemsInDb.ApartmentNumber != model.ApartmentNumber)
                {
                    itemsInDb.ApartmentNumber = model.ApartmentNumber;
                }

                if (itemsInDb.Postcode != model.Postcode)
                {
                    itemsInDb.Postcode = model.Postcode;
                }

                if (itemsInDb.City != model.City)
                {
                    itemsInDb.City = model.City;
                }

                if (itemsInDb.Offer != model.Offer)
                {
                    itemsInDb.Offer = model.Offer;
                }

                _itemsRepository.SaveChanges();

                if (model.Thumbnail != null)
                {
                    var filePath = @"wwwroot/" + itemsInDb.Image + "thumb.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Thumbnail.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                if (model.Image != null)
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(@"wwwroot/" + itemsInDb.Image + "Image/");

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }

                    for (int i = 0; i < model.Image.Count; i++)
                    {
                        var filePath = @"wwwroot/" + itemsInDb.Image + "Image/" + (i + 1) + ".jpg";
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        var fileStream = new FileStream(filePath, FileMode.Create);
                        await model.Image[i].CopyToAsync(fileStream);
                        fileStream.Close();
                    }
                }

                return RedirectToAction("ItemsOverview");
            }
            return View(nameof(ItemsEdit), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItems(ItemsProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                Items itemsInDb = _itemsRepository.GetByItemsIdNotAccepted(model.ItemsId);
                _itemsRepository.Remove(model.ItemsId);
                _itemsRepository.SaveChanges();

                var filePath = @"wwwroot/" + itemsInDb.Image;
                Directory.Delete(filePath, true);


                var emailadres = itemsInDb.Seller.EmailAddress;

                var message = new MailMessage();
                message.From = new MailAddress("3bros.shop.suport@gmail.com");
                message.To.Add(emailadres);
                message.Subject = "Yêu cầu của bạn để thêm mặt hàng mới trên 3BrosShop đã bị từ chối.";

                if (model.Note != null)
                {
                    message.Body = String.Format("Kính gửi " + model.NameSeller + ", \n\n" +
                   "Yêu cầu gần đây của bạn để thêm mặt hàng vào 3BrosShop đã bị từ chối. \n\n" +
                   model.Note + "\n\n" +
                   "Nếu bạn nghĩ rằng bạn vẫn có quyền thêm mặt hàng này vào 3BrosShop, chúng tôi khuyên bạn nên gửi yêu cầu mới. \n\n" +
                   "Trân trọng, \n" +
                  "3bros team");
                }
                else
                {
                    message.Body = String.Format("Kính gửi " + model.NameSeller + ", \n\n" +
                    "Yêu cầu gần đây của bạn để thêm mặt hàng vào 3BrosShop đã bị từ chối. \n\n" +
                    "Nếu bạn nghĩ rằng bạn vẫn có quyền thêm mặt hàng này vào 3BrosShop, chúng tôi khuyên bạn nên gửi yêu cầu mới. \n\n" +
                    "Trân trọng, \n" +
                    "3bros team");
                }

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(message);

                return RedirectToAction("ItemsRequests");
            }
            return View(nameof(SellerRequestEvaluation), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptItemsRequest(ItemsProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                Items itemsInDb = _itemsRepository.GetByItemsIdNotAccepted(model.ItemsId);

                if (itemsInDb.Name != model.Name)
                {
                    itemsInDb.Name = model.Name;
                }

                if (itemsInDb.Description != model.Description)
                {
                    itemsInDb.Description = model.Description;
                }

                if (itemsInDb.Price != model.Price)
                {
                    itemsInDb.Price = model.Price;
                }

                if (itemsInDb.Category.Name != model.Category)
                {
                    itemsInDb.Category = _categoryRepository.GetByName(model.Category);
                }

                if (itemsInDb.Street != model.Street)
                {
                    itemsInDb.Street = model.Street;
                }

                if (itemsInDb.ApartmentNumber != model.ApartmentNumber)
                {
                    itemsInDb.ApartmentNumber = model.ApartmentNumber;
                }

                if (itemsInDb.Postcode != model.Postcode)
                {
                    itemsInDb.Postcode = model.Postcode;
                }

                if (itemsInDb.City != model.City)
                {
                    itemsInDb.City = model.City;
                }

                if (itemsInDb.Offer != model.Offer)
                {
                    itemsInDb.Offer = model.Offer;
                }

                itemsInDb.Approved = true;

                _itemsRepository.SaveChanges();

                if (model.Thumbnail != null)
                {
                    var filePath = @"wwwroot/" + itemsInDb.Image + "thumb.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Thumbnail.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                if (model.Image != null)
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(@"wwwroot/" + itemsInDb.Image + "Image/");

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }

                    for (int i = 0; i < model.Image.Count; i++)
                    {
                        var filePath = @"wwwroot/" + itemsInDb.Image + "Image/" + (i + 1) + ".jpg";
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        var fileStream = new FileStream(filePath, FileMode.Create);
                        await model.Image[i].CopyToAsync(fileStream);
                        fileStream.Close();
                    }
                }


                var emailadres = itemsInDb.Seller.EmailAddress;

                var message = new MailMessage();
                message.From = new MailAddress("3bros.shop.suport@gmail.com");
                message.To.Add(emailadres);
                message.Subject = "Yêu cầu của bạn để thêm mặt hàng mới trên 3BrosShop đã được chấp nhận!";

                if (model.Note != null)
                {
                    message.Body = String.Format("Kính gửi " + model.NameSeller + ", \n\n" +
                   "Yêu cầu của bạn để thêm mặt hàng mới trên 3BrosShop đã được chấp nhận. \n\n" +
                   model.Note + "\n\n" +
                   "Trân trọng, \n" +
                  "3Bros team");
                }
                else
                {
                    message.Body = String.Format("Kính gửi " + model.NameSeller + ", \n\n" +
                    "Yêu cầu của bạn để thêm biên lai mới trên 3BrosShop đã được chấp nhận. \n\n" +
                    "Trân trọng, \n" +
                    "3Bros team");
                }

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(message);

                return RedirectToAction("ItemsOverview");
            }
            return View(nameof(ItemsRequest), model);
        }

        [HttpGet]
        public IActionResult LayoutSliderIndex()
        {
            return View(new ItemsOverviewViewModel(_itemsRepository.GetCouponOfferSlider(_itemsRepository.GetAllApproved())));
        }

        [HttpGet]
        public IActionResult LayoutOffers()
        {
            return View(new ItemsOverviewViewModel(_itemsRepository.GetItemsOfferStandardAndSlider(_itemsRepository.GetAllApproved())));
        }

        [HttpGet]
        public IActionResult SoldItemsView(int Id)
        {
            GeneratePDF(Id);
            return View(new SoldItemsViewModel(_orderItemRepository.GetById(Id)));
        }

        [HttpGet]
        public IActionResult SoldItems()
        {
            return View(new OverviewSoldOrderViewModel(_orderItemRepository.getSoldOrder()));
        }

        [HttpGet]
        public IActionResult UsedItems()
        {
            return View(new OverviewUsedOrderViewModel(_orderItemRepository.getUsedOrder()));
        }

        public void GenerateQR(string qrcode)
        {
            var bonPath = @"wwwroot/images/temp/";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://" + Request.Host + "/checkout/OrderQr?Id=" + qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(bonPath + qrcode + ".png", ImageFormat.Png);
        }

        public void GeneratePDF(int Id)
        {
            var orderItem = _orderItemRepository.GetById(Id);
            var items = _itemsRepository.GetByItemsId(orderItem.Items.ItemsId);
            var seller = _sellerRepository.GetBySellerId(items.Seller.SellerId);
            var user = _userManager.GetUserAsync(User);
            var _user = _userRepository.GetBy(user.Result.Email);

            ViewData["path"] = @"/pdf/c_" + orderItem.QRCode + ".pdf";

            string value = String.Format( orderItem.Price.ToString() + "vnd");
            string date = orderItem.CreationDate.AddYears(1).ToString("dd/MM/yyyy");
            string validity = String.Format("Co hieu luc: " + date);
            var pdf = new Document(PageSize.A5.Rotate(), 81, 225, 25, 0);
            
            GenerateQR(orderItem.QRCode);
            var imageURL = @"wwwroot/images/temp/" + orderItem.QRCode + ".png";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.ScaleToFit(145f, 145f);
            var logoURL = @"wwwroot/images/logo.png";
            var logoURLSeller = @"wwwroot" + seller.GetLogoPath();
            var kadoURL = @"wwwroot/images/kado.jpg";
            iTextSharp.text.Image kado = iTextSharp.text.Image.GetInstance(kadoURL);
            iTextSharp.text.Image logoLL = iTextSharp.text.Image.GetInstance(logoURL);
            iTextSharp.text.Image logoSeller = iTextSharp.text.Image.GetInstance(logoURLSeller);
            //Paragraph naamBon = new Paragraph("Items: " + items.FirstName);

            logoLL.SetAbsolutePosition(20, 15);
            logoLL.ScaleToFit(188f, 100f);
            logoSeller.ScaleToFit(188f, 100f);
            logoSeller.SetAbsolutePosition(410, 15);
            jpg.SetAbsolutePosition(225, 0);
            kado.SetAbsolutePosition(65, 161);

            iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 23);
            iTextSharp.text.Font arial18 = FontFactory.GetFont("Arial", 14);
            iTextSharp.text.Font arialSmall = FontFactory.GetFont("Arial", 7);

            Paragraph amount = new Paragraph(value, arial);
            amount.SpacingAfter = 50;
            Paragraph nameSeller = new Paragraph(items.Name, arial);
            nameSeller.SpacingAfter = 0;
            Paragraph giveBy = new Paragraph("Tang boi: " + _user.FirstName, arial18);
            Paragraph valid = new Paragraph(validity, arial18);

            amount.Alignment = Element.ALIGN_LEFT;

            nameSeller.Alignment = Element.ALIGN_LEFT;
            giveBy.Alignment = Element.ALIGN_LEFT;
            valid.Alignment = Element.ALIGN_LEFT;

            Phrase qrCodeString = new Phrase(orderItem.QRCode, arialSmall);

            PdfWriter writer = PdfWriter.GetInstance(pdf, new FileStream(@"wwwroot/pdf/c_" + orderItem.QRCode + ".pdf", FileMode.Create));
            pdf.Open();

            ColumnText.ShowTextAligned(writer.DirectContent,
            Element.ALIGN_MIDDLE, qrCodeString, 195, 4, 0);

            pdf.Add(logoLL);
            pdf.Add(logoSeller);
            pdf.Add(nameSeller);
            pdf.Add(amount);
            pdf.Add(giveBy);
            pdf.Add(valid);
            pdf.Add(jpg);
            pdf.Add(kado);
            pdf.Close();

            System.IO.File.Delete(imageURL);
        }

        public IActionResult AddItem()
        {
            throw new NotImplementedException();
        }
    }
}