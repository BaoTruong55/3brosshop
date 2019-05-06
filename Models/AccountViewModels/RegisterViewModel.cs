using Shop.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Enum;

namespace Shop.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [Display(Name = "Họ")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [Display(Name = "Tên")]
        [DataType(DataType.Text)]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [Display(Name = "Giới tính")]
        public Sex? Sex { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải nằm trong khoảng từ {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
