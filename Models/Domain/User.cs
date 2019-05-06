using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Enum;

namespace Shop.Models.Domain
{
    public class User
    {
        public int UserId { get; private set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public Sex Sex { get; set; }
        public string EmailAddress { get; set; }
        public ICollection<Order> Bills { get; set; }

        private string _image;
        public string Image
        {
            get
            {
                if (_image == null)
                    return @"\images\users\default.png";
                else
                    return _image;
            }
            set { _image = value; }
        }


        public User()
        {
            Bills = new List<Order>();
        }

        public void PlaceOrder(ShoppingCart shoppingCart)
        {
            Bills.Add(new Order(shoppingCart));
        }

        public string FullName()
        {
            return FirstName + " " + FamilyName;
        }
    }
}