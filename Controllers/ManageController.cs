using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shop.Models;
using Shop.Models.ManageViewModels;
using Shop.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Mail;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;
using Shop.Models.Domain.Enum;
using Shop.Models.Domain.Interface;
using Shop.Services;

namespace Shop.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IItemsRepository _itemsRepository;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder,
          ICategoryRepository categoryRepository,
          IUserRepository userRepository,
          IOrderRepository orderRepository,
          IOrderItemRepository orderItemRepository,
          ISellerRepository sellerRepository,
          IItemsRepository itemsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _sellerRepository = sellerRepository;
            _itemsRepository = itemsRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["Sex"] = Sex();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var _user = _userRepository.GetBy(user.Email);

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = _user.FirstName,
                FamilyName = _user.FamilyName,
                Sex = _user.Sex,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> IndexSeller()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var seller = _sellerRepository.GetByEmail(user.Email);

            var model = new IndexSellerViewModel
            {
                Name = seller.Name,
                Email = user.Email,
                Description = seller.Description,
                Street = seller.Street,
                City = seller.City,
                Postcode = seller.Postcode,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UsedOrderItemOverview(int Id)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await _userManager.GetUserAsync(User);
            var seller = _sellerRepository.GetByEmail(user.Email);

            var orderItemList = _orderItemRepository.getUsedItemsFromSellerId(seller.SellerId);

            return View(new UsedOrderOverviewViewModel(orderItemList));
        }


        [HttpGet]
        public IActionResult ItemsRequest()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemsRequest(ItemsAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
                }
                var seller = _sellerRepository.GetByEmail(user.Email);
                Items newItems = new Items(model.Name, model.Price, model.Description, 0, @"temp", _categoryRepository.GetByName(model.Category), model.Street, model.ApartmentNumber, model.Postcode, model.City, _sellerRepository.GetBySellerId(seller.SellerId), Offer.No, false);
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
                ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
                ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
                return RedirectToAction(nameof(HomeController.Index), "Home");

            }
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            return View(nameof(ItemsRequest), model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var _user = _userRepository.GetBy(user.Email);

            var email = user.Email;

            var firstName = _user.FirstName;
            if (model.FirstName != firstName)
            {
                _user.FirstName = model.FirstName;
                _userRepository.SaveChanges();
            }

            var familyName = _user.FamilyName;
            if (model.FamilyName != familyName)
            {
                _user.FamilyName = model.FamilyName;
                _userRepository.SaveChanges();
            }

            var phoneNumber = _user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                _user.PhoneNumber = model.PhoneNumber;
                _userRepository.SaveChanges();
            }

            var address = _user.Address;
            if (model.Address != address)
            {
                _user.Address = model.Address;
                _userRepository.SaveChanges();
            }

            var _sex = _user.Sex;
            if (model.Sex != _sex)
            {
                _user.Sex = model.Sex;
                _userRepository.SaveChanges();
            }

            StatusMessage = "Dữ liệu của bạn đã được cập nhật thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexSeller(IndexSellerViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var seller = _sellerRepository.GetByEmail(user.Email);

            var description = seller.Description;
            if (model.Description != description)
            {
                seller.Description = model.Description;
                _sellerRepository.SaveChanges();
            }


            var street = seller.Street;
            if (model.Street != street)
            {
                seller.Street = model.Street;
                _sellerRepository.SaveChanges();
            }

            var city = seller.City;
            if (model.City != city)
            {
                seller.City = model.City;
                _sellerRepository.SaveChanges();
            }

            var postcode = seller.Postcode;
            if (model.Postcode != postcode)
            {
                seller.Postcode = model.Postcode;
                _sellerRepository.SaveChanges();
            }

            var phoneNumber = seller.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                seller.PhoneNumber = model.PhoneNumber;
                _sellerRepository.SaveChanges();
            }
            

            StatusMessage = "Dữ liệu của bạn đã được cập nhật thành công.";
            return RedirectToAction(nameof(IndexSeller));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            var email = user.Email;

            var _user = _userRepository.GetBy(email);

            await _emailSender.SendEmailConfirmationAsync(email, callbackUrl, _user.FirstName);

            StatusMessage = "Một email xác nhận mới đã được gửi đến địa chỉ email của bạn.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            if (claims.Any(c => c.Value == "seller"))
            {
                var wachtwoord = model.NewPassword;

                var handelaar = _sellerRepository.GetByEmail(user.Email);

                _sellerRepository.SaveChanges();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("Người dùng đã thay đổi mật khẩu thành công.");
            StatusMessage = "Mật khẩu của bạn đã được thay đổi thành công.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Mật khẩu của bạn đã được đặt lại.";

            return RedirectToAction(nameof(SetPassword));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var model = new ExternalLoginsViewModel { CurrentLogins = await _userManager.GetLoginsAsync(user) };
            model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
            model.StatusMessage = StatusMessage;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
            {
                throw new ApplicationException($"Xảy ra lỗi không mong muốn khi tải thông tin đăng nhập bên ngoài cho người dùng  '{user.Id}'.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Xảy ra lỗi không mong muốn khi thêm đăng nhập bên ngoài cho người dùng  '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "Đăng nhập bên ngoài đã được thêm thành công.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Không thể lấy thông tin người dùng '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Xảy ra lỗi không mong muốn khi xóa đăng nhập bên ngoài cho người dùng '{user.Id}'.");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Đăng nhập bên ngoài đã được xóa thành công.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> PersonalOrder()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["Order"] = null;
            var user = await _userManager.GetUserAsync(User);
            var _user = _userRepository.GetBy(user.Email);
            ICollection<Order> orders = new HashSet<Order>();
            if (_user.Bills.Count != 0 && _user.Bills != null)
            {
                foreach (Order b in _user.Bills)
                {
                    var Order = _orderRepository.GetBy(b.BillId);
                    if (Order.Orders.All(bl => bl.Validity != Validity.Invalid))
                        orders.Add(Order);
                }
                ViewData["Order"] = orders;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DetailOrder(int id)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["OrderItem"] = null;

            var user = await _userManager.GetUserAsync(User);
            var _user = _userRepository.GetBy(user.Email);

            if (_orderRepository.GetBy(id) != null)
            {
                var Order = _orderRepository.GetBy(id);
                var _user2 = _userRepository.GetByOrderId(id);

                if (_user == _user2)
                {
                    ICollection<OrderItem> orderItems = new HashSet<OrderItem>();

                    //maak vervallen bonnen vervallen (visueel)
                    foreach (OrderItem bon in Order.Orders.Where(bl => bl.Validity == Validity.Valid && DateTime.Today > bl.CreationDate.AddYears(1)))
                    {
                        bon.Validity = Validity.Expired;
                    }
                    _orderItemRepository.SaveChanges();

                    //toon bonnen in Order en maak bijhorende pdf's
                    foreach (OrderItem bl in Order.Orders)
                    {
                        GeneratePDF(bl.OrderItemId);
                        orderItems.Add(_orderItemRepository.GetById(bl.OrderItemId));
                    }
                    ViewData["OrderItem"] = orderItems;
                    ViewData["OrderId"] = Order.BillId;
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ItemsUsed(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var orderItem = _orderItemRepository.GetBy(id);
            orderItem.Validity = Validity.Used;
            _orderItemRepository.SaveChanges();
            return  RedirectToAction("UsedOrderOverview");
        }


        [HttpGet]
        public async Task<IActionResult> ItemsOverview()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            var user = await _userManager.GetUserAsync(User);
            var seller = _sellerRepository.GetByEmail(user.Email);
            return View(new ItemsOverviewViewModel(_itemsRepository.GetAllApproved().Where(item=>item.Seller.SellerId == seller.SellerId)));
        }

        [HttpGet]
        public async Task<IActionResult> ItemsDelete(string Id)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            var user = await _userManager.GetUserAsync(User);
            var seller = _sellerRepository.GetByEmail(user.Email);
            if (user == null)
                return RedirectToAction("Index","Home");
            var itemsList = _itemsRepository.GetAllApproved().Where(item => item.Seller.SellerId == seller.SellerId && item.ItemsId == Convert.ToInt32(Id)).ToList();
            
            if (itemsList.Count > 0)
            {
                var items = itemsList[0];
                _itemsRepository.Remove(items.ItemsId);
                _itemsRepository.SaveChanges();
            }
            return RedirectToAction("ItemsOverview");
        }

        private SelectList Offers()
        {
            var offers = new List<Offer>();
            foreach (Offer offerItem in Enum.GetValues(typeof(Offer)))
            {
                offers.Add(offerItem);
            }
            return new SelectList(offers);
        }


        [HttpGet]
        public IActionResult ItemsEdit(int Id)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            Items geselecteerdeItems = _itemsRepository.GetByItemsId(Id);
            if (geselecteerdeItems == null)
            {
                return RedirectToAction("ItemsOverview");
            }
            ViewData["category"] = new SelectList(_categoryRepository.GetAll().Select(c => c.Name));
            ViewData["offer"] = Offers();
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


        public void GenerateQR(string qrcode)
        {
            var itemsPath = @"wwwroot/images/temp/";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://" + Request.Host + "/checkout/OrderQr?Id=" + qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(itemsPath + qrcode + ".png", ImageFormat.Png);
        }

        public void GeneratePDF(int Id)
        {
            var orderItem = _orderItemRepository.GetById(Id);
            var items = _itemsRepository.GetByItemsId(orderItem.Items.ItemsId);
            var seller = _sellerRepository.GetBySellerId(items.Seller.SellerId);
            var user = _userManager.GetUserAsync(User);
            var _user = _userRepository.GetBy(user.Result.Email);

            ViewData["path"] = @"/pdf/c_" + orderItem.QRCode + ".pdf";

            string value = String.Format(orderItem.Price.ToString() + " vnđ");
            string date = orderItem.CreationDate.AddYears(1).ToString("dd/MM/yyyy");
            string valid = String.Format("Hiệu lực: " + date);
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
            Paragraph givenBy = new Paragraph("mua boi: " + _user.FirstName, arial18);
            Paragraph _valid = new Paragraph(valid, arial18);

            amount.Alignment = Element.ALIGN_LEFT;

            nameSeller.Alignment = Element.ALIGN_LEFT;
            givenBy.Alignment = Element.ALIGN_LEFT;
            _valid.Alignment = Element.ALIGN_LEFT;

            Phrase qrCodeString = new Phrase(orderItem.QRCode, arialSmall);

            PdfWriter writer = PdfWriter.GetInstance(pdf, new FileStream(@"wwwroot/pdf/c_" + orderItem.QRCode + ".pdf", FileMode.Create));
            pdf.Open();

            ColumnText.ShowTextAligned(writer.DirectContent,
            Element.ALIGN_MIDDLE, qrCodeString, 195, 4, 0);

            pdf.Add(logoLL);
            pdf.Add(logoSeller);
            pdf.Add(nameSeller);
            pdf.Add(amount);
            pdf.Add(givenBy);
            pdf.Add(_valid);
            pdf.Add(jpg);
            pdf.Add(kado);
            pdf.Close();

            System.IO.File.Delete(imageURL);
        }
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        #endregion
    }
}
