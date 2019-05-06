using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Models.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Shop.Models.Domain.Interface;

namespace Shop.Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Receipt> _receipts;
        public ReceiptRepository(ApplicationDbContext context)
        {
            _context = context;
            _receipts = context.Receipt;
        }
        public IEnumerable<Receipt> GetAllApproved()
        {
            return GetReceiptApproved(_receipts.Include(b => b.Category).Include(b => b.Seller).Where(b => b.Approved).OrderByDescending(b => b.QuantityOrdered).AsNoTracking().ToList());
        }

        public int getNumberofReceiptRequests()
        {
            return _receipts.Count(b => !b.Approved);
        }

        public IEnumerable<Receipt> GetAll(string searchKey, IEnumerable<Receipt> inputList)
        {
            if (searchKey.Trim().Length != 0)
            {
                string[] _words = searchKey.ToLower().Split(' ');
                int _numberofWords = _words.Length;
                List<Receipt> _advancedSearch = new List<Receipt>();


                foreach (Receipt b in inputList)
                {
                    int _numberofMatchesWord = 0;
                    foreach (String word in _words)
                    {
                        String wordFilter = word.Replace("-", "");
                        wordFilter = wordFilter.Replace("_", "");
                        wordFilter = RemoveAccents(wordFilter);

                        bool matchFound = false;
                        foreach (String woordToMatch in b.Category.Name.ToLower().Split(' '))
                        {
                            String woordfilterMatch = woordToMatch.Replace("-", "");
                            woordfilterMatch = woordfilterMatch.Replace("_", "");
                            woordfilterMatch = RemoveAccents(woordfilterMatch);
                            if (woordfilterMatch.Contains(wordFilter))
                            {
                                matchFound = true;
                                _numberofMatchesWord++;
                                break;
                            }
                        }
                        if (matchFound == false)
                        {
                            foreach (String woordToMatch in b.City.ToLower().Split(' '))
                            {
                                String woordfilterMatch = woordToMatch.Replace("-", "");
                                woordfilterMatch = woordfilterMatch.Replace("_", "");
                                woordfilterMatch = RemoveAccents(woordfilterMatch);
                                if (woordfilterMatch.Contains(wordFilter))
                                {
                                    matchFound = true;
                                    _numberofMatchesWord++;
                                    break;
                                }
                            }
                        }
                        if (matchFound == false)
                        {
                            foreach (String woordToMatch in b.Name.ToLower().Split(' '))
                            {
                                String woordfilterMatch = woordToMatch.Replace("-", "");
                                woordfilterMatch = woordfilterMatch.Replace("_", "");
                                woordfilterMatch = RemoveAccents(woordfilterMatch);
                                if (woordfilterMatch.Contains(wordFilter))
                                {
                                    matchFound = true;
                                    _numberofMatchesWord++;
                                    break;
                                }
                            }
                        }
                        
                        
                    }
                    if (_numberofMatchesWord == _numberofWords)
                    {
                        _advancedSearch.Add(b);
                    }
                }

                return _advancedSearch.ToList();
            }
            else
            {
                return GetAllApproved().ToList();
            }
            
        }

        public IEnumerable<Receipt> GetByCategory(string searchKey, IEnumerable<Receipt> inputList)
        {
            string _searchKey = searchKey.ToLower();
            return inputList.Where(b => b.Category.Name.ToLower().Contains(_searchKey)).ToList();
        }

        public IEnumerable<Receipt> GetByLocation(string searchKey, IEnumerable<Receipt> inputList)
        {
            string _searchKey = searchKey.ToLower();
            _searchKey = _searchKey.Replace("-", "");
            _searchKey = _searchKey.Replace("_", "");
            _searchKey = RemoveAccents(searchKey);
            return inputList.Where(b => RemoveAccents(b.City.ToLower().Replace("-", "").Replace("_", "")).Contains(_searchKey)).ToList();
        }

        public IEnumerable<Receipt> GetByName(string searchKey, IEnumerable<Receipt> inputList)
        {
            string _searchKey = searchKey.ToLower();
            _searchKey = _searchKey.Replace("-", "");
            _searchKey = _searchKey.Replace("_", "");
            _searchKey = RemoveAccents(searchKey);
            return inputList.Where(b => RemoveAccents(b.Name.ToLower().Replace("-", "").Replace("_", "")).Contains(_searchKey)).ToList();
        }

        public IEnumerable<Receipt> GetByPrice(int searchKey, IEnumerable<Receipt> inputList)
        {
            return inputList.Where(b => b.Price <= searchKey).ToList();
        }

        public string RemoveAccents(string input)
        {
            string stFormD = input.Normalize(NormalizationForm.FormD);
            int len = stFormD.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public Receipt GetByReceiptId(int receiptId)
        {
            return _receipts.Include(b => b.Category).Include(b=>b.Seller).SingleOrDefault(b => b.ReceiptId == receiptId && b.Approved);
        }

        public Receipt GetByReceiptIdNotAccepted(int receiptId)
        {
            return _receipts.Include(b => b.Category).Include(b => b.Seller).SingleOrDefault(b => b.ReceiptId == receiptId && !b.Approved);
        }

        public IEnumerable<Receipt> GetTop30(IEnumerable<Receipt> inputList)
        {
            return inputList.OrderByDescending(b => b.QuantityOrdered).Take(30).ToList();
        }

        public IEnumerable<Receipt> GetCouponOfferSlider(IEnumerable<Receipt> inputList)
        {
            return inputList.Where(b => b.Offer == Offer.Slider).ToList();
        }

        public IEnumerable<Receipt> GetReceiptOfferStandard(IEnumerable<Receipt> inputList)
        {
            return inputList.Where(b => b.Offer == Offer.Standard).ToList();
        }

        public IEnumerable<Receipt> GetReceiptOfferStandardAndSlider(IEnumerable<Receipt> inputList)
        {
            return GetCouponOfferSlider(inputList).Union(GetReceiptOfferStandard(inputList)).ToList();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Add(Receipt receipt)
        {
            _receipts.Add(receipt);
        }

        public IEnumerable<Receipt> GetReceiptNotyetApproved(IEnumerable<Receipt> inputList)
        {
            return inputList.OrderBy(h => h.ReceiptId).Where(b => !b.Approved).ToList();
        }

        public IEnumerable<Receipt> GetReceiptApproved(IEnumerable<Receipt> inputList)
        {
            return inputList.OrderBy(h => h.ReceiptId).Where(b => b.Approved).ToList();
        }

        public void Remove(int receiptId)
        {
            Receipt tempReceipt = GetByReceiptIdNotAccepted(receiptId);
            if (tempReceipt == null)
            {
                tempReceipt = GetByReceiptId(receiptId);
            }
            _receipts.Remove(tempReceipt);
        }
        public IEnumerable<Receipt> GetAll()
        {
            return _receipts.Include(b => b.Category).Include(b => b.Seller).OrderByDescending(b => b.QuantityOrdered).AsNoTracking().ToList();
        }
    }
}
