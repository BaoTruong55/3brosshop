using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Shop.Models.HomeViewModels;
using System.Net.Mail;
using Shop.Models.Domain.Interface;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISellerRepository _sellerRepository;
        public HomeController(IReceiptRepository receiptRepository, ICategoryRepository categoryRepository, ISellerRepository sellerRepository)
        {
            _receiptRepository = receiptRepository;
            _categoryRepository = categoryRepository;
            _sellerRepository = sellerRepository;
        }

        public IActionResult Index()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            return View(new IndexViewModel(_receiptRepository.GetTop30(_receiptRepository.GetAllApproved().ToList()).ToList(), _receiptRepository.GetCouponOfferSlider(_receiptRepository.GetAllApproved().ToList()).ToList(), _categoryRepository.GetTop9WithAmount()));
        }

        public IActionResult Offers()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(_receiptRepository.GetReceiptOfferStandardAndSlider(_receiptRepository.GetAllApproved().ToList()).Select(b => new SearchViewModel(b)).ToList());
        }

        public IActionResult About()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }

        public IActionResult Error()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string SearchType = null, string SearchKey = null, string Categorie = null, string Location = null, string MaxStartPrice = null )
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            IEnumerable<Receipt> filteredReceipt;


            if (!string.IsNullOrEmpty(SearchKey))

            {
                switch (SearchType)
                {
                    case "Everything":
                        filteredReceipt = _receiptRepository.GetAll(SearchKey, _receiptRepository.GetAllApproved());
                        break;
                    case "Location":
                        filteredReceipt = _receiptRepository.GetByLocation(SearchKey, _receiptRepository.GetAllApproved());
                        break;
                    case "FirstName":
                        filteredReceipt = _receiptRepository.GetByName(SearchKey, _receiptRepository.GetAllApproved());
                        break;
                    case "Category":
                        filteredReceipt = _receiptRepository.GetByCategory(SearchKey, _receiptRepository.GetAllApproved());
                        break;
                    default:
                        filteredReceipt = _receiptRepository.GetAllApproved();
                        break;
                }
                ViewData["SearchAssignment"] = SearchKey + " trong " + SearchType;

                if (!string.IsNullOrEmpty(Categorie) && Categorie != "*")
                {
                    string input = Categorie;
                    filteredReceipt = _receiptRepository.GetByCategory(input, filteredReceipt);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với thể loại " + input;
                }
                if (!string.IsNullOrEmpty(Location) && Location != "*")
                {
                    string input = Location;
                    filteredReceipt = _receiptRepository.GetByLocation(input, filteredReceipt);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với địa điểm " + input;
                }
                if (!string.IsNullOrEmpty(MaxStartPrice))
                {
                    int input = int.Parse(MaxStartPrice);
                    filteredReceipt = _receiptRepository.GetByPrice(input, filteredReceipt);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với giá tối đa €" + input;
                }
            }
            else
            {
                filteredReceipt = _receiptRepository.GetAllApproved();
                ViewData["SearchAssignment"] = "Tổng quan";

                if (!string.IsNullOrEmpty(Categorie) && Categorie != "*")
                {
                    string input = Categorie;
                    filteredReceipt = _receiptRepository.GetByCategory(input, filteredReceipt);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với thể loại " + input;
                }
                if (!string.IsNullOrEmpty(Location) && Location != "*")
                {
                    string input = Location;
                    filteredReceipt = _receiptRepository.GetByLocation(input, filteredReceipt);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với địa điểm " + input;
                }
                if (!string.IsNullOrEmpty(MaxStartPrice))
                {
                    int input = int.Parse(MaxStartPrice);
                    filteredReceipt = _receiptRepository.GetByPrice(input, filteredReceipt);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với giá tối đa " + input;
                }
            }

            return View(filteredReceipt.Select(b => new SearchViewModel(b)).ToList());
        }

        public IActionResult Detail(int Id)
        {
            Receipt clickedReceipt = _receiptRepository.GetByReceiptId(Id);
            if (clickedReceipt == null)
            {
                clickedReceipt = _receiptRepository.GetByReceiptIdNotAccepted(Id);
            }
            if (clickedReceipt == null)
            {
                return RedirectToAction("Index");
            }
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(new DetailViewModel(clickedReceipt));
        }

        [HttpPost]
        public IActionResult SendEmail(SendEmailViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (ModelState.IsValid)
            {
                var message = new MailMessage();
                message.From = new MailAddress("3bros.shop.suport@gmail.com");
                message.To.Add("3bros.shop.suport@gmail.com");
                message.Subject = "tin nhắn từ liên hệ";
                message.Body = String.Format("Tên: {0}\n" +
                                        "Địa chỉ Email: {1}\n" +
                                        "Tiêu đề: {2}\n" +
                                        "Tin nhắn: {3}\n",
                                        model.Name, model.Email, model.Subject, model.Message);
                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(message);

                return RedirectToAction("Index");
            }
            return View(nameof(About), model);
        }

        [HttpGet]
        public IActionResult UpdateShoppingCartCount()
        {
            return PartialView("shoppingCartCountPartial");
        }
    }
}
