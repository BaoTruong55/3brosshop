using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain.Interface
{
    public interface ISellerRepository
    {
        IEnumerable<Seller> GetAll();

        IEnumerable<Seller> GetSellerYetNotApproved(IEnumerable<Seller> inputList);

        IEnumerable<Seller> GetSellerApproved(IEnumerable<Seller> inputList);

        void Add(Seller seller);

        void Remove(int sellerId);

        void SaveChanges();

        int getNumberOfSeller();

        Seller GetBySellerId(int sellerId);

        Seller GetByEmail(string email);

        Seller GetBySellerIdNotAccepted(int sellerId);
    }
}
