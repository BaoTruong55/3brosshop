using Shop.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Enum;

namespace Shop.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Họ")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} * bắt buộc.")]
        [DataType(DataType.Text)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Giới tính")]
        public Sex Sex { get; set; }

        public string StatusMessage { get; set; }
    }
}
