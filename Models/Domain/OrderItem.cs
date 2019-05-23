using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shop.Models.Domain.Enum;

namespace Shop.Models.Domain
{
    public class OrderItem : ShoppingCartItem
    {
        public int OrderItemId { get; set; }
        public Validity Validity { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string QRCode { get; set; }
        public Seller Seller { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderAddress { get; set; }

        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public string RecipientAddress { get; set; }
        public string Message { get; set; }
    }
}
