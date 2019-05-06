using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ReceiptOverviewViewModel
    {
        public IEnumerable<ReceiptListViewModel> AllReceiptSortedById { get; }

        public ReceiptOverviewViewModel(IEnumerable<Receipt> allReceipt)
        {
            AllReceiptSortedById = allReceipt.Select(c => new ReceiptListViewModel(c)).ToList();
        }

    }
}
