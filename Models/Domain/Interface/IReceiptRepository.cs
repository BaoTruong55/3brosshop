using System.Collections.Generic;

namespace Shop.Models.Domain.Interface
{
    public interface IReceiptRepository
    {
        IEnumerable<Receipt> GetAllApproved();
        int getNumberofReceiptRequests();
        IEnumerable<Receipt> GetAll();
        IEnumerable<Receipt> GetTop30(IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetCouponOfferSlider(IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetReceiptOfferStandard(IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetReceiptOfferStandardAndSlider(IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetAll(string searchKey, IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetByLocation(string searchKey, IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetByName(string searchKey, IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetByCategory(string searchKey, IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetByPrice(int searchKey, IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetReceiptNotyetApproved(IEnumerable<Receipt> inputList);
        IEnumerable<Receipt> GetReceiptApproved(IEnumerable<Receipt> inputList);
        Receipt GetByReceiptId(int receiptId);
        void Remove(int receiptId);
        Receipt GetByReceiptIdNotAccepted(int receiptId);
        void Add(Receipt receipt);
        void SaveChanges();
    }
}
