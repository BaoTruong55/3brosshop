using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models.Domain;

namespace Shop.Models.CheckoutViewModels
{
    public class OrderQrViewModel 
    {
        public OrderItem ItemQr { get; set; }
    }
}