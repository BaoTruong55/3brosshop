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
using Shop.Models.Domain.Interface;

namespace Shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISellerRepository _sellerRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger _logger;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ILogger<AdminController> logger,
            SignInManager<ApplicationUser> signInManager,
            ISellerRepository sellerRepository,
            IReceiptRepository receiptRepository,
            IUserRepository userRepository,
            IOrderItemRepository orderItemRepository,
            ICategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _receiptRepository = receiptRepository;
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
            return View(new DashboardViewModel(_sellerRepository.getNumberOfSeller(), _receiptRepository.getNumberofReceiptRequests(), _orderItemRepository.getSoldThisMonth(), _orderItemRepository.getUsedThisMonth()));
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

        [HttpGet]
        public IActionResult ReceiptRequest(int Id)
        {
            Receipt geselecteerdebonEvaluatie = _receiptRepository.GetByReceiptIdNotAccepted(Id);
            if (geselecteerdebonEvaluatie == null)
            {
                return RedirectToAction("ReceiptRequests");
            }
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offer();
            return View(new ReceiptProcessViewModel(geselecteerdebonEvaluatie));
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

                    Seller nieuweSeller = new Seller(model.Name, model.Email, model.Description, model.StreetName, model.ApartmentNumber, model.Postcode, model.City, true);
                    _sellerRepository.Add(nieuweSeller);
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
        public IActionResult ReceiptRequests()
        {
            //implement
            return View(new ReceiptRequestsViewModel(_receiptRepository.GetReceiptNotyetApproved(_receiptRepository.GetAll())));
        }

        [HttpGet]
        public IActionResult AddReceipt()
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
        public async Task<IActionResult> AddReceipt(ManuallyNewReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                Receipt newReceipt = new Receipt(model.Name, model.Price ,model.Description, 0, @"temp", _categoryRepository.GetByName(model.Category), model.Street, model.ApartmentNumber, model.Postcode, model.City, _sellerRepository.GetBySellerId(model.SellerId), model.Offer, true);
                _receiptRepository.Add(newReceipt);
                _receiptRepository.SaveChanges();

                newReceipt.Image = @"images\receipt\" + newReceipt.ReceiptId + @"\";
                _receiptRepository.SaveChanges();

                var filePath = @"wwwroot/images/receipt/" + newReceipt.ReceiptId + "/thumb.jpg";
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                var fileStream = new FileStream(filePath, FileMode.Create);
                await model.Thumbnail.CopyToAsync(fileStream);
                fileStream.Close();

                for (int i = 0; i < model.Image.Count; i++)
                {
                    filePath = @"wwwroot/images/receipt/" + newReceipt.ReceiptId + "/Image/" + (i + 1) + ".jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Image[i].CopyToAsync(fileStream);
                    fileStream.Close();
                }


                return RedirectToAction("ReceiptOverview");

            }
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offer();
            return View(nameof(AddReceipt), model);
        }


        [HttpGet]
        public IActionResult ReceiptOverview()
        {
            return View(new ReceiptOverviewViewModel(_receiptRepository.GetAllApproved()));
        }

        [HttpGet]
        public IActionResult ReceiptEdit(int Id)
        {
            Receipt geselecteerdeReceipt = _receiptRepository.GetByReceiptId(Id);
            if (geselecteerdeReceipt == null)
            {
                return RedirectToAction("ReceiptOverview");
            }
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offer();
            return View(new ReceiptProcessViewModel(geselecteerdeReceipt));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceiptEdit(ReceiptProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                Receipt receiptInDb = _receiptRepository.GetByReceiptId(model.ReceiptId);

                if (receiptInDb.Name != model.Name)
                {
                    receiptInDb.Name = model.Name;
                }

                if (receiptInDb.Description != model.Description)
                {
                    receiptInDb.Description = model.Description;
                }

                if (receiptInDb.Price != model.Price)
                {
                    receiptInDb.Price = model.Price;
                }


                if (receiptInDb.Category.Name != model.Category)
                {
                    receiptInDb.Category = _categoryRepository.GetByName(model.Category);
                }

                if (receiptInDb.Street != model.Street)
                {
                    receiptInDb.Street = model.Street;
                }

                if (receiptInDb.ApartmentNumber != model.ApartmentNumber)
                {
                    receiptInDb.ApartmentNumber = model.ApartmentNumber;
                }

                if (receiptInDb.Postcode != model.Postcode)
                {
                    receiptInDb.Postcode = model.Postcode;
                }

                if (receiptInDb.City != model.City)
                {
                    receiptInDb.City = model.City;
                }

                if (receiptInDb.Offer != model.Offer)
                {
                    receiptInDb.Offer = model.Offer;
                }

                _receiptRepository.SaveChanges();

                if (model.Thumbnail != null)
                {
                    var filePath = @"wwwroot/" + receiptInDb.Image + "thumb.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Thumbnail.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                if (model.Image != null)
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(@"wwwroot/" + receiptInDb.Image + "Image/");

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }

                    for (int i = 0; i < model.Image.Count; i++)
                    {
                        var filePath = @"wwwroot/" + receiptInDb.Image + "Image/" + (i + 1) + ".jpg";
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        var fileStream = new FileStream(filePath, FileMode.Create);
                        await model.Image[i].CopyToAsync(fileStream);
                        fileStream.Close();
                    }
                }

                return RedirectToAction("ReceiptOverview");
            }
            return View(nameof(ReceiptEdit), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReceipt(ReceiptProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                Receipt receiptInDb = _receiptRepository.GetByReceiptIdNotAccepted(model.ReceiptId);
                _receiptRepository.Remove(model.ReceiptId);
                _receiptRepository.SaveChanges();

                var filePath = @"wwwroot/" + receiptInDb.Image;
                Directory.Delete(filePath, true);


                var emailadres = receiptInDb.Seller.EmailAddress;

                var message = new MailMessage();
                message.From = new MailAddress("3bros.shop.suport@gmail.com");
                message.To.Add(emailadres);
                message.Subject = "Yêu cầu của bạn để thêm biên lai mới trên 3BrosShop đã bị từ chối.";

                if (model.Note != null)
                {
                    message.Body = String.Format("Kính gửi " + model.NameSeller + ", \n\n" +
                   "Yêu cầu gần đây của bạn để thêm biên nhận vào 3BrosShop đã bị từ chối. \n\n" +
                   model.Note + "\n\n" +
                   "Nếu bạn nghĩ rằng bạn vẫn có quyền thêm biên lai này vào 3BrosShop, chúng tôi khuyên bạn nên gửi yêu cầu mới. \n\n" +
                   "Trân trọng, \n" +
                  "3bros team");
                }
                else
                {
                    message.Body = String.Format("Kính gửi " + model.NameSeller + ", \n\n" +
                    "Yêu cầu gần đây của bạn để thêm biên nhận vào 3BrosShop đã bị từ chối. \n\n" +
                    "Nếu bạn nghĩ rằng bạn vẫn có quyền thêm biên lai này vào 3BrosShop, chúng tôi khuyên bạn nên gửi yêu cầu mới. \n\n" +
                    "Trân trọng, \n" +
                    "3bros team");
                }

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(message);

                return RedirectToAction("ReceiptRequests");
            }
            return View(nameof(SellerRequestEvaluation), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptReceiptRequest(ReceiptProcessViewModel model)
        {
            if (ModelState.IsValid)
            {
                Receipt receiptInDb = _receiptRepository.GetByReceiptIdNotAccepted(model.ReceiptId);

                if (receiptInDb.Name != model.Name)
                {
                    receiptInDb.Name = model.Name;
                }

                if (receiptInDb.Description != model.Description)
                {
                    receiptInDb.Description = model.Description;
                }

                if (receiptInDb.Price != model.Price)
                {
                    receiptInDb.Price = model.Price;
                }

                if (receiptInDb.Category.Name != model.Category)
                {
                    receiptInDb.Category = _categoryRepository.GetByName(model.Category);
                }

                if (receiptInDb.Street != model.Street)
                {
                    receiptInDb.Street = model.Street;
                }

                if (receiptInDb.ApartmentNumber != model.ApartmentNumber)
                {
                    receiptInDb.ApartmentNumber = model.ApartmentNumber;
                }

                if (receiptInDb.Postcode != model.Postcode)
                {
                    receiptInDb.Postcode = model.Postcode;
                }

                if (receiptInDb.City != model.City)
                {
                    receiptInDb.City = model.City;
                }

                if (receiptInDb.Offer != model.Offer)
                {
                    receiptInDb.Offer = model.Offer;
                }

                receiptInDb.Approved = true;

                _receiptRepository.SaveChanges();

                if (model.Thumbnail != null)
                {
                    var filePath = @"wwwroot/" + receiptInDb.Image + "thumb.jpg";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.Thumbnail.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                if (model.Image != null)
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(@"wwwroot/" + receiptInDb.Image + "Image/");

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }

                    for (int i = 0; i < model.Image.Count; i++)
                    {
                        var filePath = @"wwwroot/" + receiptInDb.Image + "Image/" + (i + 1) + ".jpg";
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        var fileStream = new FileStream(filePath, FileMode.Create);
                        await model.Image[i].CopyToAsync(fileStream);
                        fileStream.Close();
                    }
                }


                var emailadres = receiptInDb.Seller.EmailAddress;

                var message = new MailMessage();
                message.From = new MailAddress("3bros.shop.suport@gmail.com");
                message.To.Add(emailadres);
                message.Subject = "Yêu cầu của bạn để thêm biên lai mới trên 3BrosShop đã được chấp nhận!";

                if (model.Note != null)
                {
                    message.Body = String.Format("Kính gửi " + model.NameSeller + ", \n\n" +
                   "Yêu cầu của bạn để thêm biên lai mới trên 3BrosShop đã được chấp nhận. \n\n" +
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

                return RedirectToAction("ReceiptOverview");
            }
            return View(nameof(ReceiptRequest), model);
        }

        [HttpGet]
        public IActionResult LayoutSliderIndex()
        {
            return View(new ReceiptOverviewViewModel(_receiptRepository.GetCouponOfferSlider(_receiptRepository.GetAllApproved())));
        }

        [HttpGet]
        public IActionResult LayoutOffers()
        {
            return View(new ReceiptOverviewViewModel(_receiptRepository.GetReceiptOfferStandardAndSlider(_receiptRepository.GetAllApproved())));
        }

        [HttpGet]
        public IActionResult SoldReceiptView(int Id)
        {
            GeneratePDF(Id);
            return View(new SoldReceiptViewModel(_orderItemRepository.GetById(Id)));
        }

        [HttpGet]
        public IActionResult SoldReceipt()
        {
            return View(new OverviewSoldOrderViewModel(_orderItemRepository.getSoldOrder()));
        }

        [HttpGet]
        public IActionResult UsedReceipt()
        {
            return View(new OverviewUsedOrderViewModel(_orderItemRepository.getUsedOrder()));
        }

        public void GenerateQR(string qrcode)
        {
            var bonPath = @"wwwroot/images/temp/";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(bonPath + qrcode + ".png", ImageFormat.Png);
        }

        public void GeneratePDF(int Id)
        {
            var orderItem = _orderItemRepository.GetById(Id);
            var receipt = _receiptRepository.GetByReceiptId(orderItem.Receipt.ReceiptId);
            var seller = _sellerRepository.GetBySellerId(receipt.Seller.SellerId);
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
            //Paragraph naamBon = new Paragraph("Receipt: " + receipt.FirstName);

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
            Paragraph nameSeller = new Paragraph(receipt.Name, arial);
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