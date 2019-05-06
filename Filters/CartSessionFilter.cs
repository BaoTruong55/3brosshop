using Shop.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Interface;

namespace Shop.Filters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class CartSessionFilter : ActionFilterAttribute
    {
        private readonly IReceiptRepository _receiptRepository;
        private ShoppingCart _shoppingCart;

        public CartSessionFilter(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _shoppingCart = ReadCartFromSession(context.HttpContext);
            context.ActionArguments["shoppingCart"] = _shoppingCart;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            WriteCartToSession(_shoppingCart, context.HttpContext);
            base.OnActionExecuted(context);
        }

        private ShoppingCart ReadCartFromSession(HttpContext context)
        {
            ShoppingCart shoppingCart = context.Session.GetString("shoppingCart") == null ?
                new ShoppingCart() : JsonConvert.DeserializeObject<ShoppingCart>(context.Session.GetString("shoppingCart"));
            foreach (var l in shoppingCart.ShoppingCartItems)
                l.Receipt = _receiptRepository.GetByReceiptId(l.Receipt.ReceiptId);
            return shoppingCart;
        }

        private void WriteCartToSession(ShoppingCart shoppingCart, HttpContext context)
        {
            context.Session.SetString("shoppingCart", JsonConvert.SerializeObject(shoppingCart));
        }
    }
}
