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
        private readonly IItemsRepository _itemsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISellerRepository _sellerRepository;
        public HomeController(IItemsRepository itemsRepository, ICategoryRepository categoryRepository, ISellerRepository sellerRepository)
        {
            _itemsRepository = itemsRepository;
            _categoryRepository = categoryRepository;
            _sellerRepository = sellerRepository;
        }

        public IActionResult Index()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            return View(new IndexViewModel(_itemsRepository.GetTop30(_itemsRepository.GetAllApproved().ToList()).ToList(), _itemsRepository.GetCouponOfferSlider(_itemsRepository.GetAllApproved().ToList()).ToList(), _categoryRepository.GetTop9WithAmount()));
        }

        public IActionResult Offers()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(_itemsRepository.GetItemsOfferStandardAndSlider(_itemsRepository.GetAllApproved().ToList()).Select(b => new SearchViewModel(b)).ToList());
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

        public IActionResult Search(string SearchType = null, string SearchKey = null, string Category = null, string Location = null, string MaxStartPrice = null )
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            IEnumerable<Items> filteredItems;


            if (!string.IsNullOrEmpty(SearchKey))

            {
                switch (SearchType)
                {
                    case "All":
                        filteredItems = _itemsRepository.GetAll(SearchKey, _itemsRepository.GetAllApproved());
                        break;
                    case "Location":
                        filteredItems = _itemsRepository.GetByLocation(SearchKey, _itemsRepository.GetAllApproved());
                        break;
                    case "FirstName":
                        filteredItems = _itemsRepository.GetByName(SearchKey, _itemsRepository.GetAllApproved());
                        break;
                        
                    default:
                        filteredItems = _itemsRepository.GetAllApproved();
                        break;
                }
                ViewData["SearchAssignment"] = SearchKey + " trong " + SearchType;

                if (!string.IsNullOrEmpty(Category) && Category != "*")
                {
                    string input = Category;
                    filteredItems = _itemsRepository.GetByCategory(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với thể loại " + input;
                }
                if (!string.IsNullOrEmpty(Location) && Location != "*")
                {
                    string input = Location;
                    filteredItems = _itemsRepository.GetByLocation(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với địa điểm " + input;
                }
                if (!string.IsNullOrEmpty(MaxStartPrice))
                {
                    int input = int.Parse(MaxStartPrice);
                    filteredItems = _itemsRepository.GetByPrice(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với giá tối đa €" + input;
                }
            }
            else
            {
                filteredItems = _itemsRepository.GetAllApproved();
                ViewData["SearchAssignment"] = "Tổng quan";

                if (!string.IsNullOrEmpty(Category) && Category != "*")
                {
                    string input = Category;
                    filteredItems = _itemsRepository.GetByCategory(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với thể loại " + input;
                }
                if (!string.IsNullOrEmpty(Location) && Location != "*")
                {
                    string input = Location;
                    filteredItems = _itemsRepository.GetByLocation(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với địa điểm " + input;
                }
                if (!string.IsNullOrEmpty(MaxStartPrice))
                {
                    int input = int.Parse(MaxStartPrice);
                    filteredItems = _itemsRepository.GetByPrice(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với giá tối đa " + input;
                }
            }

            return View(filteredItems.Select(b => new SearchViewModel(b)).ToList());
        }

        public IActionResult Detail(int Id)
        {
            Items clickedItems = _itemsRepository.GetByItemsId(Id);
            if (clickedItems == null)
            {
                clickedItems = _itemsRepository.GetByItemsIdNotAccepted(Id);
            }
            if (clickedItems == null)
            {
                return RedirectToAction("Index");
            }
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(new DetailViewModel(clickedItems));
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
