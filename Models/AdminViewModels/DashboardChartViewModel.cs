using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.AdminViewModels
{
    public class DashboardChartViewModel
    {
        public string Date { get; }
        public int AmountSold { get; }
        public int NumberUsed { get; }

        public DashboardChartViewModel(string date, int amountSold, int numberUsed)
        {
            Date = date;
            NumberUsed = numberUsed;
            AmountSold = amountSold;
        }

    }
}
