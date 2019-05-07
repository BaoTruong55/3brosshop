using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;

namespace Shop.Models.AdminViewModels
{
    public class SoldItemsViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Ngày tạo")]
        public string CreationDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên mặt hàng")]
        public string ItemsName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Ngày sử dụng")]
        public string UseDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Hiệu lực")]
        public string Status { get; set; }

        public string StatusClass { get; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string StationName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên người nhận")]
        public string NameReciever { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Email người nhận")]
        public string EmailReciever { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Email người gửi")]
        public string EmailSender { get; set; }




        public SoldItemsViewModel(OrderItem order)
        {
            Id = order.OrderItemId ;
            Name = order.Seller.Name;
            ItemsName = order.Items.Name;
            Price = order.Price;
            CreationDate = order.CreationDate.ToString("dd/MM/yyyy");
            Status = order.Validity.ToString();
            UseDate = order.ExpirationDate.ToString("dd/MM/yyyy");
            StationName = order.SenderName;
            NameReciever = order.RecipientName;
            EmailReciever = order.RecipientEmail;
            EmailSender = order.SenderEmail;

            switch (order.Validity)
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

        public SoldItemsViewModel()
        {

        }

    }
}
