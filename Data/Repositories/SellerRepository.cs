using Shop.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Interface;

namespace Shop.Data.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Seller> _sellers;
        public SellerRepository(ApplicationDbContext context)
        {
            _dbContext = context;
            _sellers = context.Seller;
        }

        public void Add(Seller seller)
        {
            _sellers.Add(seller);
        }

        public int getNumberOfSeller()
        {
            return _sellers.Count(h => !h.Approved);
        }

        public IEnumerable<Seller> GetAll()
        {
            return _sellers.Include(h => h.Items).AsNoTracking().ToList();
        }

        public Seller GetByEmail(string email)
        {
            return _sellers.SingleOrDefault(h => h.EmailAddress == email);
        }

        public Seller GetBySellerId(int sellerId)
        {
            return _sellers.SingleOrDefault(h => h.SellerId == sellerId && h.Approved);
        }

        public Seller GetBySellerIdNotAccepted(int sellerId)
        {
            return _sellers.SingleOrDefault(h => h.SellerId == sellerId && !h.Approved);
        }

        public IEnumerable<Seller> GetSellerApproved(IEnumerable<Seller> inputList)
        {
            return inputList.OrderBy(h => h.SellerId).Where(h => h.Approved).ToList();
        }

        public IEnumerable<Seller> GetSellerYetNotApproved(IEnumerable<Seller> inputList)
        {
            return inputList.OrderBy(h => h.SellerId).Where(h => !h.Approved).ToList();
        }

        public void Remove(int sellerId)
        {
            Seller tempSeller = GetBySellerIdNotAccepted(sellerId);
            if (tempSeller == null)
            {
                tempSeller = GetBySellerId(sellerId);
            }
            _sellers.Remove(tempSeller);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
