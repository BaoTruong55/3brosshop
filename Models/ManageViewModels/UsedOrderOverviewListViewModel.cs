using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;

namespace Shop.Models.ManageViewModels
{
    public class UsedOrderOverviewListViewModel
    {
        
        public string Date { get; }
        public decimal Amount { get; }
        public string Name { get; }
        public Validity Validity { get; }
       
        public string QRCode { get; }

        public UsedOrderOverviewListViewModel()
        {

        }


        public UsedOrderOverviewListViewModel(OrderItem order)
        {
            Date = order.CreationDate.ToString("dd/MM/yyyy");
            Amount = order.Price;
            Name = (order.Items == null) ? "none" : order.Items.Name;
            Validity = order.Validity;
            QRCode = order.QRCode;
        }

        
    }
}
