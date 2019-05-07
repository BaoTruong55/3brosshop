using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;

namespace Shop.Models.AdminViewModels
{
    public class OverviewSoldOrderListViewModel
    {
        public string Date { get; }
        public int Id { get; }
        public decimal Amount { get; }
        public string Name { get; }
        public string Status { get; }
        public string StatusClass { get; }

        public OverviewSoldOrderListViewModel(OrderItem bon)
        {
            Date = bon.CreationDate.ToString("dd/MM/yyyy");
            Amount = bon.Price;
            Id = bon.OrderItemId;
            Name = bon.Items.Name;
            Status = bon.Validity.ToString();

            switch (bon.Validity)
            {
                case Validity.Used:
                    StatusClass = "label-success";
                    break;
                case Validity.Valid:
                    StatusClass = "label-primary";
                    break;
                case Validity.Expired:
                    StatusClass = "label-danger";
                    break;
                default:
                    StatusClass = "label-primary";
                    break;
            }
        }

    }
}
