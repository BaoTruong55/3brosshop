using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class DashboardViewModel
    {
        public int NumberofSellerRequests { get; }

        public int NumberofReceiptRequests { get; }

        public int NumberofReceiptSold1M { get; }

        public int NumberUsedReceipt1M { get; }

        public List<DashboardChartViewModel> GraphDataList { get; }

        public IEnumerable<OverviewSoldOrderListViewModel> RecentlySoldList { get; }

        public DashboardViewModel(int numberofSellerRequests, int numberofReceiptRequests, IEnumerable<OrderItem> receiptSold1M, IEnumerable<OrderItem> usedReceipt1M)
        {
            NumberofSellerRequests = numberofSellerRequests;
            NumberofReceiptRequests = numberofReceiptRequests;
            NumberofReceiptSold1M = receiptSold1M.Count();
            NumberUsedReceipt1M = usedReceipt1M.Count();

            DateTime nowDate = DateTime.Now.Date;
            nowDate = nowDate.AddMonths(-1);

            RecentlySoldList = receiptSold1M.OrderByDescending(b => b.CreationDate).Take(10).Select(b => new OverviewSoldOrderListViewModel(b)).ToList();
            GraphDataList = new List<DashboardChartViewModel>();

            for (DateTime currentDate = nowDate; currentDate.Date <= DateTime.Today; currentDate = currentDate.AddDays(1))
            {
                string datum = currentDate.ToString("yyyy-MM-dd");
                int amountSold = receiptSold1M.Where(b => b.CreationDate == currentDate).Count();
                int numberUsed = usedReceipt1M.Where(b => b.ExpirationDate == currentDate).Count();
                GraphDataList.Add(new DashboardChartViewModel(datum, amountSold, numberUsed));
            }
        }


    }
}
