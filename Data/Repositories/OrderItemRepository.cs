using Shop.Models.Domain;
using Shop.Models.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain.Interface;

namespace Shop.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {

        private readonly DbSet<OrderItem> _orderItems;
        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _orderItems = dbContext.OrderItem;
        }

        public void Add(OrderItem orderItem)
        {
            _orderItems.Add(orderItem);
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return _orderItems.Select(x => new OrderItem()
            {
                Validity = x.Validity,
                CreationDate = x.CreationDate,
                ExpirationDate = x.ExpirationDate,
                QRCode = x.QRCode,
                Seller = x.Seller,
                SenderName = x.SenderName,
                SenderEmail = x.SenderEmail,
                RecipientName = x.RecipientName,
                RecipientEmail = x.RecipientEmail,
                Message = x.Message,
                Items = x.Items,
                Price = x.Price,
                Count = x.Count
            }).ToList();
        }


        public OrderItem GetBy(string qrcode)
        {
            return _orderItems.Select(x => new OrderItem()
            {
                Validity = x.Validity,
                CreationDate = x.CreationDate,
                ExpirationDate = x.ExpirationDate,
                QRCode = x.QRCode,
                Seller = x.Seller,
                SenderName = x.SenderName,
                SenderEmail = x.SenderEmail,
                SenderPhoneNumber = x.SenderPhoneNumber,
                SenderAddress = x.SenderAddress,
                RecipientName = x.RecipientName,
                RecipientEmail = x.RecipientEmail,
                RecipientPhoneNumber = x.RecipientPhoneNumber,
                RecipientAddress = x.RecipientAddress,
                Message = x.Message,
                Items = x.Items,
                Price = x.Price,
                Count = x.Count
            }).Include(b => b.Items).SingleOrDefault(g => g.QRCode == qrcode);
        }

        public OrderItem GetById(int orderItemId)
        {
            return _orderItems.Include(b => b.Items).Include(b => b.Seller).SingleOrDefault(g => g.OrderItemId == orderItemId);
        }

        public IEnumerable<OrderItem> getUsedOrder()
        {
            return _orderItems.Where(b => b.Validity == Validity.Used).Include(b => b.Items);
        }

        public IEnumerable<OrderItem> getUsedThisMonth()
        {
            DateTime date = DateTime.Now.Date;
            date = date.AddMonths(-1);
            return _orderItems.Where(b => (b.ExpirationDate >= date) && (b.Validity == Validity.Used));
        }

        public IEnumerable<OrderItem> getSoldOrder()
        {
            MakeExpiredItemsExpired();
            return _orderItems.Where(b => b.Validity != Validity.Invalid).Include(b => b.Items);
        }

        public IEnumerable<OrderItem> getSoldThisMonth()
        {
            DateTime date = DateTime.Now.Date;
            date = date.AddMonths(-1);
            return _orderItems.Where(b => (b.CreationDate >= date) && (b.Validity != Validity.Invalid)).Include(b => b.Items);
        }

        public void MakeExpiredItemsExpired()
        {
            foreach (OrderItem bon in _orderItems.Where(bl => bl.Validity == Validity.Valid && DateTime.Today > bl.CreationDate.AddYears(1)))
            {
                bon.Validity = Validity.Expired;
            }
            SaveChanges();
        }

        
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }



        public IEnumerable<OrderItem> getUsedItemsFromSellerId(int id)
        {
            
            return _orderItems.Select(x=>new OrderItem()
            {
                Validity = x.Validity,
                CreationDate = x.CreationDate,
                ExpirationDate = x.ExpirationDate,
                QRCode = x.QRCode,
                Seller = x.Seller,
                SenderName = x.SenderName,
                SenderEmail = x.SenderEmail,
                RecipientName = x.RecipientName,
                RecipientEmail = x.RecipientEmail,
                Message = x.Message,
                Items = x.Items,
                Price = x.Price,
                Count = x.Count
            }).Where(x=>x.Seller.SellerId == id).ToList();
        }
    }
}
