using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Enum;

namespace Shop.Models.Domain
{
    public class Order
    {
        [Key]
        public int BillId { get; private set; }
        public DateTime BillDate { get; set; }
        public ICollection<OrderItem> Orders { get; }
        public decimal BillTotal => Orders.Sum(b => b.Total);

        protected Order()
        {
            Orders = new HashSet<OrderItem>();
            BillDate = DateTime.Today;
        }

        public Order(ShoppingCart shoppingCart) : this()
        {
            if (!shoppingCart.ShoppingCartItems.Any())
                throw new InvalidOperationException("Vui lòng thêm một hoặc nhiều phiếu quà tặng vào giỏ hàng của bạn trước khi đặt hàng.");

            foreach (ShoppingCartItem cart in shoppingCart.ShoppingCartItems)
            {
                for (int i = 1; i <= cart.Count; i++)
                {
                    cart.Receipt.QuantityOrdered++;
                    string qrcode = String.Format(Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddhhmmssffffff"));
                    Orders.Add(new OrderItem
                    {
                        Receipt = cart.Receipt,
                        Count = 1,
                        Price = cart.Price,
                        Validity = Validity.Invalid,
                        CreationDate = DateTime.Today,
                        Seller = cart.Receipt.Seller,
                        QRCode = qrcode
                    });
                }
            }
        }

        public bool HasOrdered(Receipt receipt) => Orders.Any(b => b.Receipt.Equals(receipt));

}
}
