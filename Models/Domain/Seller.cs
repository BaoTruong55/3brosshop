using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    public class Seller
    {
        public int SellerId { get; private set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public ICollection<Receipt> Receipt { get; private set; }
        public string Street { get; set; }
        public string ApartmentNumber { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public bool Approved { get; set; }

        protected Seller()
        {

        }

        public Seller(string name, string emailAddress, string description, string street, string apartmentNumber, string postcode, string city, bool approved = false)
        {
            Approved = approved;
            Name = name;
            EmailAddress = emailAddress;
            Description = description;
            Street = street;
            ApartmentNumber = apartmentNumber;
            Postcode = postcode;
            City = city;
            Receipt = new HashSet<Receipt>();
        }

        public void AddReceipt(Receipt receipt)
        {
            Receipt.Add(receipt);
        }

        public string GetLogoPath()
        {
            return @"/images/seller/" + SellerId + "/logo.jpg";
        }

    }
}
