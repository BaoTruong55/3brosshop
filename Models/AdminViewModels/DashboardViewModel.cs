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

        public int NumberofItemsRequests { get; }

        public int NumberofItemsSold1M { get; }

        public int NumberUsedItems1M { get; }

        public List<DashboardChartViewModel> GraphDataList { get; }

        public IEnumerable<OverviewSoldOrderListViewModel> RecentlySoldList { get; }

        public DashboardViewModel(int numberofSellerRequests, int numberofItemsRequests, IEnumerable<OrderItem> itemsSold1M, IEnumerable<OrderItem> usedItems1M)
        {
            NumberofSellerRequests = numberofSellerRequests;
            NumberofItemsRequests = numberofItemsRequests;
            NumberofItemsSold1M = itemsSold1M.Count();
            NumberUsedItems1M = usedItems1M.Count();

            DateTime nowDate = DateTime.Now.Date;
            nowDate = nowDate.AddMonths(-1);

            RecentlySoldList = itemsSold1M.OrderByDescending(b => b.CreationDate).Take(10).Select(b => new OverviewSoldOrderListViewModel(b)).ToList();
            GraphDataList = new List<DashboardChartViewModel>();

            for (DateTime currentDate = nowDate; currentDate.Date <= DateTime.Today; currentDate = currentDate.AddDays(1))
            {
                string datum = currentDate.ToString("yyyy-MM-dd");
                int amountSold = itemsSold1M.Where(b => b.CreationDate == currentDate).Count();
                int numberUsed = usedItems1M.Where(b => b.ExpirationDate == currentDate).Count();
                GraphDataList.Add(new DashboardChartViewModel(datum, amountSold, numberUsed));
            }
        }


    }
}
