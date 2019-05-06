using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;

namespace Shop.Models.AdminViewModels
{
    public class SoldReceiptViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Ngày tạo")]
        public string CreationDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên trên hóa đơn")]
        public string ReceiptName { get; set; }

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




        public SoldReceiptViewModel(OrderItem bon)
        {
            Id = bon.OrderItemId ;
            Name = bon.Seller.Name;
            ReceiptName = bon.Receipt.Name;
            Price = bon.Price;
            CreationDate = bon.CreationDate.ToString("dd/MM/yyyy");
            Status = bon.Validity.ToString();
            UseDate = bon.ExpirationDate.ToString("dd/MM/yyyy");
            StationName = bon.SenderName;
            NameReciever = bon.RecipientName;
            EmailReciever = bon.RecipientEmail;
            EmailSender = bon.SenderEmail;

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

        public SoldReceiptViewModel()
        {

        }

    }
}
