using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ReceiptRequestsViewModel
    {
        public IEnumerable<ReceiptRequestsListViewModel> AllReceiptRequestsSortedById { get; }

        public ReceiptRequestsViewModel(IEnumerable<Receipt> AllReceiptNotYetApproved)
        {
            AllReceiptRequestsSortedById = AllReceiptNotYetApproved.Select(b => new ReceiptRequestsListViewModel(b)).ToList();
        }

    }
}
