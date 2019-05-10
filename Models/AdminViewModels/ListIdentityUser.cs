using System.Collections.Generic;

namespace Shop.Models.AdminViewModels
{
    public class ListIdentityUser
    {
        public List<UsersViewModel> ListUser { get; set; } = new List<UsersViewModel>();
    }
}