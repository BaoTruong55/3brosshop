using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.ShoppingCartViewModels
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập một địa chỉ email hợp lệ")]
        [Display(Name = "Địa chỉ email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên người nhận")]
        public string NameReciever { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail người nhận")]
        public string EmailReciever { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Tin nhắn cá nhân (tùy chọn)")]
        public string Message { get; set; }
    }
}