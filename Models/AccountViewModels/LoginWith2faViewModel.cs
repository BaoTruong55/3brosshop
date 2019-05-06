using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "{0} phải có ít nhất {2} và dài tối đa {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Mã xác thực")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Nhớ máy hiện tại")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
