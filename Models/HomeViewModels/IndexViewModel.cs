using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<IndexTop30SheetsListModel> Top30Items { get; }

        public IEnumerable<IndexOfferSliderListModel> SliderItems { get; }

        public IEnumerable<IndexCategoryWithNumberListModel> Top9Category { get; }

        public IndexViewModel()
        {
        }
        public IndexViewModel(IEnumerable<Items> top30Items, IEnumerable<Items> sliderItems, Dictionary<Category, int> top9Category)
        {
            Top30Items = top30Items.Select(b => new IndexTop30SheetsListModel(b)).ToList();
            SliderItems = sliderItems.Select(b => new IndexOfferSliderListModel(b)).ToList();
            Top9Category = top9Category.Select(b => new IndexCategoryWithNumberListModel(b)).ToList();
        }
    }
}
