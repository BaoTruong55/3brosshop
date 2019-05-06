using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.HomeViewModels
{
    public class IndexCategoryWithNumberListModel
    {
        public string CategoryName { get; }
        public int Number { get; }
        public string Icon { get; }

        public IndexCategoryWithNumberListModel()
        {
        }

        public IndexCategoryWithNumberListModel(KeyValuePair<Category, int> categoryWithNumber)
        {
            CategoryName = categoryWithNumber.Key.Name;
            Number = categoryWithNumber.Value;
            Icon = categoryWithNumber.Key.Icon;
        }
    }
}
