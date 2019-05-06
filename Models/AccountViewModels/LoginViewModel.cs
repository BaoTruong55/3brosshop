using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email của bạn.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu của bạn.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Giữ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}
