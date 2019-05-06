using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<IndexTop30SheetsListModel> Top30Receipt { get; }

        public IEnumerable<IndexOfferSliderListModel> SliderReceipt { get; }

        public IEnumerable<IndexCategoryWithNumberListModel> Top9Category { get; }

        public IndexViewModel()
        {
        }
        public IndexViewModel(IEnumerable<Receipt> top30Receipt, IEnumerable<Receipt> sliderReceipt, Dictionary<Category, int> top9Category)
        {
            Top30Receipt = top30Receipt.Select(b => new IndexTop30SheetsListModel(b)).ToList();
            SliderReceipt = sliderReceipt.Select(b => new IndexOfferSliderListModel(b)).ToList();
            Top9Category = top9Category.Select(b => new IndexCategoryWithNumberListModel(b)).ToList();
        }
    }
}
