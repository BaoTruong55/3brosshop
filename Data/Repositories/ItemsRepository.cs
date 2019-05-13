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
    public class ItemsRepository : IItemsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Items> _items;
        public ItemsRepository(ApplicationDbContext context)
        {
            _context = context;
            _items = context.Items;
        }
        public IEnumerable<Items> GetAllApproved()
        {
            return GetItemsApproved(_items.Include(b => b.Category).Include(b => b.Seller).Where(b => b.Approved).OrderByDescending(b => b.QuantityOrdered).AsNoTracking().ToList());
        }

        public int getNumberofItemsRequests()
        {
            return _items.Count(b => !b.Approved);
        }

        public IEnumerable<Items> GetAll(string searchKey, IEnumerable<Items> inputList)
        {
            if (searchKey.Trim().Length != 0)
            {
                string[] _words = searchKey.ToLower().Split(' ');
                int _numberofWords = _words.Length;
                List<Items> _advancedSearch = new List<Items>();


                foreach (Items b in inputList)
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

        public IEnumerable<Items> GetByCategory(string searchKey, IEnumerable<Items> inputList)
        {
            string _searchKey = searchKey.ToLower();
            return inputList.Where(b => b.Category.Name.ToLower().Contains(_searchKey)).ToList();
        }

        public IEnumerable<Items> GetByLocation(string searchKey, IEnumerable<Items> inputList)
        {
            string _searchKey = searchKey.ToLower();
            _searchKey = _searchKey.Replace("-", "");
            _searchKey = _searchKey.Replace("_", "");
            _searchKey = RemoveAccents(_searchKey);
            return inputList.Where(b => RemoveAccents(b.City.ToLower().Replace("-", "").Replace("_", "")).Contains(_searchKey)).ToList();
        }

        public IEnumerable<Items> GetByName(string searchKey, IEnumerable<Items> inputList)
        {
            string _searchKey = searchKey.ToLower();
            _searchKey = _searchKey.Replace("-", "");
            _searchKey = _searchKey.Replace("_", "");
            _searchKey = RemoveAccents(_searchKey);
            return inputList.Where(b => RemoveAccents(b.Name.ToLower().Replace("-", "").Replace("_", "")).Contains(_searchKey)).ToList();
        }

        public IEnumerable<Items> GetByPrice(int searchKey, IEnumerable<Items> inputList)
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

        public Items GetByItemsId(int itemsId)
        {
            return _items.Include(b => b.Category).Include(b=>b.Seller).SingleOrDefault(b => b.ItemsId == itemsId && b.Approved);
        }

        public Items GetByItemsIdNotAccepted(int itemsId)
        {
            return _items.Include(b => b.Category).Include(b => b.Seller).SingleOrDefault(b => b.ItemsId == itemsId && !b.Approved);
        }

        public IEnumerable<Items> GetTop30(IEnumerable<Items> inputList)
        {
            return inputList.OrderByDescending(b => b.QuantityOrdered).Take(30).ToList();
        }

        public IEnumerable<Items> GetCouponOfferSlider(IEnumerable<Items> inputList)
        {
            return inputList.Where(b => b.Offer == Offer.Slider).ToList();
        }

        public IEnumerable<Items> GetItemsOfferStandard(IEnumerable<Items> inputList)
        {
            return inputList.Where(b => b.Offer == Offer.Standard).ToList();
        }

        public IEnumerable<Items> GetItemsOfferStandardAndSlider(IEnumerable<Items> inputList)
        {
            return GetCouponOfferSlider(inputList).Union(GetItemsOfferStandard(inputList)).ToList();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Add(Items items)
        {
            _items.Add(items);
        }

        public IEnumerable<Items> GetItemsNotyetApproved(IEnumerable<Items> inputList)
        {
            return inputList.OrderBy(h => h.ItemsId).Where(b => !b.Approved).ToList();
        }

        public IEnumerable<Items> GetItemsApproved(IEnumerable<Items> inputList)
        {
            return inputList.OrderBy(h => h.ItemsId).Where(b => b.Approved).ToList();
        }

        public void Remove(int itemsId)
        {
            Items tempItems = GetByItemsIdNotAccepted(itemsId);
            if (tempItems == null)
            {
                tempItems = GetByItemsId(itemsId);
            }
            _items.Remove(tempItems);
        }
        public IEnumerable<Items> GetAll()
        {
            return _items.Include(b => b.Category).Include(b => b.Seller).OrderByDescending(b => b.QuantityOrdered).AsNoTracking().ToList();
        }
    }
}
