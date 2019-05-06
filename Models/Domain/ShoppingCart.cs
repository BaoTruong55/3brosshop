using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShoppingCart
    {
        [JsonProperty]
        private readonly IList<ShoppingCartItem> _listShoppingCart = new List<ShoppingCartItem>();
        public IEnumerable<ShoppingCartItem> ShoppingCartItems => _listShoppingCart.AsEnumerable();
        public int NumberofVouchers => _listShoppingCart.Sum(l => l.Count);
        public bool IsEmpty => NumberofVouchers == 0;
        public decimal TotalValue => _listShoppingCart.Sum(l => l.Total);

        public void AddItem(Receipt receipt, int count, decimal price)
        {
            ShoppingCartItem item = searchShoppingCartItem(receipt.ReceiptId, price);
            if (item == null)
                _listShoppingCart.Add(new ShoppingCartItem() { Receipt = receipt, Count = count, Price = price });
            else
                item.Count += count;
        }

        public void DeleteItem(Receipt receipt, decimal price)
        {
            ShoppingCartItem cartItem = searchShoppingCartItem(receipt.ReceiptId, price);
            if (cartItem != null)
                _listShoppingCart.Remove(cartItem);
        }

        public void IncreaseNumber(int receiptId, decimal price)
        {
            ShoppingCartItem item = searchShoppingCartItem(receiptId, price);
            if (item != null)
                item.Count++;
        }

        public void DecreaseNumber(int receiptId, decimal price)
        {
            ShoppingCartItem cartItem = searchShoppingCartItem(receiptId, price);
            if (cartItem != null)
                cartItem.Count--;
            if (cartItem.Count <= 0)
                _listShoppingCart.Remove(cartItem);
        }

        public void MakeEmpty()
        {
            _listShoppingCart.Clear();
        }

        private ShoppingCartItem searchShoppingCartItem(int receiptId, decimal price)
        {
            return _listShoppingCart.SingleOrDefault(l => (l.Receipt.ReceiptId == receiptId) && (l.Price == price));
        }
    }
}
