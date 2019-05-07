using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.ShoppingCartViewModels;
using Shop.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Shop.Filters;
using Shop.Models.Domain.Interface;

namespace Shop.Controllers
{
    [ServiceFilter(typeof(CartSessionFilter))]
    public class ShoppingCartController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemsRepository _itemsRepository;

        public ShoppingCartController(ICategoryRepository categoryRepository, IItemsRepository itemsRepository)
        {
            _categoryRepository = categoryRepository;
            _itemsRepository = itemsRepository;
        }

        public IActionResult Index(ShoppingCart shoppingCart)
        {
            ViewData["Navbar"] = "None";
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(new IndexViewModel(shoppingCart.ShoppingCartItems, shoppingCart.TotalValue));
        }

        [HttpGet]
        public IActionResult Add(int id, decimal price, int number, ShoppingCart shoppingCart)
        {
            Items items = _itemsRepository.GetByItemsId(id);
            if (items != null)
            {
                shoppingCart.AddItem(items, number, price);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Remove(int id, decimal price, ShoppingCart shoppingCart)
        {
            Items items = _itemsRepository.GetByItemsId(id);
            shoppingCart.DeleteItem(items, price);
            TempData["message"] = $"Mặt hàng {items.Name} với số tiền {price} vnđ đã bị xóa khỏi giỏ hàng của bạn.";
            return PartialView("IndexPartialItemsList", new IndexViewModel(shoppingCart.ShoppingCartItems, shoppingCart.TotalValue));
        }

        [HttpGet]
        public IActionResult Plus(int id, decimal price, ShoppingCart shoppingCart)
        {
            shoppingCart.IncreaseNumber(id, price);
            return PartialView("IndexPartialItemsList", new IndexViewModel(shoppingCart.ShoppingCartItems, shoppingCart.TotalValue));
        }

        [HttpGet]
        public IActionResult Min(int id, decimal price, ShoppingCart shoppingCart)
        {
            shoppingCart.DecreaseNumber(id, price);
            return PartialView("IndexPartialItemsList", new IndexViewModel(shoppingCart.ShoppingCartItems, shoppingCart.TotalValue));
        }

        public IActionResult Checkout(ShoppingCart shoppingCart)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["Total"] = shoppingCart.TotalValue;
            ViewData["Count"] = shoppingCart.NumberofVouchers;
            ViewData["ReturnUrl"] = "/ShoppingCart/Checkout";
            return View();
        }

    }
}
